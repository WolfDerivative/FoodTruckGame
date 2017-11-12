using System.Collections.Generic;
using UnityEngine;


/// <summary>
///  Will be instantiated by WeekdayBehaviour script.
/// </summary>
public class PoolManager: MonoBehaviour {

    public static PoolManager Instance;
    public bool IsAtLeastOneActive { get { return this.checkAtLeastOneActive(); } }
    public bool IsEmpty { get { return _pools == null || _pools.Count == 0; } }

    private List<ObjectPool> _pools;
    private Dictionary<string, int> spawnsTracker;


    public void Start() {
        Instance = this;
        this.spawnsTracker = new Dictionary<string, int>();
        //_pools = GetComponentsInChildren<ObjectPool>();
        _pools = new List<ObjectPool>();
    }//Start


    public GameObject PickFromPool() {
        if (IsEmpty)
            _pools = new List<ObjectPool>(GetComponentsInChildren<ObjectPool>());

        if (IsEmpty) {
            GameUtils.Utils.WarningMessage("Empty pool in " + this.name + ". Forgot to add Population prefabs?");
            return null;
        }//if empty

        int randomPool = this.poolGamble();
        ObjectPool pool = _pools[randomPool];
        GameObject go = pool.GetInactive();
        if (go == null)
            return null;
        go.SetActive(true);

        //A little Y margin so that spawned objects doesn't always
        //overlap with each outher.
        int yMargin = Random.Range(5, 25);
        Vector3 goPosition = go.transform.position;
        goPosition.y += (float)yMargin / 100;
        go.transform.position = goPosition;

        return go;
    }//PickFromPool


    /// <summary>
    ///  Pick a pool index based of its spawn chance. When no
    /// index is picked, it will use the one with the highest random
    /// value during the gamble loop.
    /// </summary>
    private int poolGamble() {
        float highestChance = 0f;
        int highestPoolIndexByChance = -1;
        int poolIndex = -1;
        for (int i = 0; i < _pools.Count; i++) {
            float randValue = Random.value;
            if(randValue <= _pools[i].SpawnChance)
                poolIndex = i;
            if (randValue > highestChance) {
                highestChance = randValue;
                highestPoolIndexByChance = i; //safety net in case no pool is selected
            }
        }//for

        return (poolIndex != -1) ? poolIndex : highestPoolIndexByChance;
    }//poolGamble


    private void trackSpawn(string poolName) {
        if(this.spawnsTracker == null)
            this.spawnsTracker = new Dictionary<string, int>();
        if(!this.spawnsTracker.ContainsKey(poolName))
            this.spawnsTracker.Add(poolName, 1);
        else
            this.spawnsTracker[poolName] += 1;
    }//trackSpawn


    private bool checkAtLeastOneActive() {
        for(int i=0; i < _pools.Count; i++) {
            if (_pools[i].ActiveCount > 0)
                return false;
        }//for
        return true;
    }//checkAtLeastOneActive


    public string SpawnTrackerToString() {
        string result = "";
        foreach (string poolName in this.spawnsTracker.Keys) {
            result += string.Format("{0} : {1};\n", poolName, this.spawnsTracker[poolName]);
        }//foreach
        return result;
    }//SpawnTrackerToString


    public ObjectPool CreatePool(GameObject prefabForThePool) {
        GameObject poolGO = new GameObject();
        ObjectPool objectPool = poolGO.AddComponent<ObjectPool>();
        objectPool.SetObjectToPool(prefabForThePool);

        poolGO.transform.SetParent(this.transform);
        poolGO.name = prefabForThePool.name + "_Pool";

        AddPool(objectPool);
        return objectPool;
    }//CreatePool


    public void AddPool(ObjectPool pool) {
        if(_pools == null)
            _pools = new List<ObjectPool>();
        _pools.Add(pool);
    }//AddPool

}//class
