using UnityEngine;


public class ShopProperties : MonoBehaviour {

    [Tooltip("Chance to attract customer to buy food from this shop.")]
    [Range(0, 1)] public float ChanceOfAttraction = 0.5f;
    public int MaxOrders = 2;
    [Range(0, 1)] public float FastCookChance = 0.005f;


    public bool TryAttractByChance() {
        return Random.value <= ChanceOfAttraction;
    }//AttractByChance


    public bool TryFaskCook() {
        float chance = Random.value;
        return chance <= FastCookChance;
    }//TryFaskCook

}//class ShopProperties
