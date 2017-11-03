using UnityEngine;
using UnityEngine.UI;

public class UIEventManager : MonoBehaviour {

    public float UnitsSelected { get { return this.units; } }

    private ResourceModifier _resourceMod;
    private Button[] allButtons;
    private float units;


    public RectTransform RectTransformCmp {
        get {
            if(this._rectTransform == null)
                this._rectTransform = GetComponent<RectTransform>();
            return this._rectTransform;
        }//get
    }//RectTransformCmp

    protected RectTransform _rectTransform;


    public void Start() {
        _rectTransform = GetComponent<RectTransform>();
        this.allButtons = GetComponentsInChildren<Button>();
        units = 1;

        for (int i = 0; i < this.allButtons.Length; i++) {
            string buttonName = this.allButtons[i].name;
            this.allButtons[i].onClick.AddListener( delegate{ SetUnit(buttonName); });
        }//for
    }//Start


    public void SetUnit(string unit) {
        if (unit.ToLower().Equals("all")) {
            if (_resourceMod != null)
                _resourceMod.Add(float.MaxValue);
            return;
        }//if

        float parsed = float.MinValue;
        float.TryParse(unit, out parsed);
        if (parsed == int.MinValue) {
            GameUtils.Utils.WarningMessage(this.name + " failed to parse " + unit + " into int!");
            return;
        }
        this.units = parsed;
        if(_resourceMod != null)
            _resourceMod.UnitsPerPurchase = this.units;
    }//SetUnit


    public void OnDisable() {
        _resourceMod = null;
    }//OnDisable


    public void SetMarket(ResourceModifier mod) {
        _resourceMod = mod;
    }//SetMarket

}//class UIEventManager
