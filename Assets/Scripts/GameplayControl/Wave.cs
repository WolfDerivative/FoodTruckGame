using UnityEngine;

[RequireComponent(typeof(SpawnProgression))]
public class Wave : MonoBehaviour {

    /// <summary>
    ///  Status of the wave spawn progress. True - last object in the last subwave has been spawned.
    ///  False - there is more to spawn.
    /// </summary>
    public bool IsNoMoreSpawns  { get { return bIsCompleted; } }
    public bool IsCanSpawn      { get { return this.bIsCanSpawn; } }
    public bool IsWaveStarted   { get { return this.bWaveHasStarted; } }
    public SpawnProgression Progression { get { return _progression; } }
    public int TotalSpawns { get { return Mathf.FloorToInt(Progression.GetTotalValues()); } }

    protected PoolManager _poolManager;
    protected SpawnProgression _progression;

    private int     currentIndex; //current index on the animation curve
    private int     spawnedCounter; //number of spawns per currentIndex
    private float   timeout;
    private bool    bIsSubwaveDelay;
    private bool    bIsCompleted; //last wave has been spawned.
    private bool    bIsCanSpawn;
    private bool    bWaveHasStarted;
    private Modifier _modifier;


    public void Start() {
        _poolManager = PoolManager.Instance;
        _progression = GetComponent<SpawnProgression>();
        bIsCanSpawn = false;
        timeout = _progression.SpawnDelay;
    }//Start


    public void Update() {
        if(this.bIsCanSpawn)
            StartSpawning();
    }//Update


    public void StartSpawning() {
        bWaveHasStarted = true;
        //PARANOIA. In case PoolManager loaded after this class, even though 
        //"Script execution order" is set correctly.
        if (_poolManager == null)
            _poolManager = PoolManager.Instance;

        //Get current spawn amount from index
        int toSpawnAmount = Mathf.FloorToInt(Progression.GetValue(currentIndex));
        if (toSpawnAmount < 0) { //No more subwaves on the progression curve
            bIsCompleted = true;
            return;
        }

        if (spawnedCounter >= toSpawnAmount) {
            float subwaveDelay = Progression.GetSubwaveDelay(currentIndex);
            if (subwaveDelay < 0) {
                currentIndex++;
                return;
            }

            if (!bIsSubwaveDelay) {
                bIsSubwaveDelay = true;
                Invoke("NextSubwave", subwaveDelay);
            }
            return;
        }//if spawnedCounter

        //Spawn object every SpawnDelay second
        if (timeout >= Progression.SpawnDelay) {
            Spawn();
            //increment spawn count
            spawnedCounter++;
            timeout = 0;
        }

        timeout += Time.deltaTime;
    }//StartSpawning


    public void ModProgression(Modifier mod) {
        Keyframe[] spawnFrames = new Keyframe[mod.Progression.Length];
        for (int i = 0; i < mod.Progression.Length; i++) {
            int minSpawn = Mathf.FloorToInt(Progression.GetValue(i));
            if(minSpawn < 0) //no spawn value found in the original progression curve
                minSpawn = 0;
            int maxSpawn = Mathf.FloorToInt(minSpawn + mod.SpawnRange);
            float newSpawnAmount = Random.Range(minSpawn, maxSpawn);
            spawnFrames[i] = new Keyframe(i, newSpawnAmount);
        }//for
        Progression.SetSpawnCurve(spawnFrames);
        Progression.SetSpawnDelay(mod.SpawnRange);
    }//ModProgression


    public void NextSubwave() {
        timeout = 0;
        spawnedCounter = 0;
        currentIndex++;
        bIsSubwaveDelay = false;
    }//NextSubwave


    /// <summary>
    ///  Spawn object from pool.
    /// </summary>
    public void Spawn() {
        _poolManager.PickFromPool();
    }//Spawn


    public void SetCanSpawn(bool state) {
        this.bIsCanSpawn = state;

        if(_modifier == null)
            return;

        SetProgression(_modifier.Progression);
        ModProgression(_modifier);
    }//SetCanSpawn


    public void SetModifier(Modifier mod) {
        this._modifier = mod;
    }//SetModifier


    public void SetProgression(SpawnProgression prog) {
        if (prog == null) {
            GameUtils.Utils.WarningMessage("SetProggression getting null!");
            return;
        }//if passing null
        this._progression.SetSpawnCurve(prog.CurveFrame);
    }//SetProgression
}//Wave