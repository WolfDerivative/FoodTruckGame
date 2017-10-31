using System.Collections.Generic;
using UnityEngine;


/// <summary>
///  Handy in 99% cases (fake news data) in situations where you need to reuse the same
/// type of GameObject\Prefab many times per second(s). E.g. bullet hell game where
/// you can pool projectiles and save CPU cicles in creating new gameojbects many times
/// per second(s).
/// </summary>
public class ObjectPool: MonoBehaviour {

    [Tooltip("Game object to be created into the pool.")]
    public GameObject ObjectToPool;
    [Tooltip("Number of objects instanciated on start.")]
    public int PoolSize = 5;
    [Tooltip("Allow pool to grow if there is not enough objects.")]
    public bool IsDynamic = true;
    [Tooltip("How many objects of the pool can be active simultaneously. '0' means no limit.")]
    public int SpawnLimit = 0;
    [Tooltip("Chance for PoolManager to use this pool to spawn.")]
    [Range(0f, 1f)]public float SpawnChance = 0.5f;
    [Tooltip("Name of the GameObject in the scene to where to store pool instancies. If object not found - it will be created with that name.")]
    public string StorageBucketName = "Bucket";

    public virtual int InactiveCount { get {
            int count = 0;
            foreach (GameObject go in this.pool)
                count = (go.activeSelf) ? count : count + 1;
            return count;
        }//get
    }//InactiveCount

    public virtual int ActiveCount {
        get {
            int count = 0;
            foreach (GameObject go in this.pool)
                count = (go.activeSelf) ? count + 1: count;
            return count;
        }//get
    }//ActiveCount


    protected List<GameObject> pool;
    protected GameObject bucket; //parent for all the clones objects.

    /* ------------------------------------------------------------------------------- */

    public virtual void Start() {
        pool = new List<GameObject>();
        createBucket();
        for (int i = 0; i < PoolSize; i++) {
            AddToPool();
        }//for
    }//Start


    /// <summary>
    ///  Pick an object from the Inactive pool. This will just return an object and
    /// will not activate or do any other operations with it.
    /// When "IsDynamic" variable is set to True - this function will create new object 
    /// instance if there are no inactive available.
    /// </summary>
    /// <returns>GameObject instanc or null when pool has no inactive objects.</returns>
    public virtual GameObject GetInactive() {
        for (int i = 0; i < this.pool.Count; i++) { 
            GameObject go = this.pool[i];
            if (go == null)
                continue;
            if (!go.activeInHierarchy)
                return go;
        }//foreach

        //at this point, there is no avaailable objects to pool from.
        //This, create a new one to supply the demand.
        if (!IsDynamic)
            return null;
        return AddToPool();
    }//GetInactive

    
    /// <summary>
    ///  Instantiate an object from the ObjectToPool prefab and add it to inactive pool,
    /// which can be picked from during the runtime.
    /// </summary>
    /// <returns> Instantiated game object that has been added to the pool. </returns>
    public virtual GameObject AddToPool() {
        if (SpawnLimit > 0) {
            if (ActiveCount >= SpawnLimit)
                return null;
        }//if
        GameObject go = (GameObject)Instantiate(ObjectToPool);
        go.name = go.name + "_" + (this.pool.Count + 1);

        if(bucket != null)
            go.transform.SetParent(bucket.transform);

        go.transform.position = new Vector3(0f, 0f, 0f);
        go.transform.localScale = go.transform.localScale;
        go.SetActive(false);
        this.pool.Add(go);
        return go;
    }//AddToPool


    /// <summary>
    ///  Get an object from an inactive pool and spawn it at the
    /// desiered location.
    /// </summary>
    /// <param name="spawnAt">Coords at which object will be spawned.</param>
    /// <returns>Spawned GameObject instance or Null, when no inactive found.</returns>
    public virtual GameObject SpawnAt(Vector3 spawnAt) {
        GameObject go = GetInactive();
        if (go == null)
            return null;
        go.transform.position = spawnAt;
        go.SetActive(true);
        return go;
    }//SpawnAt


    public List<GameObject> GetPool() { return this.pool; }
    

    /// <summary>
    ///  Create a "dummy" empty game object in the scene to where
    /// all the Pool instancies will be placed.
    /// </summary>
    protected void createBucket() {
        bucket = GameObject.Find(StorageBucketName);
        if (bucket == null) {
            bucket = new GameObject();
            bucket.name = StorageBucketName;
        }//if null
    }//createBucket


    public void SetObjectToPool(GameObject prefabGo) {
        ObjectToPool = prefabGo;
    }//SetObjectToPool


    public void SetSpawnChance(float val) {
        SpawnChance = val;
    }//SetSpawnChance


    public void SetPoolSize(int size) {
        PoolSize = size;
    }//SetPoolSize

}//Class