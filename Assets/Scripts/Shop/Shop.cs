using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer), typeof(ShopProperties))]
public class Shop : MonoBehaviour {

    public static Shop Instance;

    public List<BasicAI> WaitingQ   { get { return this.waitingQueue; } }
    public List<BasicAI> OrderedQ   { get { return this.orderedQueue; } }
    public Storage StorageState     { get { return _shopStorage; } }
    public int MaxWaitingQueue      { get { return this.maxWaitingCustomers; } }

    public List<BasicAI> waitingQueue;  //FIXME: make it private
    public List<BasicAI> orderedQueue;  //FIXME: make it private

    public Recepe RecepeToServe;

    protected SpriteRenderer    _spriteRenderer;
    protected BoxCollider2D     _boxCollider;
    protected ShopProperties    _shopProps;
    protected Dictionary<BasicAI, Recepe> cooking;
    protected Storage _shopStorage;
    protected bool bIsEnoughIngredients { get { return ValidateStockForRecepe(); } }

    private int maxWaitingCustomers;


    /* ----------------------------------------------------------------------- */


    public void Start() {
        Instance = this;
        this.initComponents();
        _shopStorage = GameManager.Instance.GlobalStorage;
        _shopProps = GetComponent<ShopProperties>();
        Reset();
    }//Start


    private void initComponents() {
        if (_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_boxCollider == null)
            _boxCollider = GetComponent<BoxCollider2D>();
    }//InitComponents


    public void Reset() {
        waitingQueue = new List<BasicAI>();
        orderedQueue = new List<BasicAI>();
        cooking = new Dictionary<BasicAI, Recepe>();
    }//Reset

    public void LateUpdate() {
        if (orderedQueue.Count < _shopProps.MaxOrders)
            TakeOrder();
        if(orderedQueue.Count > 0)
            CookAndServe();
    }//Update


    public void RemoveFromWaitQueue(BasicAI pedestrian) {
        if (waitingQueue.Contains(pedestrian))
            waitingQueue.Remove(pedestrian);
    }//RemoveFromWaitQueue


    /// <summary>
    ///  Take order from the clinet (in the queue). Cook the
    /// food for Food.cooktime seconds and serve it when ready.
    /// </summary>
    public void TakeOrder() {
        if (orderedQueue.Count >= 2)
            return;
        if (waitingQueue.Count == 0)
            return;

        var client = waitingQueue[0];
        RemoveFromWaitQueue(client);

        if (!bIsEnoughIngredients) {
            client.DeclineService();
            return;
        }//if

        _shopStorage.Substruct(RecepeToServe);
        //dispose food for the client if he ordered before, 
        //but food was not removed from the grill.
        DisposeCooked(client);

        orderedQueue.Add(client);
        Recepe foodToCook = new Recepe(RecepeToServe);

        cooking.Add(client, foodToCook);

        client.SetState(BasicAI.StateMachine.waitingForOrder);
    }//TakeOrder


    public void ServeClient(BasicAI client, Recepe toServe) {
        if (client.CurrentState != BasicAI.StateMachine.waitingForOrder) {
            GameUtils.Utils.WarningMessage(client.name + 
                " is not waiting-for-order while ServeClient is called! [" + client.CurrentState + "]");
        }

        float cash = client.RecieveOrder(toServe);
        _shopStorage.Cash.Add(cash);
        if (orderedQueue.Contains(client)) {
            orderedQueue.Remove(client);
        }
    }//ServeClient


    public void CookAndServe() {
        List<BasicAI> toRemove = new List<BasicAI>();
        for (int i = 0; i < orderedQueue.Count; i++) {
            BasicAI client = orderedQueue[i];
            if (client.CurrentState != BasicAI.StateMachine.waitingForOrder) {
                ClientWalkedAway(client);
                continue;
            }
            if (!cooking.ContainsKey(client)) {
                GameUtils.Utils.WarningMessage(client.name + " has no cooking food?!");
                continue;
            }//if
            Recepe foodOnTheGrill = cooking[client];
            bool isCooked = foodOnTheGrill.Cook(Time.deltaTime);

            if (i == 0) { //try fastcook only for the first in queue
                bool isFastCook = _shopProps.TryFaskCook();
                if (isFastCook) {
                    Debug.Log("Fast Cook worked on " + client.name);
                    isCooked = true;
                }
            }//of first in line

            if (isCooked) {
                toRemove.Add(client);
                ServeClient(client, foodOnTheGrill);
            }//if cooked
        }//for

        foreach(BasicAI served in toRemove) {
            if (!cooking.ContainsKey(served))
                continue;
            cooking.Remove(served);
        }//foreach
    }//Cook


    /// <summary>
    ///  Check if there is enough ingredients in the storage to put an
    /// order on the grill.
    /// </summary>
    /// <returns>True - life is good. False - not enough ingredients.</returns>
    public bool ValidateStockForRecepe() {
        if (_shopStorage.Brains.Count < RecepeToServe.Brains.Count)
            return false;
        if (_shopStorage.Seasonings.Count < RecepeToServe.Seasonings.Count)
            return false;
        if (_shopStorage.Drinks.Count < RecepeToServe.Seasonings.Count)
            return false;

        return true;
    }//IsThereEnoughIngredients


    /// <summary>
    ///  Remove food from the grill for the given client.
    /// </summary>
    /// <param name="ofClient"></param>
    public void DisposeCooked(BasicAI ofClient) {
        if (cooking.ContainsKey(ofClient))
            cooking.Remove(ofClient);
    }//DisposeCooked


    public void ClientWalkedAway(BasicAI client) {
        if(this.waitingQueue.Contains(client))
            this.waitingQueue.Remove(client);
        if(this.orderedQueue.Contains(client))
            this.orderedQueue.Remove(client);
    }//ClientWalkedAway


    public void SetStorage(Storage st) { this._shopStorage = st; }


    public void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag.ToLower() != "pedestrian")
            return;

        BasicAI pedestrian = collision.GetComponent<BasicAI>();
        if (pedestrian == null)
            return;

        bool isPriceGood = District.Instance.TryAttactByPrice(RecepeToServe.Cash.Count);

        if (!isPriceGood) {
            Debug.Log("Bad Price!");
            return;
        }//if bad price

        if (_shopProps.TryAttractByChance() == false)
            return;

        pedestrian.SetState(BasicAI.StateMachine.standingInLine);
        waitingQueue.Add(pedestrian);

        if(waitingQueue.Count > this.maxWaitingCustomers)
            this.maxWaitingCustomers = waitingQueue.Count;
    }//OnTriggerEnter2D


    public void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }//OnEnable


    public void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }//OnDisable


    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        this.initComponents();
        var activeScene = SceneManager.GetActiveScene();

        if (activeScene.buildIndex != 0) {
            _spriteRenderer.enabled = true;
            _boxCollider.enabled    = true;
        } else {
            _spriteRenderer.enabled = false;
            _boxCollider.enabled    = false;
        }//if-else

        Reset();
    }//OnSceneLoaded

}//class