using UnityEngine;


public class RecepeUI : StorageUI {


    public override void LateUpdate() {
        if (Shop.Instance == null)
            return;
        UpdateBrainsValue(Shop.Instance.RecepeToServe.Brains.Count);
        UpdateSeasoningValue(Shop.Instance.RecepeToServe.Seasonings.Count);
        UpdateDrinksValue(Shop.Instance.RecepeToServe.Drinks.Count);
        UpdateRecepePrice(Shop.Instance.RecepeToServe.Cash.Count);
    }

}//class
