using System.Collections.Generic;

[System.Serializable]
public class LocalizationData {

    public LocalizationItem[] Entries { get { return this.entries; } }

    [UnityEngine.SerializeField]
    private LocalizationItem[] entries;

    public LocalizationItem GetEntry(int index) {
        if (index > Entries.Length - 1) {
            GameUtils.Utils.WarningMessage("Localization Item " + index + " is out of range!");
            return new LocalizationItem();
        }//if
        return Entries[index];
    }//GetEntry

}//class Localization

[System.Serializable]
public class LocalizationItem {
    public string Key { get { return this.key; } }
    public string Value { get { return this.value; } }

    [UnityEngine.SerializeField]
    private string key; //name of the UI Text object
    [UnityEngine.SerializeField]
    private string value; //value for that UI Text.
}//class LocalizationItem


[System.Serializable]
public class UpgradesLocaleData {
    public List<UpgradeLocaleItem> Entries;
}//class StoveLocaleData


[System.Serializable]
public class UpgradeLocaleItem {
    public string ID;
    public string Description;
    public string DisplayName;
}//class StoveDscLocalization