using UnityEngine;


public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public Storage GlobalStorage;
    public GameObject FloatingTextPrefab;


    public void Start() {
        Instance = this;
    }//Start

}//class
