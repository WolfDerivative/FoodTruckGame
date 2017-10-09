using UnityEngine;


public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public Storage GlobalStorage;
    public GameObject FloatingTextPrefab;


    public void Start() {
        if (Instance == null)
            Instance = this;
        else
            DestroyImmediate(this.gameObject); //To ensure only one copy exists.
    }//Start

}//class
