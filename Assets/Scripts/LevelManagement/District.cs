using UnityEngine;


public class District : MonoBehaviour {

    public static District Instance;

    [Tooltip("Prefered recepe by this district.")]
    public RecepePreference     RecepePref;
    [Tooltip("Prefered price range for this district.")]
    public PricePreference      PricePref;
    [Tooltip("How much time customers are willing to wait.")]
    public WaitTimePreference   WaitTimePref;


    public void Start() {
        Instance = this;
    }//Start


    public bool TryAttractByRecepe(ref Brain b, ref Seasoning s, ref Drink d) {
        int chance = 0;
        if (b.Count >= RecepePref.BrainsRange.x && b.Count <= RecepePref.BrainsRange.y)
            chance += 35;
        if(s.Count >= RecepePref.SeasoningRange.x && s.Count <= RecepePref.SeasoningRange.y)
            chance += 35;
        if (d.Count >= RecepePref.DrinksRange.x && d.Count <= RecepePref.DrinksRange.y)
            chance += 35;
        int randomValue = Random.Range(1, 100);
        return randomValue >= 1 && randomValue <= chance;
    }//TryAttractByRecepe


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

}//class


[System.Serializable]
public class RecepePreference{
    public Vector2 BrainsRange      = new Vector2(1f, 2f);
    public Vector2 SeasoningRange   = new Vector2(1f, 1f);
    public Vector2 DrinksRange      = new Vector2(1f, 3f);
}//class RecepePreference


[System.Serializable]
public class PricePreference {
    public float Price = 5.0f;
    [Tooltip("For every MismatchPenaltyRatio amount the desired price doesn't match shop's price.")]
    public float MismatchPenaltyRatio = 0.15f;
    [Tooltip("Reduce attraction chance by PenaltyPercent when desiered price doesn't match shop's price.")]
    [Range(0.0f, 1.0f)]public float PenaltyPercent = 0.1f;
}//class PricePreference


[System.Serializable]
public class WaitTimePreference {
    public Vector2 WaitTime = new Vector2(2f, 3f);
}//class PricePreference