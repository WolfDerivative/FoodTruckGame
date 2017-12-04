using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StoveStore : UpgradeStore {

    private List<StoveMod> mods;

    public override void Start() {
        base.Start();
    }//Start


    protected override void readJsonData() {
        var streamingAssetsPath = Application.streamingAssetsPath + "/localization/eng/stove_mod";
        string fileContent = File.ReadAllText(streamingAssetsPath);
        CookingMod localiData = JsonUtility.FromJson<StoveMod>(fileContent);
    }//readJsonData


    protected override void setUpgradeNames() {

    }//SetUpgradeNames

}//class StoveStore
