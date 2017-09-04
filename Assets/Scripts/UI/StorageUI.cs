using UnityEngine;
using UnityEngine.UI;

public class StorageUI : MonoBehaviour {
    
    protected Text tBrainsText, tBreadsText, tDrinksText;


    public void Start() {

    }//Start


    public virtual void LateUpdate() {
        if (Shop.Instance == null)
            return;
        UpdateBrainsValue(Shop.Instance.shopStorage.Brains.Count);
        UpdateBreadsValue(Shop.Instance.shopStorage.Breads.Count);
        UpdateDrinksValue(Shop.Instance.shopStorage.Drinks.Count);
    }//LateUpdate


    /// <summary>
    ///  Update value of the "Brains count" to be shown on the screen.
    /// </summary>
    /// <param name="amount">Value to be shown</param>
    public virtual void UpdateBrainsValue(int amount) {
        if (!tBrainsText && !findTextObj(ref tBrainsText, "BrainsAmount"))
            return;
        tBrainsText.text = amount.ToString();
    }//ShowBrainsCount


    /// <summary>
    ///  Update value of the "Breads count" to be shown on the screen.
    /// </summary>
    /// <param name="amount">Value to be shown</param>
    public virtual void UpdateBreadsValue(int amount) {
        if (!tBreadsText && !findTextObj(ref tBreadsText, "BreadsAmount"))
            return;
        tBreadsText.text = amount.ToString();
    }//ShowBreadsCountTxt


    /// <summary>
    ///  Update value of the "Drinks count" to be shown on the screen.
    /// </summary>
    /// <param name="amount">Value to be shown</param>
    public virtual void UpdateDrinksValue(int amount) {
        if (!tDrinksText && !findTextObj(ref tDrinksText, "DrinksAmount"))
            return;
        tDrinksText.text = amount.ToString();
    }//ShowDrinksCountTxt


    protected bool findTextObj(ref Text textOjb, string objName) {
        foreach(Transform child in this.transform) {
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
