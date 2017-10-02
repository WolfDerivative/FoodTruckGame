using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : MonoBehaviour {

    public enum StateMachine { walking, movingToQueue, standingInLine, waitingForOrder }
    public float    Speed = 0.4f;
    public Vector2  SpeedRange = new Vector2(6f, 7f);
    [Tooltip("Random time between X and Y this pedestrian will wait in line.")]
    public Vector2  MaxWaitingTime = new Vector2(1, 2);
    public Vector2  VelocityRange  = new Vector2(0.2f, 1.0f);
    public Vector2  velocity;
    public string   POIName = "FoodTruck";
    public float    Gravity = 0.98f;
    public Recepe[]   FoodPref;

    protected StateMachine          eState;
    protected GameObject            leftWall, rightWall;
    protected int                   moveDirection;
    protected float                 waitTime;
    protected float                 currentWaitTime;
    protected Vector3               destination;
    protected CollisionDetection    _collision;
    protected Shop                  _shop; //point of interest
    protected ThoughtProcessor      _thoughtsProcessor;
    protected Recepe                 foodToOrder;


    void Start () {
        _collision  = GetComponent<CollisionDetection>();
        _shop       = GameObject.Find(POIName).GetComponent<Shop>();
        leftWall    = GameObject.Find("leftWall");
        rightWall   = GameObject.Find("rightWall");
        _thoughtsProcessor   = GetComponentInChildren<ThoughtProcessor>();

        Respawn();
    }//start
	
	
	void FixedUpdate () {
        Vector2 deltaMovement = velocity;

        WalkingState(ref deltaMovement);
        StaindingInLine(ref deltaMovement);
        WaitingForOrderState(ref deltaMovement);

        velocity = deltaMovement;
    }//update


    public void WalkingState(ref Vector2 deltaMovement) {
        if (eState != StateMachine.walking)
            return;

        deltaMovement.x = Speed * moveDirection * Time.deltaTime;
        _collision.Move(ref deltaMovement);

        if(_collision.left || _collision.right) {
            this.gameObject.SetActive(false);
            return;
        } 

        if (_collision.left || _collision.right)  //Hide thoughts when reaching destination\walls
            _thoughtsProcessor.HideAll();

        transform.Translate(deltaMovement);
    }//WalkingState


    public void StaindingInLine(ref Vector2 deltaMovement) {
        if (eState != StateMachine.standingInLine)
            return;
        if (eState == StateMachine.standingInLine)
            deltaMovement.x = 0;
        currentWaitTime += Time.deltaTime;
        if (currentWaitTime >= waitTime) {
            this.eState = StateMachine.walking;
            waitTime = currentWaitTime = 0;
            _thoughtsProcessor.ShowFeedback(-1, true);
            _shop.RemoveFromWaitQueue(this);
        }

        _thoughtsProcessor.ShowAction(ActionThoughts.Actions.StandingInLine, true);
    }//WaitingState


    public void WaitingForOrderState(ref Vector2 deltaMovement) {
        if (eState != StateMachine.waitingForOrder)
            return;

        _thoughtsProcessor.ShowAction(ActionThoughts.Actions.WaitingForOrder, true);
        _thoughtsProcessor.ShowFoodChoice(foodToOrder.Icon, true);
    }//WaitingForOrderState


    public void DeclineService() {
        _thoughtsProcessor.ShowFeedback(-1, true);
        eState = StateMachine.walking;
    }//DeclineService



    /// <summary>
    ///  Return a Food object to be cooked for this client.
    /// </summary>
    /// <returns></returns>
    public Recepe TakeOrder() {
        this.eState = StateMachine.waitingForOrder;
        _thoughtsProcessor.ShowFoodChoice(foodToOrder.Icon, true);
        return foodToOrder;
    }//Order


    public float RecieveOrder(Recepe order) {
        this.eState = StateMachine.walking;

        //TODO: calculate satisfaction value and tip percent
        float satisfaction = District.Instance.GetSatisfactionRatio(order, currentWaitTime);
        _thoughtsProcessor.ShowFeedback(satisfaction, true);
        float tipAmount = (satisfaction / 100) * order.Price;

        //Show tip amount as a floating text object
        if(GameManager.Instance.FloatingTextPrefab != null) {
            var floatingTextGO = Instantiate(GameManager.Instance.FloatingTextPrefab);
            var floatingTextCmp = floatingTextGO.GetComponent<FloatingText>();
            floatingTextCmp.ShowText(satisfaction + "! $" + tipAmount, this.transform.position);
        }//if floating text

        return (float)System.Math.Round(order.Price + tipAmount, 2);
    }//RecieveOrder


    public int MoveTowardsDirection(bool isRandom=false) {
        if (isRandom) {
            int[] dirChoice = new int[] { -1, 1 };
            return dirChoice[Random.Range(0, 2)];
        }//if random
        Vector3 leftWallDist = leftWall.transform.position - this.transform.position;
        Vector3 rightWallDist = rightWall.transform.position - this.transform.position;
        return leftWallDist.magnitude > rightWallDist.magnitude ?  -1 : 1;
    }//MoveTowardsDirection


    public void SetState(StateMachine newState) {
        eState = newState;
        if(eState == StateMachine.standingInLine) {
            foodToOrder = _shop.GetRecepe();
            if (foodToOrder == null) {      //PARANOIA??? Why would that ever happened.
                eState = StateMachine.walking;
                return;
            }
            waitTime = Random.Range(MaxWaitingTime.x, MaxWaitingTime.y);
        }//if standing in line
    }//SetState


    public void Respawn() {
        this.moveDirection = MoveTowardsDirection(true);
        currentWaitTime = 0;
        waitTime = 0;
        velocity = Vector2.zero;
        Speed = Random.Range(SpeedRange.x, SpeedRange.y);
        eState = StateMachine.walking;

        _thoughtsProcessor.HideAll();

        Vector3 spawnPosition = Vector3.zero;

        if(this.moveDirection < 0) {
            spawnPosition = rightWall.transform.position;
            spawnPosition.x -= rightWall.GetComponent<BoxCollider2D>().size.x;
        }else if(this.moveDirection > 0) {
            spawnPosition = leftWall.transform.position;
            spawnPosition.x += leftWall.GetComponent<BoxCollider2D>().size.x / 2;
        }
        this.transform.position = spawnPosition;
    }//Reset


    public void OnEnable() {
        if (_collision == null)
            Start();
        else
            Respawn();
    }//OnEnable


    public virtual void OnDisable() {
        CancelInvoke();
    }//OnDisable


    public virtual void Destroy() {
        this.gameObject.SetActive(false);
    }//OnDestroy

}//class
