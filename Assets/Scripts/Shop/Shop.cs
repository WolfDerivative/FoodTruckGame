using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
public class Shop : MonoBehaviour {

    public static Shop Instance;

    [Range(0, 1)]public float ChanceOfAttraction = 0.5f;
    public int MaxOrders = 2;
    public int MaxPrepedFood = 2;
    public List<BasicAI> WaitingQ   { get { return this.waitingQueue; } }
    public List<BasicAI> OrderedQ   { get { return this.orderedQueue; } }

    public List<BasicAI> waitingQueue;  //FIXME: make it private
    public List<BasicAI> orderedQueue;  //FIXME: make it private
    //public List<Food> FoodMenu;
    public Recepe RecepeToServe;

    protected SpriteRenderer _spriteRenderer;
    protected BoxCollider2D _boxCollider;
    protected Dictionary<BasicAI, Recepe> cooking;
    protected Storage _shopStorage;
    protected bool bIsEnoughIngredients { get { return ValidateStockForRecepe(); } }


    /* ----------------------------------------------------------------------- */


    public void Start() {
        Instance        = this;
        this.initComponents();
        waitingQueue    = new List<BasicAI>();
        orderedQueue    = new List<BasicAI>();
        cooking         = new Dictionary<BasicAI, Recepe>();
        _shopStorage    = GameManager.Instance.GlobalStorage;
    }//Start


    private void initComponents() {
        if (_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_boxCollider == null)
            _boxCollider = GetComponent<BoxCollider2D>();
    }//InitComponents


    public void Update() {
        if (orderedQueue.Count < MaxOrders)
            TakeOrder();
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

        orderedQueue.Add(client);
        //dispose food for the client if he ordered before, 
        //but food was not removed from the grill.
        DisposeCooked(client);
        Recepe foodToCook = new Recepe(RecepeToServe);

        cooking.Add(client, foodToCook);
    }//TakeOrder


    public void ServeClient(BasicAI client, Recepe toServe) {
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
            if (!cooking.ContainsKey(client)) {
                GameUtils.Utils.WarningMessage(client.name + " has no cooking food?!");
                continue;
            }//if
            Recepe foodOnTheGrill = cooking[client];
            bool isCooked = foodOnTheGrill.Cook(Time.deltaTime);
            if (isCooked) {
                toRemove.Add(client);
                ServeClient(client, foodOnTheGrill);
            }//if cooked
        }//for
        foreach(BasicAI served in toRemove) {
            if (!cooking.ContainsKey(served))
                continue;
            //Destroy(cooking[served].gameObject);
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


    /// <summary>
    ///  Return currently server by the shop recepe.
    /// </summary>
    public Recepe GetRecepe() {
        return RecepeToServe;
    }//PickAnyFromMenu


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
        }

        pedestrian.SetState(BasicAI.StateMachine.standingInLine);
        waitingQueue.Add(pedestrian);
    }//OnTriggerEnter2D


    public void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

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
    }//OnSceneLoaded

}//class