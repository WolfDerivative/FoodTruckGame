using UnityEngine;

[RequireComponent(typeof(Kitchen), typeof(FrontDesk), typeof(Exterior))]
public class ShopProperties : MonoBehaviour {

    protected Kitchen KitchenCompartment { get {
            if (_kitchen == null)
                _kitchen = GetComponent<Kitchen>();
            return _kitchen;
        }//get
    }//KitchenCompartment
    protected FrontDesk FrontdeskCompartment { get {
            if (_frontDesk == null) _frontDesk = GetComponent<FrontDesk>();
            return _frontDesk;
        }//get
    }//FrontdeskCompartment
    protected Exterior ExteriorCompartment { get {
            if(_exterior == null) _exterior = GetComponent<Exterior>();
            return _exterior;
        }//get
    }//ExteriorCompartment

    private Kitchen _kitchen;
    private FrontDesk _frontDesk;
    private Exterior _exterior;


    /// <summary>
    ///  Try the odds agains all of the ExteriorCompartment AttractionChance
    /// mods.
    /// </summary>
    public bool TryAttractByChance() {
        float chance = Random.value;
        return chance <= ExteriorCompartment.GetAttractionChance();
    }//AttractByChance


    /// <summary>
    ///  Using KitchenCompartment mods to get a sum of FastCook
    /// chances to try the ods.
    /// </summary>
    public bool TryFaskCook() {
        float chance = Random.value;
        return chance <= KitchenCompartment.GetFastCookChance();
    }//TryFaskCook


    /// <summary>
    ///  Sum of all MaxOrders mods from FrontdeskCompartment.
    /// </summary>
    public int GetMaxOrders() {
        return FrontdeskCompartment.GetMaxOrders();
    }//GetMaxOrders


    /// <summary>
    ///  Sum of all PriceSwing mods from ExteriorCompartment.
    /// </summary>
    public float GetPriceSwing() {
        return ExteriorCompartment.GetPriceSwing();
    }//GetPriceSwing

}//class ShopProperties
