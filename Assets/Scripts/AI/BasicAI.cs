using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : MonoBehaviour {

    public enum StateMachine { walking, movingToQueue, standingInLine, waitingForOrder }
    public float    Speed = 0.4f;
    [Tooltip("Random time between X and Y this pedestrian will wait in line.")]
    public Vector2  MaxWaitingTime = new Vector2(1, 2);
    public Vector2  velocity;
    public string   POIName = "FoodTrack";
    public float    Gravity = 0.98f;
    public Recepe[]   FoodPref;

    protected StateMachine          eState;
    protected CollisionDetection    _collision;
    protected Shop                  _shop; //point of interest
    protected GameObject            leftWall, rightWall;
    protected int                   moveDirection;
    protected float                 waitTime;
    protected float                 currentWaitTime;
    protected Vector3               destination;
    protected Recepe                  foodToOrder;
    protected ThoughtProcessor      _thoughtsProcessor;

    void Start () {
        _collision  = GetComponent<CollisionDetection>();
        _shop       = GameObject.Find(POIName).GetComponent<Shop>();
        leftWall    = GameObject.Find("leftWall");
        rightWall   = GameObject.Find("rightWall");
        _thoughtsProcessor   = GetComponentInChildren<ThoughtProcessor>();

        moveDirection   = MoveTowardsDirection();
        currentWaitTime = 0;
        waitTime        = 0;
        eState          = StateMachine.walking;

        _thoughtsProcessor.HideAll();
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

        //Switch movement direction when hitting left of right wall
        if (moveDirection == -1 && _collision.left) this.moveDirection = 1;
        if (moveDirection == 1 && _collision.right) this.moveDirection = -1;

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
            _thoughtsProcessor.ShowFeedback(FeedbackThoughts.Feedbacks.Angry, true);
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


    /// <summary>
    ///  Return a Food object to be cooked for this client.
    /// </summary>
    /// <returns></returns>
    public Recepe GetOrder() {
        this.eState = StateMachine.waitingForOrder;
        _thoughtsProcessor.ShowFoodChoice(foodToOrder.Icon, true);
        return foodToOrder;
    }//Order


    public float RecieveOrder(Recepe toRecieve) {
        this.eState = StateMachine.walking;

        _thoughtsProcessor.ShowFeedback(FeedbackThoughts.Feedbacks.Happy, true);
        //TODO: calculate satisfaction value and tip percent
        return 1;
    }//RecieveOrder


    public int MoveTowardsDirection() {
        Vector3 leftWallDist = leftWall.transform.position - this.transform.position;
        Vector3 rightWallDist = rightWall.transform.position - this.transform.position;
        return leftWallDist.magnitude > rightWallDist.magnitude ?  -1 : 1;
    }//MoveTowardsDirection


    public void SetState(StateMachine newState) {
        eState = newState;
        if(eState == StateMachine.standingInLine) {
            foodToOrder = (FoodPref.Length == 0) ?_shop.PickAnyFromMenu() : foodToOrder = pickFoodFromPrefs();
            if (foodToOrder == null) {
                eState = StateMachine.walking;
                return;
            }
            waitTime = Random.Range(MaxWaitingTime.x, MaxWaitingTime.y);
        }
    }//SetState


    protected Recepe pickFoodFromPrefs() {
        foreach(Recepe choice in FoodPref) {
            if (!_shop.CheckFoodInMenu(choice))
                continue;
            return choice;
        }
        return null;
    }//pickFoodFromPrefs

}//class
