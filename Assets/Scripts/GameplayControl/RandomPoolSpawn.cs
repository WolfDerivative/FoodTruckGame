using UnityEngine;


public class RandomPoolSpawn : MonoBehaviour {

    protected ObjectPool[] _pools;


    public void Start() {
        _pools = GetComponentsInChildren<ObjectPool>();
        InvokeRepeating("PickFromPool", 0, 1.0f);
    }//Start


    public GameObject PickFromPool() {
        if (_pools == null || _pools.Length == 0)
            return null;
        int randomPool = Random.Range(0, _pools.Length);
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

}//class
