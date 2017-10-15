using UnityEngine;

[System.Serializable]
public class SpawnProgression {

    public AnimationCurve Rate;
    public float SpawnDelay = 0.5f;
    public int Length { get { return Rate.keys.Length; } }


    /// <summary>
    ///  Return Value for the given keyframe index.
    /// </summary>
    /// <param name="keyIndex">Index to get a value on the curve.</param>
    public float GetValue(int keyIndex) {
        Keyframe[] keys = Rate.keys;
        if (keyIndex > keys.Length - 1)
            return -1;
        return keys[keyIndex].value;
    }//GetValue


    public float GetSubwaveDelay(int currentIndex) {
        Keyframe[] keys = Rate.keys;
        if (currentIndex >= keys.Length - 1)
            return -1;
        return keys[currentIndex + 1].time - keys[currentIndex].time;
    }//GetSubwaveDelay


}//Property


public class Wave : MonoBehaviour {

    public SpawnProgression Progression;
    /// <summary>
    ///  Status of the wave spawn progress. True - last object in the last subwave has been spawned.
    ///  False - there is more to spawn.
    /// </summary>
    public bool IsNoMoreSpawns { get { return bIsCompleted; } }

    protected PoolManager _poolManager;

    private int currentIndex; //current index on the animation curve
    private int spawnedCounter; //number of spawns per currentIndex
    private float timeout;
    private bool bIsSubwaveDelay;
    private bool bIsCompleted; //last wave has been spawned.


    public void Start() {
        _poolManager = PoolManager.Instance;
    }//Start


    public void Update() {
        StartSpawning();
    }//Update


    public void StartSpawning() {
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
        if (timeout == 0) {
            Spawn();
            //increment spawn count
            spawnedCounter++;
        }

        timeout += Time.deltaTime;
        //calculate spawn delay
        if (timeout >= Progression.SpawnDelay)
            timeout = 0;
    }//StartSpawning


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


    public int TotalSpanws() {
        int total = 0;
        for(int i=0; i < Progression.Rate.keys.Length; i++) {
            Keyframe key = Progression.Rate.keys[i];
            if (key.value > 0)
                total += Mathf.FloorToInt(key.value);
        }//for
        return total;
    }//TotalSpawns
}//Wave