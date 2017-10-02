using UnityEngine;


public class District: MonoBehaviour {

    public static District Instance;

    [Tooltip("Prefered recepe by this district.")]
    public RecepePreference     RecepePref;
    [Tooltip("Prefered price range for this district.")]
    public PricePreference      PricePref;
    [Tooltip("How much time customers are willing to wait.")]
    public WaitTimePreference   WaitTimePref;
    [Tooltip("How much customers are willing to tip in this district.")]
    [Range(0.0f, 1.0f)] public float MaxTips = 0.1f;
    //public ConsumerRating FoodQalityRating;
    public ConsumerRating BrainsRating, SeasoningsRating, DrinksRating;
    public ConsumerRating WaitTimeRating;
    //public ConsumerRating PriceRating;


    public void Start() {
        Instance = this;
    }//Start


    /*
    public bool TryAttractByRecepe(ref Brain b, ref Seasoning s, ref Drink d) {
        int chance = 0;
        if (b.Count >= RecepePref.BrainsRange.x && b.Count <= RecepePref.BrainsRange.y)
            chance += 35;
        if (s.Count >= RecepePref.SeasoningRange.x && s.Count <= RecepePref.SeasoningRange.y)
            chance += 35;
        if (d.Count >= RecepePref.DrinksRange.x && d.Count <= RecepePref.DrinksRange.y)
            chance += 35;
        int randomValue = Random.Range(1, 100);
        return randomValue >= 1 && randomValue <= chance;
    }//TryAttractByRecepe
    */

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
    public float GetSatisfactionRatio(Recepe received, float timeWaited) {
        float total = 0;
        total += BrainsRating.RatioByRange(RecepePref.BrainsRange, received.Brains.Count);
        total += SeasoningsRating.RatioByRange(RecepePref.SeasoningRange, received.Seasoning.Count);
        total += DrinksRating.RatioByRange(RecepePref.DrinksRange, received.Drinks.Count);

        //float maxPrice = (PricePref.Price * PricePref.SatisfactionSwing) + PricePref.Price;
        //total += PriceRating.RatioByMedian(received.Price, PricePref.Price, maxPrice);

        total += WaitTimeRating.RatioByRange(WaitTimePref.WaitTime, timeWaited, true);
        if (total < 0)
            total = 0;
        return total;
    }//GetSatisfactionRatio


}//class

[System.Serializable]
public class ConsumerRating {
    public float Bad = -1;
    public float Good = 1;
    public float Excelent = 2;

    public float RatioByRange(Vector2 range, float recieved, bool isReverse=false) {
        int median = Mathf.CeilToInt((range.x + range.y) / 2);

        if (recieved < range.x || recieved > range.y) //bellow or above range
            return (!isReverse) ? Bad : Excelent;

        if (recieved >= range.x && recieved < median) //Bellow median
            return Good;

        if (recieved >= median && recieved <= range.y) //Above median
            return (!isReverse) ? Excelent : Bad;

        return 0;
    }//SatisfactionValue


    /// <param name="paid">How much customer paid for food.</param>
    /// <param name="price">Min price.</param>
    /// <param name="maxPrice">Max "reasonable" price willing to pay.</param>
    /// <returns></returns>
    public float RatioByMedian(float paid, float price, float maxPrice) {
        float median = Mathf.CeilToInt((price + maxPrice) / 2);

        if (paid > maxPrice) //abouve max price
            return Bad;

        if (paid < median && paid > price) //Bellow median
            return Good;

        if (paid >= median && paid <= maxPrice) //Above median
            return Excelent;

        return 0;
    }//SatisfactionRatio
}//Rating class


[System.Serializable]
public class RecepePreference {
    public Vector2 BrainsRange      = new Vector2(1f, 2f);
    public Vector2 SeasoningRange   = new Vector2(1f, 1f);
    public Vector2 DrinksRange      = new Vector2(1f, 3f);
}//class RecepePreference


[System.Serializable]
public class PricePreference {
    public float Price = 5.0f;
    [Tooltip("[(Price * Satistaction) + Price] Highest price to satisfy a customer.")]
    [Range(0.0f, 1.0f)]public float SatisfactionSwing = 0.25f;
    [Tooltip("For every MismatchPenaltyRatio amount the desired price doesn't match shop's price.")]
    public float MismatchPenaltyRatio = 0.15f;
    [Tooltip("Reduce attraction chance by PenaltyPercent when desiered price doesn't match shop's price.")]
    [Range(0.0f, 1.0f)]public float PenaltyPercent = 0.1f;
}//PricePreference class


[System.Serializable]
public class WaitTimePreference {
    [Tooltip("Range between X and Y is a Good satisfaction rate. Bellow X is excelent..")]
    public Vector2 WaitTime = new Vector2(2f, 3f);
}//PricePreference class