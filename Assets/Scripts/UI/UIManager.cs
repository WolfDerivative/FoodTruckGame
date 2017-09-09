using UnityEngine;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour {


    public UIGroup[] UIGroups = new UIGroup[] {
        new UIGroup("MainMenu"),
        new UIGroup("lvl_")
    };


    public void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }//OnEnable


    void OnDisable() {
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