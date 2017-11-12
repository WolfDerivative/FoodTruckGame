using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public static LevelManager Instance;

    public bool IsLevelComplete             { get { return this.checkLevelComplete(); } }
    public int  TotalPotentialCustommers    { get { return _weekday.WaveCmp.TotalSpawns; } }


    private float               waitBeforeLevelComplete = 3f;
    private float               theEndCountdown;
    private WeekdayBehaviour    _weekday;
    private DistrictData        districtSaveData;
    private bool                bHasSaved;
    private bool                bIsLevelCompleted;


    public void Start() {
        Instance = this;
        _weekday = GetComponentInChildren<WeekdayBehaviour>();
        this.bHasSaved = false;
        this.bIsLevelCompleted = false;
        this.theEndCountdown = 0f;

        var savedGame = GameManager.Instance.LoadGame();
        this.districtSaveData = savedGame.GetDistrictDataByName(SceneManager.GetActiveScene().name);
        LoadDistrictProps(this.districtSaveData);

        SetGameSpeed(1);
    }//Start


    public void Update() {
        if(this.bIsLevelCompleted)
            return;

        if (_weekday.IsReady) {
            if (!_weekday.WaveCmp.IsCanSpawn) {
                _weekday.WaveCmp.SetModifier(_weekday.GetModifier());
                _weekday.WaveCmp.SetCanSpawn(true);
            }//if cant spawn
        }//if weekday not loaded yet
    }//Update


    public void LateUpdate() {
        if(this.bIsLevelCompleted)
            return;

       this.bIsLevelCompleted = this.checkLevelComplete();
        if(!this.bIsLevelCompleted)
            this.bHasSaved = false;

        if (this.bIsLevelCompleted) {
            if(this.bHasSaved) //savegame onlt once.
                return;
            SetGameSpeed(1);    //reset gamespeed back to normal

            if (Calendar.Instance == null)
                GameUtils.Utils.WarningMessage("Calendar Instance is not set!");

            this.bHasSaved = true;
        }
    }//Update


    public void LoadDistrictProps(DistrictData dt) {
        District.Instance.SetReputation(dt.GetReputationStatus());
    }//LoadDistrictProps


    /// <summary>
    ///  Verify if all customers of the day were spawned and are
    /// not waiting in the line or walking towards the shop.
    /// </summary>
    protected bool checkLevelComplete() {
        //_wave has not yet initiated
        if (_weekday.WaveCmp == null) {
            GameUtils.Utils.WarningMessage(_weekday.name + " WaveCmp is null in LevelMangaer!");
            return false;
        }//if wave is null

        //wave has not started spawn routine yet.
        if (!_weekday.WaveCmp.IsWaveStarted) {
            GameUtils.Utils.WarningMessage(_weekday.WaveCmp.name + " has not yet started spawn routine.");
            return false;
        }//if not started

        if (!_weekday.WaveCmp.IsNoMoreSpawns)
            return false;

        if (Shop.Instance.WaitingQ.Count > 0 ||
            Shop.Instance.OrderedQ.Count > 0)
            return false;

        this.theEndCountdown += Time.deltaTime;
        if (this.theEndCountdown < this.waitBeforeLevelComplete)
            return false;

        return true;
    }//checkLevelComplete


    public void OnDestroy() {
        this.districtSaveData.SetReputation(District.Instance.ReputationStatus);
        Calendar.Instance.NextDay();
        GameManager.Instance.SaveGameDistrict(this.districtSaveData);
        GameManager.Instance.SaveGame();
    }//OnDestroy


    public void SetGameSpeed(float multiplier) {
        Time.timeScale = multiplier;
    }//SetGameSpeed
}//class