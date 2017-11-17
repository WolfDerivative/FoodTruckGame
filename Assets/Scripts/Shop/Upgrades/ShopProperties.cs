using UnityEngine;

[RequireComponent(typeof(Kitchen))]
public class ShopProperties : MonoBehaviour {

    [Tooltip("Default stove property of the truck without applying any mods.")]
    public StoveMod DefaultStove;
    [Tooltip("Default Exterior property of the truck without applying any mods.")]
    public ExteriorMod DefaultExterior;
    [Tooltip("Default cashier property of the truck without applying any mods.")]
    public CashierMod DefaultCashier;

    protected Kitchen KitchenCompartment { get {
            if (_kitchen == null)
                _kitchen = GetComponent<Kitchen>();
            return _kitchen;
        }//get
    }//KitchenCompartment

    [SerializeField] private StoveMod[] stoveMods = new StoveMod[0];
    [SerializeField] private ExteriorMod[] exteriorMods = new ExteriorMod[0];
    [SerializeField] private CashierMod[] cashierMods = new CashierMod[0];
    private Kitchen _kitchen;


    public bool TryAttractByChance() {
        float chance = Random.value;
        return chance <= GetAttractionChance();
    }//AttractByChance


    public bool TryFaskCook() {
        float chance = Random.value;
        return chance <= KitchenCompartment.GetFastCookChance();
    }//TryFaskCook


    /// <summary>
    ///  Return sum of default AttractionChance value with it
    /// Exterior mods. If no mods attached - return truck's default.
    /// </summary>
    public float GetAttractionChance() {
        if (this.exteriorMods.Length == 0) //no mods attached
            return this.DefaultExterior.AttractionChance;
        Debug.Log(this.DefaultExterior.AttractionChance + SumAllAttractionChance());
        return this.DefaultExterior.AttractionChance + SumAllAttractionChance();
    }//GetAttractionChance


    /// <summary>
    ///  Return sum of all AttractionMod of the attached exterior mods.
    /// If no mods attached - return 0.
    /// </summary>
    public float SumAllAttractionChance() {
        float total = 0;
        for (int i = 0; i < this.exteriorMods.Length; i++) {
            total += this.exteriorMods[i].AttractionChance;
        }//for
        return total;
    }//SumAllAttractionChance


    /// <summary>
    ///  Return max number of clients cashier can take order from at a time.
    /// </summary>
    public int GetMaxOrders() {
        if(this.cashierMods.Length == 0)
            return DefaultCashier.MaxOrders;
        return this.DefaultCashier.MaxOrders + SumAllMaxOrders();
    }//GetMaxOrders


    /// <summary>
    ///  Sum of all max order mods attached to the truck.
    /// </summary>
    public int SumAllMaxOrders() {
        int total = 0;
        for (int i = 0; i < this.cashierMods.Length; i++) {
            total += this.cashierMods[i].MaxOrders;
        }//for
        return total;
    }//SumAllMaxOrders


    public float GetPriceSwing() {
        int total = 0;
        for (int i = 0; i < this.cashierMods.Length; i++) {
            total += this.cashierMods[i].MaxOrders;
        }//for
        return total;
    }//GetPriceSwing


    /// <summary>
    ///  Sum of all max order mods attached to the truck.
    /// </summary>
    public float SumAllPriceSwings() {
        float total = 0;
        for (int i = 0; i < this.exteriorMods.Length; i++) {
            total += this.exteriorMods[i].PriceSwing;
        }//for
        return total;
    }//SumAllMaxOrders


}//class ShopProperties
