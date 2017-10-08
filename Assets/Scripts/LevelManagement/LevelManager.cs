using UnityEngine;

public class LevelManager : MonoBehaviour {

    public static LevelManager Instance;

    protected Wave _wave;
    protected float waitBeforeLevelComplete = 3f;

    private float theEndCountdown;


    public void Start() {
        Instance = this;
        _wave = GetComponentInChildren<Wave>();
    }//Start


    public void Update() {
        this.checkLevelComplete();
    }//Update


    protected bool checkLevelComplete() {
        if (!_wave.IsNoMoreSpawns)
            return false;

        if (Shop.Instance.WaitingQ.Count > 0 ||
            Shop.Instance.OrderedQ.Count > 0)
            return false;

        theEndCountdown += Time.deltaTime;
        if (theEndCountdown < waitBeforeLevelComplete)
            return false;

        Debug.Log("Level Completed!");
        return true;
    }//checkLevelComplete

}//class
