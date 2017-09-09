using UnityEngine;


public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public Storage GlobalStorage;


    public void Start() {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }//Start

}//class
