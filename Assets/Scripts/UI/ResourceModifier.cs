using UnityEngine;
using UnityEngine.UI;

public class ResourceModifier : MonoBehaviour {

    public GameObject UnitsMod;
    [Tooltip("Type of resource that is being modified by this object.")]
    public Storage.ResourceType ModType;
    [Tooltip("How much units to add per button click.")]
    public float  UnitsPerPurchase  = 3;
    public string GONameSubstruct   = "Substruct";
    [Tooltip("Object for the display value.")]
    public string GONameValue       = "Value";
    public string GONameAdd         = "Add";

    public float CurrentValue {
        get {
            var splited_text = tValue.text.Split('$');
            float val = 0;
            if (splited_text.Length > 1)
                float.TryParse(splited_text[1], out val);
            else
                float.TryParse(splited_text[0], out val);
            return val;
        }//get
    }//UnitsToPurchase

    public virtual Ingredient IngredientType {
        get {
            return GameManager.Instance.GlobalStorage.ObjectFromType(ModType);
        }//get
    }//IngredientType

    protected Button btnAdd, btnSubstruct;
    protected Text   tValue;
    protected HoldableButton _btnAddHoldable, _btnSubstructHoldable;
    protected UIEventManager _uiEventManager;

    private float modifierTimeout = 0.2f;

    /* *********************************************************************** */


    public virtual void Start() {
        foreach (Transform go in this.transform) {
            if (go.name == GONameAdd)
                SetBtn(go.gameObject, ref btnAdd);
            if (go.name == GONameSubstruct)
                SetBtn(go.gameObject, ref btnSubstruct);
            if (go.name == GONameValue)
                SetTxt(go.gameObject, ref tValue);
        }//foreach

        _btnAddHoldable         = this.btnAdd.gameObject.AddComponent<HoldableButton>();
        _btnSubstructHoldable   = this.btnSubstruct.gameObject.AddComponent<HoldableButton>();

        this.btnAdd.onClick.AddListener(delegate { Add(); });
        this.btnSubstruct.onClick.AddListener(delegate { Substruct(); });
        if (UnitsMod != null) {
            UnitsMod.SetActive(false);
            _uiEventManager = this.UnitsMod.GetComponent<UIEventManager>();
        }
    }//Start


    public void Update() {
        if(UnitsMod == null)
            return;
        this.addOrSubstructButton(ref _btnAddHoldable, false);
        this.addOrSubstructButton(ref _btnSubstructHoldable, true);
    }//Update


    private void addOrSubstructButton(ref HoldableButton holdable, bool isSubstruct) {
        if (holdable.IsButtonDown && !holdable.IsButtonReleased) {
            if (holdable.ButtonUsed == KeyCode.Mouse1) {
                UnitsMod.SetActive(true);
                UnitsMod.transform.SetParent(holdable.transform);
                _uiEventManager.RectTransformCmp.anchoredPosition = Vector3.zero;
                _uiEventManager.RectTransformCmp.localScale = Vector3.one;
                _uiEventManager.SetMarket(this);
            }//if holding mouse1
            if (holdable.ButtonUsed == KeyCode.Mouse0) {
                if (holdable.HoldTime >= this.modifierTimeout) {
                    if(isSubstruct)
                        Substruct();
                    else
                        Add();
                    holdable.ResetHoldTime();
                }
            }//if holding mouse0
        }//if holding button

        if (holdable.IsButtonReleased) { //FIXME: check of Mouse1 is still holding
            UnitsMod.SetActive(false);
            UnitsMod.transform.SetParent(null);
        }//if
    }//addOrSubstructButton


    /// <summary>
    ///  Add number of Units to the "Value" text GO.
    /// </summary>
    /// <returns>The amount of units that were added.</returns>
    public virtual float Add(float amount = float.NegativeInfinity) {
        if (amount == float.NegativeInfinity)
            amount = this.UnitsPerPurchase;

        //How many units can be added to reach allowed Max.
        float valueLimit = IngredientType.Max - IngredientType.Count;

        if(valueLimit == 0)
            return 0;

        //Number of units potentially extending the limit.
        float newCartValue = this.CurrentValue + amount;
        //when valueLimit = 7, current = 5 and newCartValue = 9, means new
        //cart value will overflow the limit. Therefore, just add as many units
        //needed to fill reach the limit: valueLimit -  current = 2 -> amount = 2
        float unitsToAdd = (newCartValue > valueLimit) ? valueLimit - this.CurrentValue : amount;

        return unitsToAdd;
    }//Add


    /// <summary>
    ///  Substruct amount from the "Value" text GO.
    /// </summary>
    /// <param name="amount">default = 1</param>
    /// <returns>The amount of units to be substructed.</returns>
    public virtual float Substruct(float amount = float.NegativeInfinity) {
        if (amount == float.NegativeInfinity) {
            amount = this.UnitsPerPurchase;
            //override amount regardless of the default units count
            if (this.CurrentValue % UnitsPerPurchase != 0)
                amount = this.CurrentValue % UnitsPerPurchase;
        }//if amount

        float toSubstruct = (this.CurrentValue - amount) < 0 ? this.CurrentValue : amount;

        return toSubstruct;
    }//Substruct


    /// <summary>
    ///  Reset changes to original value.
    /// </summary>
    public virtual void Reset() {
        Substruct(this.CurrentValue);
    }//Rest


    /// <summary>
    ///  Get Button component from gameobject and set it to the passed button element.
    /// This just saves some sanity setting tAdd, tSubstruct button objects.
    /// </summary>
    /// <param name="go">GameObject to get Button component from to set btnTarget.</param>
    /// <param name="btnTarget">Button object to be set</param>
    protected virtual void SetBtn(GameObject go, ref Button btnTarget) {
        Button btn = go.GetComponent<Button>();
        if (btn == null) {
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
