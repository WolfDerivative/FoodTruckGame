using UnityEngine;

[RequireComponent(typeof(Wave))]
public class WeekdayBehaviour : MonoBehaviour {

    [Tooltip("Spawn behaviour for the district for each day of the week.")]
    public DayModifier[] DistrictCalendar = new DayModifier[]{
        new DayModifier(Calendar.Day.Mon),
        new DayModifier(Calendar.Day.Tue),
        new DayModifier(Calendar.Day.Wed),
        new DayModifier(Calendar.Day.Thu),
        new DayModifier(Calendar.Day.Fri),
        new DayModifier(Calendar.Day.Sat),
        new DayModifier(Calendar.Day.Sun)
    };

    public bool IsReady { get { return Calendar.Instance != null && WaveCmp != null; } }
    public Wave WaveCmp {
        get {
            if(_wave == null)
                _wave = GetComponent<Wave>();
            return _wave;
        }//get
    }//WaveCmp

    protected Wave _wave;


    public void Start() {
        Init();
        _wave = GetComponent<Wave>();
    }//Start


    /// <summary>
    ///  Instantiate prefab object to get SpawnProgression component from it.
    /// Instantiated object will be destroyed in the end of the routine.
    /// </summary>
    public void Init() {
        DayModifier modifier = GetModifier();
        if (modifier.ProgressionPrefab == null) {
            GameUtils.Utils.WarningMessage("ProgressionPrefab is not set!");
            return;
        }//if Prefab not set

        GameObject progressionInstance = Instantiate(modifier.ProgressionPrefab);
        SpawnProgression progression = progressionInstance.GetComponent<SpawnProgression>();
        if (progression == null) {
            GameUtils.Utils.WarningMessage("Active Day SpawnProgression is not set!\n" + modifier);
        }//if wave cmp is null
        modifier.SetProgression(progression);
    }//Init


    /// <summary>
    ///  Return SpawnProgression cmp to be used for current day of the week (Calendar.Today).
    /// </summary>
    public SpawnProgression GetProgression() {
        DayModifier modifier = GetModifier();
        if (modifier.Progression != null)
            return modifier.Progression;
        Init();
        return modifier.Progression;
    }//GetTodaysWave


    /// <summary>
    ///  Return a property describing current's Calendar day customer behaviour.
    /// </summary>
    public DayModifier GetModifier() {
        Calendar.Day today = Calendar.Instance.Today;
        if ((int)today > DistrictCalendar.Length) { //PARANOIA, kind of... Just in case.
            GameUtils.Utils.WarningMessage("DayOfTheWeek's calendar is not of the same length as Calendar's week??");
            today = Calendar.Day.Mon;
        }//if out of range

        return DistrictCalendar[(int)today];
    }//GetTodaysProperty

}//class DayOfTheWeek