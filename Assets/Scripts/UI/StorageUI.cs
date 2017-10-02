using UnityEngine;
using UnityEngine.UI;

public class StorageUI : MonoBehaviour {
    
    protected Text tBrainsText, tBreadsText, tDrinksText, tPriceText, tCashText;
    protected Text[] _allTextInChild;


    public void Start() {
        _allTextInChild = GetComponentsInChildren<Text>();
    }//Start


    public virtual void LateUpdate() {
        if (GameManager.Instance == null)
            return;
        UpdateBrainsValue   (GameManager.Instance.GlobalStorage.Brains.Count);
        UpdateSeasoningValue(GameManager.Instance.GlobalStorage.Seasonings.Count);
        UpdateDrinksValue   (GameManager.Instance.GlobalStorage.Drinks.Count);
        UpdateCashValue     (GameManager.Instance.GlobalStorage.Cash);
    }//LateUpdate


    /// <summary>
    ///  Update value of the "Brains count" to be shown on the screen.
    /// </summary>
    /// <param name="amount">Value to be shown</param>
    public virtual void UpdateBrainsValue(int amount) {
        if (!tBrainsText && !findTextObj(ref tBrainsText, "BrainsValue"))
            return;
        tBrainsText.text = amount.ToString();
    }//ShowBrainsCount


    /// <summary>
    ///  Update value of the "Breads count" to be shown on the screen.
    /// </summary>
    /// <param name="amount">Value to be shown</param>
    public virtual void UpdateSeasoningValue(int amount) {
        if (!tBreadsText && !findTextObj(ref tBreadsText, "SeasoningValue"))
            return;
        tBreadsText.text = amount.ToString();
    }//ShowBreadsCountTxt


    /// <summary>
    ///  Update value of the "Drinks count" to be shown on the screen.
    /// </summary>
    /// <param name="amount">Value to be shown</param>
    public virtual void UpdateDrinksValue(int amount) {
        if (!tDrinksText && !findTextObj(ref tDrinksText, "DrinksValue"))
            return;
        tDrinksText.text = amount.ToString();
    }//ShowDrinksCountTxt


    /// <summary>
    ///  Update value of the "Cash count" to be shown on the screen.
    /// </summary>
    /// <param name="amount">Value to be shown</param>
    public virtual void UpdateCashValue(float amount) {
        if (!tCashText && !findTextObj(ref tCashText, "CashValue"))
            return;
        tCashText.text = System.String.Format("{0:0.00}", amount);;
    }//UpdateCashValue


    /// <summary>
    ///  Show price set for the recepe.
    /// </summary>
    /// <param name="amount">Value to be shown</param>
    public virtual void UpdateRecepePrice(float amount) {
        if (!tPriceText && !findTextObj(ref tPriceText, "RecepePrice"))
            return;
        tPriceText.text = amount.ToString();
    }//ShowDrinksCountTxt


    protected bool findTextObj(ref Text textOjb, string objName) {
        if (_allTextInChild == null)
            _allTextInChild = GetComponentsInChildren<Text>();
        foreach(Text child in _allTextInChild) {
            if (child.name.ToLower() != objName.ToLower())
                continue;
            textOjb = child.GetComponent<Text>();
            break;
        }//foreach
        if(textOjb == null) {
            GameUtils.Utils.WarningGONotFound(objName + " Was not found or doesnt have Text component attached!");
            return false;
        }
        return true;
    }//findBrainsTextObj

}//class
