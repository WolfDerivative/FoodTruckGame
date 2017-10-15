using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MarketCheckout : MonoBehaviour {

    public float TotalPrice {
        get {
            //checkout value will be in format of "$1.0". So, split out '$'
            //sign and parse the rest to float.
            return float.Parse(tCheckout.text.Split('$')[1]);
        }//get
    }//TotalCheckoutPrice

    protected Button _button;
    public Market[] _allMarkets;

    private Text tCheckout;


    public void Start() {
        tCheckout = GetComponentInChildren<Text>();
        _allMarkets = this.transform.parent.GetComponentsInChildren<Market>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(delegate { Checkout(); });
        tCheckout.text = "$0.00";
    }//Start

    
    public void Add(float amount) {
        float newCheckoutTotal = TotalPrice + amount;
        tCheckout.text = System.String.Format("${0:0.00}", newCheckoutTotal);  //update total checkout price
    }//Add


    public void Substruct(float amount) {
        float newCheckoutTotal = TotalPrice - amount;

        if (newCheckoutTotal < 0) { // this should never happened!
            newCheckoutTotal = 0;
            GameUtils.Utils.WarningMessage("No Soup For you! Checkout price is negative...");
        }//if

        tCheckout.text = System.String.Format("${0:0.00}", newCheckoutTotal);  //update total checkout price
    }//Substruct


    public void Checkout() {
        GameManager.Instance.GlobalStorage.Cash.Subtract(TotalPrice);
        for (int i = 0; i < _allMarkets.Length; i++) {
            Market marketObj = _allMarkets[i];
            switch (marketObj.ModType) {
                case (Storage.ResourceType.Brains):
                    GameManager.Instance.GlobalStorage.Brains.Add(marketObj.CurrentValue);
                    marketObj.Reset();
                    continue;
                case (Storage.ResourceType.Seasonings):
                    GameManager.Instance.GlobalStorage.Seasonings.Add(marketObj.CurrentValue);
                    marketObj.Reset();
                    continue;
                case (Storage.ResourceType.Drinks):
                    GameManager.Instance.GlobalStorage.Drinks.Add(marketObj.CurrentValue);
                    marketObj.Reset();
                    continue;
            }//switch
        }//for
    }//Checkout

}//class
