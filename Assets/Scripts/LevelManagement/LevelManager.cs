using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public static LevelManager Instance;
    public bool IsLevelComplete { get { return this.checkLevelComplete(); } }
    public int TotalPotentialCustommers { get { return _weekday.WaveCmp.TotalSpawns; } }

    protected float waitBeforeLevelComplete = 3f;

    private float theEndCountdown;
    private WeekdayBehaviour _weekday;
    private bool bHasSaved;
    private DistrictData districtSaveData;
    private float gameSpeedMultiplier = 1f;


    public void Start() {
        Instance = this;
        _weekday = GetComponentInChildren<WeekdayBehaviour>();
        this.bHasSaved = false;

        var savedGame = GameManager.Instance.LoadGame();
        this.districtSaveData = savedGame.GetDistrictDataByName(SceneManager.GetActiveScene().name);
        LoadDistrictProps(this.districtSaveData);

        SetGameSpeed(1);
    }//Start


    public void Update() {
        if (_weekday.IsReady) {
            if (!_weekday.WaveCmp.IsCanSpawn) {
                _weekday.WaveCmp.SetModifier(_weekday.GetModifier());
                _weekday.WaveCmp.SetCanSpawn(true);
            }//if cant spawn
        }//if weekday not loaded yet
    }//Update


    public void LateUpdate() {
        bool isCompleted = this.checkLevelComplete();
        if(!isCompleted)
            this.bHasSaved = false;

        if (isCompleted) {
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
        if (_weekday.WaveCmp == null) //_wave has not yet initiated
            return false;
        if(!_weekday.WaveCmp.IsWaveStarted) //wave has not started spawn routine yet.
            return false;

        if (!_weekday.WaveCmp.IsNoMoreSpawns)
            return false;

        if (Shop.Instance.WaitingQ.Count > 0 ||
            Shop.Instance.OrderedQ.Count > 0)
            return false;

        theEndCountdown += Time.deltaTime;
        if (theEndCountdown < waitBeforeLevelComplete)
            return false;

        return true;
    }//checkLevelComplete


    public void OnDestroy() {
        this.districtSaveData.SetReputation(District.Instance.ReputationStatus);
        GameManager.Instance.SaveGameDistrict(this.districtSaveData);
        Calendar.Instance.NextDay();
        GameManager.Instance.SaveGame();
    }//OnDestroy


    public void SetGameSpeed(float multiplier) {
        Time.timeScale = multiplier;
    }//SetGameSpeed
}//class