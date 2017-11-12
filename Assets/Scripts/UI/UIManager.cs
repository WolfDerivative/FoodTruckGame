using UnityEngine;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour {

    public static UIManager Instance;
    public Canvas SceneCanvas {
        get {
            if(_canvas == null)
                _canvas = GetComponent<Canvas>();
            return _canvas;
        }//get
    }//SceneCanvas

    protected Canvas _canvas;


    public void Start() {
        if (Instance == null) {
            Instance = this;
        } else {
            DestroyImmediate(this.gameObject); //To ensure only one copy exists.
            return;
        }
        _canvas = GetComponent<Canvas>();
    }//Start


    public UIGroup[] UIGroups = new UIGroup[] {
        new UIGroup("MainMenu"),
        new UIGroup("lvl_")
    };


    public void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }//OnEnable


    public void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }//OnDisable


    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        for (int i = 0; i < UIGroups.Length; i++) {
            UIGroup group = UIGroups[i];

            if (scene.name.Contains(group.SceneNamePattern)) {
                group.SetActive(true);
                continue;
            }//if pattern found

            group.SetActive(false);
        }//for

        //Use loaded scene's camera on load
        if(SceneCanvas.worldCamera == null)
            SceneCanvas.worldCamera = Camera.main;
    }//OnSceneLoaded

}//class


[System.Serializable]
public class UIGroup{

    public GameObject UIObjectGroup;
    [Tooltip("Show this UI group when this string pattern is met in the loaded Scene name.")]
    public string SceneNamePattern = "lvl_";


    public UIGroup(string pattern) {
        SceneNamePattern = pattern;
    }//ctor


    public void SetActive(bool state) {
        if(UIObjectGroup == null) {
            GameUtils.Utils.WarningMessage("Forgot to set UIObjectGroup for [" + SceneNamePattern + "] pattern??");
            return;
        }
        UIObjectGroup.SetActive(state);
    }//SetActive

}//class