using UnityEngine;


public class District: MonoBehaviour {

    public static District Instance;

    //[Tooltip("Prefered recepe by this district.")]
    //public RecepePreference     RecepePref;
    [Tooltip("Prefered price range for this district.")]
    public PricePreference      PricePref;
    [Tooltip("How much time customers are willing to wait.")]
    public WaitTimePreference   WaitTimePref;
    [Tooltip("How much customers are willing to tip in this district.")]
    [Range(0.0f, 1.0f)] public float MaxTips = 0.1f;
    [Tooltip("Max reputation value available for this level.")]
    public float MaxReputationValue = 100;
    //used by BasicAI to get randoim wait value
    public Vector2 MaxWaitTime = new Vector2(4.5f, 6f);
    public ConsumerRating Ratings;
    public float ReputationStatus { get { return this.reputation; } }

    private float reputation;


    public void Start() {
        Instance = this;
    }//Start


    public bool TryAttactByPrice(float target) {
        float priceMismatch = 0;
        if (target > PricePref.Price)
            priceMismatch = target - PricePref.Price;
        else
            return true;
        float penalty = (priceMismatch / PricePref.MismatchPenaltyRatio) * PricePref.PenaltyPercent;
        int maxChance = 100 - Mathf.FloorToInt(penalty * 100);
        int chance = Random.Range(1, 100);
        return chance >= 1 && chance <= maxChance;
    }//TryAttactByPrice


    /// <summary>
    /// 
    /// </summary>
    /// <param name="received">Recepe that client bought.</param>
    /// <param name="timeWaited">How long did he wait in line.</param>
    /// <returns></returns>
    public ConsumerRating GetSatisfactionRatio(ConsumerRating prefs, Recepe received, float timeWaited) {
       var feedback = prefs.GetSatisfactionRatio(received, timeWaited);

        AddReputation(feedback.Points);
        return feedback;
    }//GetSatisfactionRatio


    public void AddReputation(float amount) {
        reputation += amount;
        reputation = reputation < 0 ? 0 : reputation;
        reputation = reputation > MaxReputationValue ? MaxReputationValue : reputation;
    }//AddReputation


    public void ReduceReputation(float amount) {
        AddReputation(-amount);
    }//ReduceReputation
}//class