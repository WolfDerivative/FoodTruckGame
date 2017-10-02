using UnityEngine;
using UnityEngine.UI;

public class Market: MonoBehaviour {

    public Storage.StorageType MarketType;
    [Tooltip("Price for One unit of the merchandise.")]
    public float  PricePerUnit      = 1.0f;
    [Tooltip("How much units to add per button click.")]
    public int    UnitsPerPurchase  = 3;
    public string GONameSubstruct   = "Substruct";
    public string GONameValue       = "Value";
    public string GONameAdd         = "Add";
    public string GONamePrice       = "Price";
    public string GONameCheckoutBtn = "CheckoutBtn";

    /// <summary>
    ///  Current number of units in the "shoping cart" to be bought.
    /// </summary>
    public int ShopingCart {
        get {
            int val = int.Parse(tValue.text);
            return val;
        }//get
    }//UnitsToPurchase

    public Ingredient IngredientType{
        get {
            return GameManager.Instance.GlobalStorage.ObjectFromType(MarketType);
        }//get
    }//IngredientType
    
    protected MarketCheckout _checkout;
    protected Button bAdd, bSubstruct;
    protected Text   tValue, tPrice;


    /* *********************************************************************** */


    public virtual void Start() {
        foreach(Transform go in this.transform) {
            if (go.name == GONameAdd)       SetBtn(go.gameObject, ref bAdd);
            if (go.name == GONameSubstruct) SetBtn(go.gameObject, ref bSubstruct);
            if (go.name == GONameValue)     SetTxt(go.gameObject, ref tValue);
            if (go.name == GONamePrice)     SetTxt(go.gameObject, ref tPrice);
        }//foreach

        tPrice.text = "$" + PricePerUnit;

        //Checkout button is on the same hierarchy level as This game object.
        //Therefore, step up to parent and search from there.
        _checkout = this.transform.parent.GetComponentInChildren<MarketCheckout>();
        
        bAdd.onClick.AddListener(delegate { Add(); });
        bSubstruct.onClick.AddListener(delegate { Substruct(); });
    }//Start


    /// <summary>
    ///  Add number of Units to the Value text object.
    /// </summary>
    public virtual void Add(int amount=int.MaxValue) {
        if (amount == int.MaxValue)
            amount = this.UnitsPerPurchase;

        int buyLimit = IngredientType.Max - IngredientType.Count;
        //to check if will overflow the limit
        int limitOverflow = this.ShopingCart + amount;
        int toBuy = (limitOverflow > buyLimit) ? limitOverflow - buyLimit : amount;

        float availableCash = GameManager.Instance.GlobalStorage.Cash - _checkout.TotalPrice;
        UnitsToCashAdjustment(ref toBuy, PricePerUnit, availableCash);
        float price = toBuy * PricePerUnit;

        _checkout.Add(price);            //update total checkout price
        tValue.text = (ShopingCart + toBuy).ToString(); //update merchandise units
    }//Add


    public virtual void Substruct(int amount=int.MinValue) {
        if (amount == int.MinValue) {
            amount = this.UnitsPerPurchase;
            if (this.ShopingCart % UnitsPerPurchase != 0) //overriding amount doesn't care about standard units...
                amount = this.ShopingCart % UnitsPerPurchase;
        }//if

        int toSubstruct = (this.ShopingCart - amount) < 0 ? this.ShopingCart : amount;

        _checkout.Substruct(toSubstruct * PricePerUnit);      //update total checkout price
        tValue.text = (ShopingCart - toSubstruct).ToString(); //update merchandise units
    }//Substruct


    public void UnitsToCashAdjustment(ref int units, float price, float availableCash) {
        float totalPrice = units * price;
        if (totalPrice < availableCash) //have enough cash to buy given units amount
            return;

        float overflow = Mathf.Abs(totalPrice - availableCash);
        int unitsReduction = Mathf.CeilToInt(overflow / price);
        units -= unitsReduction;
        if (units < 0) //PARANOIA
            units = 0;
    }//UnitsToCashAdjustment


    /// <summary>
    ///  Set shopping cart value to 0.
    /// </summary>
    public void Reset() {
        Substruct(this.ShopingCart);
    }//Rest


    /// <summary>
    ///  Get Button component from gameobject and set it to the passed button element.
    /// This just saves some sanity setting tAdd, tSubstruct button objects.
    /// </summary>
    /// <param name="go">GameObject to get Button component from to set btnTarget.</param>
    /// <param name="btnTarget">Button object to be set</param>
    protected virtual void SetBtn(GameObject go, ref Button btnTarget) {
        Button btn = go.GetComponent<Button>();
        if(btn == null) {
            GameUtils.Utils.WarningMessage(go.name + " doesnt have Button component to set " + btnTarget.name + "!");
            return;
        }
        btnTarget = btn;
    }//SetBtn


    /// <summary>
    ///  Sets Text component from passed gameobject to text object.
    /// </summary>
    /// <param name="go">GameObject to get Text component from to set txtTarget.</param>
    /// <param name="txtTarget">Text object to be set</param>
    protected virtual void SetTxt(GameObject go, ref Text txtTarget) {
        Text txt = go.GetComponent<Text>();
        if (txt == null) {
            GameUtils.Utils.WarningMessage(go.name + " doesnt have Text component to set " + txtTarget.name + "!");
            return;
        }
        txtTarget = txt;
    }//SetTxt


}//class
