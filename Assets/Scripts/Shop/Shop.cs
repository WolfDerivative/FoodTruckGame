using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Shop : MonoBehaviour {

    public static Shop Instance;

    [Range(0, 1)]public float ChanceOfAttraction = 0.5f;
    public ShopStorage shopStorage;
    public int MaxOrders = 2;
    public int MaxPrepedFood = 2;
    public List<BasicAI> WaitingQ       { get { return this.waitingQueue; } }
    public List<BasicAI> OrderedQ   { get { return this.orderedQueue; } }

    public List<BasicAI> waitingQueue;  //FIXME: make it private
    public List<BasicAI> orderedQueue;  //FIXME: make it private
    //public List<Food> FoodMenu;
    public Recepe Recepe;
    
    protected Dictionary<BasicAI, Recepe> cooking;


    public void Start() {
        Instance        = this;
        waitingQueue    = new List<BasicAI>();
        orderedQueue    = new List<BasicAI>();
        cooking         = new Dictionary<BasicAI, Recepe>();
    }//Start


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

        orderedQueue.Add(client);
        //dispose food for the client if he ordered before, 
        //but food was not removed from the grill.
        DisposeCooked(client);
        Recepe foodToCook = new Recepe();

        cooking.Add(client, foodToCook);
    }//TakeOrder


    public void ServeClient(BasicAI client, Recepe toServe) {
        float cash = client.RecieveOrder(toServe);
        shopStorage.Cash += cash;
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
    ///  Remove food from the grill for the given client.
    /// </summary>
    /// <param name="ofClient"></param>
    public void DisposeCooked(BasicAI ofClient) {
        if (cooking.ContainsKey(ofClient))
            cooking.Remove(ofClient);
    }//DisposeCooked


    /// <summary>
    ///  Check if desiered food is in the shop's menu.
    /// Return True if recepe exists. False = no such food.
    /// </summary>
    /// <param name="toFind">Food to find in the menu</param>
    /// <returns></returns>
    public bool CheckFoodInMenu(Recepe toFind) {
        //FIXME
        return true;
    }//CheckFoodInMenu


    public Recepe PickAnyFromMenu() {
        return Recepe;
    }//PickAnyFromMenu


    public void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag.ToLower() != "pedestrian")
            return;

        BasicAI pedestrian = collision.GetComponent<BasicAI>();
        if (pedestrian == null)
            return;

        bool isPriceGood = District.Instance.TryAttactByPrice(Recepe.Price);

        if (!isPriceGood) {
            Debug.Log("Bad Price!");
            return;
        }

        bool isRecepeGood = District.Instance.TryAttractByRecepe(
                            ref Recepe.Brains, ref Recepe.Seasoning, ref Recepe.Drinks);
        if (!isRecepeGood) {
            Debug.Log("Bad Recepe!");
            return;
        }
        
        pedestrian.SetState(BasicAI.StateMachine.standingInLine);
        waitingQueue.Add(pedestrian);
    }//OnTriggerEnter2D

}//class
