using UnityEngine;


public class BasicAI : MonoBehaviour {

    public enum StateMachine { walking, movingToQueue, standingInLine, waitingForOrder }
    public float    Speed = 0.4f;
    public Vector2  SpeedRange = new Vector2(6f, 7f);
    public Vector2  VelocityRange  = new Vector2(0.2f, 1.0f);
    public Vector2  velocity;
    public string   POIName = "FoodTruck";
    public float    Gravity = 0.98f;
    public ConsumerRating ServicePrefs;
    public StateMachine CurrentState    { get { return this.eState; } }
    public float TimeWaitedInLine       { get { return this.timeStandedInLine; } }
    public float TimeWaitedForOrder     { get { return this.timeWaitedForOrder; } }
    public float TotalWaitTime          { get { return TimeWaitedForOrder + TimeWaitedInLine; } }
    public float MaxWaitInLine {
        get {
            if (this.maxWaitInLineTime <= 0) {
                this.maxWaitInLineTime = Random.Range(District.Instance.MaxWaitTime.x,
                                                District.Instance.MaxWaitTime.y);
            }//if
            return this.maxWaitInLineTime;
        }//get
    }//MaxWaitTime

    protected StateMachine          eState;
    protected GameObject            leftWall, rightWall;
    protected int                   moveDirection;
    //protected float                 waitTime;
    protected float                 timeStandedInLine;
    protected float                 timeWaitedForOrder;
    protected Vector3               destination;
    protected CollisionDetection    _collision;
    protected Shop                  _shop; //point of interest
    protected ThoughtProcessor      _thoughtsProcessor;
    protected Recepe                 foodToOrder;
    protected ConsumerRating        _serviceFeedback;
    protected District              _district;
    protected SpriteRenderer        _spriteRenderer;

    //Random time between X and Y this pedestrian will wait in line.
    //private Vector2  maxWaitTime;
    private float maxWaitInLineTime;


    void Start () {
        _collision  = GetComponent<CollisionDetection>();
        _shop       = GameObject.Find(POIName).GetComponent<Shop>();
        leftWall    = GameObject.Find("leftWall");
        rightWall   = GameObject.Find("rightWall");
        _thoughtsProcessor  = GetComponentInChildren<ThoughtProcessor>();
        _spriteRenderer     = GetComponent<SpriteRenderer>();
        _district = District.Instance;
        _serviceFeedback = null;

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
        if (CurrentState != StateMachine.walking)
            return;

        deltaMovement.x = Speed * moveDirection * Time.deltaTime;
        _collision.Move(ref deltaMovement);

        if(_collision.left || _collision.right) {
            this.gameObject.SetActive(false);
            return;
        }

        //Hide thoughts when reaching destination\walls
        if (_collision.left || _collision.right)
            _thoughtsProcessor.HideAll();

        transform.Translate(deltaMovement);
    }//WalkingState


    public void StaindingInLine(ref Vector2 deltaMovement) {
        if (CurrentState != StateMachine.standingInLine)
            return;
        if (CurrentState == StateMachine.standingInLine)
            deltaMovement.x = 0;

        this.timeStandedInLine += Time.deltaTime;

        if (this.timeStandedInLine >= MaxWaitInLine) {
            this.eState = StateMachine.walking;
            this.maxWaitInLineTime = -1;
            _thoughtsProcessor.ShowFeedback(Rating.Grade.F, true);
            _shop.ClientWalkedAway(this);
        }

        _thoughtsProcessor.ShowAction(ActionThoughts.Actions.StandingInLine, true);
    }//WaitingState


    public void WaitingForOrderState(ref Vector2 deltaMovement) {
        if (CurrentState != StateMachine.waitingForOrder)
            return;

        this.timeWaitedForOrder += Time.deltaTime;
        _thoughtsProcessor.ShowAction(ActionThoughts.Actions.WaitingForOrder, true);
        //_thoughtsProcessor.ShowFoodChoice(foodToOrder.Icon, true);
    }//WaitingForOrderState


    public void DeclineService() {
        //_thoughtsProcessor.ShowFeedback(Rating.Grade.F, true);
        eState = StateMachine.walking;
        _shop.ClientWalkedAway(this);
    }//DeclineService


    public float RecieveOrder(Recepe order) {
        if (CurrentState != StateMachine.waitingForOrder) {
            GameUtils.Utils.WarningMessage(this.name + " trying to recieve order " + 
                            order.ToString() + " while " + CurrentState.ToString());
            return 0;
        }
        this.eState = StateMachine.walking;
        
        _serviceFeedback = _district.MakeFeedback(ServicePrefs, order, timeStandedInLine);

        float satisfaction = (float)_serviceFeedback.ServiceGrade;
        _thoughtsProcessor.ShowFeedback(ServicePrefs.ServiceGrade, true);
        float tipAmount = (satisfaction / 100) * order.Cash.Count;

        //Show tip amount as a floating text object
        if(GameManager.Instance.FloatingTextPrefab != null) {
            var floatingTextGO = Instantiate(GameManager.Instance.FloatingTextPrefab);
            var floatingTextCmp = floatingTextGO.GetComponent<FloatingText>();
            floatingTextCmp.ShowText(satisfaction + "! $" + tipAmount, this.transform.position);
        }//if floating text

        if (LevelStats.Instance != null) {
            ConsumerReport cr = new ConsumerReport(_serviceFeedback, this);
            LevelStats.Instance.AddReport(cr);
        }

        return (float)System.Math.Round(order.Cash.Count + tipAmount, 2);
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
            foodToOrder = _shop.RecepeToServe;
            if (foodToOrder == null) {      //PARANOIA??? Why would that ever happened.
                eState = StateMachine.walking;
                GameUtils.Utils.WarningMessage(this.name + " received a null recipe obj!");
                return;
            }

            //Vector2 waittimeRange = District.Instance.MaxWaitTime;
            //this.maxWaitTime = Random.Range(waittimeRange.x, waittimeRange.y);
            //waitTime = Random.Range(maxWaitTime.x, maxWaitTime.y);
        }//if standing in line
    }//SetState


    public void Respawn() {
        this.moveDirection = MoveTowardsDirection(true);
        timeStandedInLine = 0;
        timeWaitedForOrder = 0;
        this.maxWaitInLineTime = -1; //value not yet being randomized based on District
        velocity = Vector2.zero;
        Speed = Random.Range(SpeedRange.x, SpeedRange.y);
        eState = StateMachine.walking;
        _serviceFeedback = null;

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

        _spriteRenderer.color = new Color(Random.value, Random.value, Random.value);
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


    public void OnMouseDown() {
        if (_serviceFeedback == null)
            return;
        Debug.Log(_serviceFeedback.GetLowestGrade());
    }
}//class
