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

    public bool IsReady { get {
            if(Calendar.Instance == null)
                GameUtils.Utils.WarningMessage("Callendar Instance not set! Referenced by " + this.name);
            return Calendar.Instance != null && WaveCmp != null;
        }//get
    }//IsReady

    public Wave WaveCmp {
        get {
            if(_wave == null)
                _wave = GetComponent<Wave>();
            return _wave;
        }//get
    }//WaveCmp

    protected Wave _wave;
    protected PoolManager _poolmanager;


    public void Start() {
        _wave = GetComponent<Wave>();
        Init();
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
        this.createPopulation();
    }//Init


    private void createPopulation() {
        //Creating PoolManager object
        GameObject poolMngGO = new GameObject();
        poolMngGO.name = "PoolManager";
        _poolmanager = poolMngGO.AddComponent<PoolManager>();

        //Population distribution based of the Today's modifier
        Population[] populationMod = GetModifier().DistrictPopulation;
        //Create as manu object pools as there are population diversity
        for (int i = 0; i < populationMod.Length; i++) {
            GameObject poolPrefab = populationMod[i].CustomerPrefab;
            ObjectPool objPool = _poolmanager.CreatePool(poolPrefab);
            objPool.SetSpawnChance(populationMod[i].SpawnChance);

            //Create some amount of customers GO based of its SpawnChance
            //and overall level's total spawn amount.
            float poolSizePercent = GetProgression().GetTotalValues() *
                                                populationMod[i].SpawnChance;
            //Half of the size percent should be enough. Pool is set to "Dynamic" by
            //default, so it will handle shortage of the Instancies on demand.
            int poolSize = Mathf.FloorToInt(poolSizePercent / 2);
            objPool.SetPoolSize(poolSize);
        }//for
        _wave.SetPoolManager(_poolmanager);
    }//createPopulation


    /// <summary>
    ///  Return SpawnProgression cmp to be used for current day of the week (Calendar.Today).
    /// </summary>
    public SpawnProgression GetProgression() {
        DayModifier modifier = GetModifier();
        if (modifier.Progression != null)
            return modifier.Progression;
        GameUtils.Utils.WarningMessage("Progression is not set for " + this.name);
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