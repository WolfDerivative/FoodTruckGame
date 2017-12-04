using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeStore : MonoBehaviour {

    [Tooltip("Where will description of the upgrade outputed.")]
    public GameObject DSCOutput;
    [Tooltip("Name of the file to be used for localization. Leave empty to use Game Object's name.")]
    public string LocaleFileName = "";

    private Text _dscOutput;
    private Dictionary<Button, Text> upgradeBtns;
    private Dictionary <string, string> locale;


    public virtual void Start() {
        _dscOutput = DSCOutput.GetComponent<Text>();
        if (_dscOutput == null) {
            GameUtils.Utils.WarningMessage(this.name +
                " cant find DSCOutput Text cmp of " +
                DSCOutput.name);
        }//if text is null
        if (LocaleFileName == null || LocaleFileName == "")
            LocaleFileName = this.name;

        this.locale = LocalizationManager.Instance.ReadLocalizationFile(LocaleFileName);
        this.upgradeBtns = this.initAllButtons();
        this.readJsonData();

        //this.SetUpgradeNames();
    }//Start


    protected virtual void readJsonData() {
        throw new System.NotImplementedException();
    }//readJsonData


    /// <summary>
    ///  Find all buttons in the children objects with its text cmp and
    /// store them in the upgradeBtns dictionary for further referencing.
    /// </summary>
    protected Dictionary<Button, Text> initAllButtons() {
        var allBtns = new Dictionary<Button, Text>();
        var buttons = GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++) {
            Text btnTxt = buttons[i].GetComponentInChildren<Text>();
            allBtns.Add(buttons[i], btnTxt);
        }//for
        return allBtns;
    }//initAllButtons


    protected virtual void setUpgradeNames() {
        throw new System.NotImplementedException();
        //for (int i = 0; i < this.upgradeBtns.Count; i++) {
        //    Button btn = this.upgradeBtns[i];
        //    string upgradeName = GetDisplayName(btn.name);
        //    Text upgradeTxt = btn.gameObject.GetComponentInChildren<Text>();
        //    upgradeTxt.text = upgradeName;

        //    this.upgradeTxts.Add(upgradeTxt);
        //}//for
    }//SetUpgradeNames


    public virtual string GetDisplayName(string upgradeName) {
        string localeField = upgradeName + "_displayName";
        string value = null;
        this.locale.TryGetValue(localeField, out value);
        return value;
    }//GetDisplayName

}//class UIUpgradeSelection
