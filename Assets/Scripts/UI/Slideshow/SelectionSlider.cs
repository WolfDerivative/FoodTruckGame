using System.Collections.Generic;
using UnityEngine;

public class SelectionSlider : MonoBehaviour {

    [Tooltip("Options to be searched for to use in a list of selections.")]
    public string ChoicePrefix = "d_";
    public int CurrentSelection { get { return this.currentSelection; } }

    private List<GameObject> selections;
    private int currentSelection;


    public void Start() {
        this.selections = ExtractSelections(ChoicePrefix);
        this.currentSelection = 0;
        Show(this.currentSelection);
    }//Start


    /// <summary>
    ///  Find and return list of all the objects of this children that start
    /// with specific prefix.
    /// </summary>
    /// <param name="prefix">Prefix to search for to match start of objects' name.</param>
    public List<GameObject> ExtractSelections(string prefix) {
        List<GameObject> found = new List<GameObject>();
        foreach (Transform go in this.transform) {
            if(!go.name.StartsWith(prefix))
                continue;
            found.Add(go.gameObject);
        }//foreach
        return found;
    }//ExtractSelections


    public void Show(int index) {
        if (index > this.selections.Count - 1) {
            GameUtils.Utils.WarningMessage("" +
                "Cant Show index '" + index + 
                "' while selections count is '" + 
                this.selections.Count + "'");
            return;
        }//if out of range
        this.selections[index].SetActive(true);
        this.currentSelection = index;

        for (int i = 0; i < this.selections.Count; i++) {
            if(i != index) //dont hide what needs to be shown
                Hide(i);
        }//for
    }//Show


    public void Hide(int index) {
        if (index > this.selections.Count) {
            GameUtils.Utils.WarningMessage("" +
                "Cant Hide index '" + index +
                "' while selections count is '" +
                this.selections.Count + "'");
            return;
        }//if out of range
        this.selections[index].SetActive(false);
        if(this.currentSelection == index)
            this.currentSelection = -1; //nothing is selected.
    }//Hide


    public void NextSelection() {
        if(this.currentSelection >= this.selections.Count - 1)
            return;
        Show(this.currentSelection + 1);
    }//NextSelection


    public void PreviousSelection() {
        if(this.currentSelection <= 0)
            return;
        Show(this.currentSelection - 1);
    }//PreviousSelection


    public void LoadSelectedScene() {
        int sceneToLoad = CurrentSelection + 1;
        int maxScenes = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        if (sceneToLoad > maxScenes - 1) {
            GameUtils.Utils.WarningMessage("Cant load scene '" + sceneToLoad + "'! Max district scenes '" + (maxScenes - 1) + "'!");
            return;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }//LoadSelectedScene

}//class SceneSelection
