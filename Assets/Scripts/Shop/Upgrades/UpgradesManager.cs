using System.IO;
using UnityEngine;

public class UpgradesManager : MonoBehaviour {

    public static UpgradesManager Instance;


    public void Start() {
        if (Instance == null) {
            Instance = this;
        } else {
            DestroyImmediate(this.gameObject); //To ensure only one copy exists.
            return;
        }
    }//Start


}//class UpgradesManager
