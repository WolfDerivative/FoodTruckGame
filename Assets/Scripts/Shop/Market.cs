using UnityEngine;
using UnityEngine.UI;

public class Market: ResourceModifier {

    [Tooltip("Price for One unit of the merchandise.")]
    public float  PricePerUnit      = 1.0f;
    public string GONamePrice       = "Price";
    public string GONameCheckoutBtn = "CheckoutBtn";

    protected MarketCheckout _checkout;
    protected Text tPrice;


    public override void Start() {
        base.Start();
        foreach (Transform go in this.transform) {
            if (go.name == GONamePrice)
                SetTxt(go.gameObject, ref tPrice);
        }//foreach

        tPrice.text = "$" + PricePerUnit;

        //Checkout button is on the same hierarchy level as This game object.
        //Therefore, step up to parent and search from there.
        _checkout = this.transform.parent.GetComponentInChildren<MarketCheckout>();
    }//Start


    public override float Add(float amount = float.NegativeInfinity) {
        float toBuy = base.Add(amount);

        float availableCash = GameManager.Instance.GlobalStorage.Cash.Count - _checkout.TotalPrice;
        UnitsToCashAdjustment(ref toBuy, PricePerUnit, availableCash);
        float price = toBuy * PricePerUnit;

        _checkout.Add(price);            //update total checkout price
        tValue.text = (this.CurrentValue + toBuy).ToString(); //update merchandise units
        return this.CurrentValue + toBuy;
    }//Add


    public override float Substruct(float amount = float.NegativeInfinity) {
        float toSubstruct = base.Substruct(amount);

        _checkout.Substruct(toSubstruct * PricePerUnit);      //update total checkout price
        tValue.text = (this.CurrentValue - toSubstruct).ToString(); //update merchandise units
        return this.CurrentValue - toSubstruct;
    }//substruct


    /// <summary>
    ///  Adjust number of units that can be bought with respect to available cash.
    /// </summary>
    /// <param name="units">Units wanted to be bought.</param>
    /// <param name="price">Price per one unit.</param>
    /// <param name="availableCash">Cash in the bank available to spend.</param>
    public void UnitsToCashAdjustment(ref float units, float price, float availableCash) {
        float totalPrice = units * price;
        if (totalPrice < availableCash) //have enough cash to buy given units amount
            return;

        float overflow = Mathf.Abs(totalPrice - availableCash);
        int unitsReduction = Mathf.CeilToInt(overflow / price);
        units -= unitsReduction;
        if (units < 0) //PARANOIA
            units = 0;
    }//UnitsToCashAdjustment

}//class