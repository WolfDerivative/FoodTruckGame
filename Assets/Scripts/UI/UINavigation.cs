using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UINavigation : MonoBehaviour {

    public void LoadScene(int level) {
        SceneManager.LoadScene(level);
    }//LoadScene


    public void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }//RestartLevel


    public void ExitGame() {
        Application.Quit();
    }//ExitGame


    /// <summary>
    ///  Toggle Show\Hide menu on the screen. This function intended to be used
    /// by UI buttons and suck.
    /// </summary>
    public void ToggleMenu(GameObject toToggle) {
        toToggle.SetActive(!toToggle.activeSelf);
    }//ToggleMenu


}//class
