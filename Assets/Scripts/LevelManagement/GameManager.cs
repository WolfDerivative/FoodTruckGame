using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    [Tooltip("Do not save the game in the end of the level. For testing purposes")]
    public bool DontSave = false;
    public Storage GlobalStorage;
    public GameObject FloatingTextPrefab;
    public Savable SavabledData {
        get {
            this.saveLoadNotNull();
            return this.saveLoad.DataBuffer;
        }
    }//SavableData

    private SaveLoad saveLoad;


    public void Start() {
        if (Instance == null)
            Instance = this;
        else {
            DestroyImmediate(this.transform.parent.gameObject); //To ensure only one copy exists.
        }

        LoadGame();
    }//Start


    public void SaveGame() {
        this.saveLoadNotNull();
        GameData gd = SavabledData.GameStateData;
        gd.SetCurrentDay((int)Calendar.Instance.Today);
        gd.SetStorage(Shop.Instance.StorageState);
        SavabledData.SetGameStateData(gd);

        this.saveLoad.Save(SavabledData, !DontSave);
    }//SaveGame


    public void SaveGameDistrict(DistrictData districtData) {
        if (districtData == null) {
            GameUtils.Utils.WarningMessage("Passing null to SaveGameDistrict!");
            return;
        }//if

        SavabledData.SaveDistrict(districtData);
    }//SaveGameDistrict


    public Savable LoadGame() {
        GameData gd = SavabledData.GameStateData;

        //For testing purposes, allow Calendar override flag to set Today, instead
        //of loading from the file.
        if(!Calendar.Instance.IsOverrideToday)
            Calendar.Instance.SetDay((Calendar.Day)gd.CurrentDay);
        if(gd.SavedStorage != null)
             GlobalStorage = gd.SavedStorage;

        return SavabledData;
    }//LoadGame


    /// <summary>
    ///  Make sure saveLoad object is not null.
    /// Create an empty one, if so..
    /// </summary>
    private void saveLoadNotNull() {
        if (this.saveLoad == null)
            this.saveLoad = new SaveLoad();
    }//saveLoadNotNull
}//class
