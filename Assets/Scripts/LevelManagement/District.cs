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
    //used by BasicAI to get randoim wait value
    public Vector2 MaxWaitTime = new Vector2(4.5f, 6f);

    public ConsumerRating Ratings;


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
    public ConsumerRating GetSatisfactionRatio(Recepe received, float timeWaited) {
        Ratings.BrainsRating.RatioByRange(received.Brains.Count);

        Ratings.SeasoningsRating.RatioByRange(received.Seasonings.Count);
        Ratings.DrinksRating.RatioByRange(received.Drinks.Count);

        Ratings.WaitTimeRating.RatioByRange(timeWaited);

        return Ratings;
    }//GetSatisfactionRatio


}//class