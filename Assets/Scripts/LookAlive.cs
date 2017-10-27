using UnityEngine;


public class LookAlive : MonoBehaviour {
    

    public void Start() {
        DontDestroyOnLoad(this);
    }//Awake

}//class
