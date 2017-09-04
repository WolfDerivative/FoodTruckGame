using UnityEngine;


public class RecepeUI : StorageUI {


    public override void LateUpdate() {
        if (Shop.Instance == null)
            return;
        UpdateBrainsValue(Shop.Instance.Recepe.Brains.Count);
        UpdateBreadsValue(Shop.Instance.Recepe.Seasoning.Count);
        UpdateDrinksValue(Shop.Instance.Recepe.Drinks.Count);
    }

}//class
