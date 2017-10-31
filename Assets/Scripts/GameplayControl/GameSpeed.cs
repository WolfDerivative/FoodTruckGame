using UnityEngine;
using UnityEngine.UI;

public class GameSpeed : MonoBehaviour {

    public Color ActiveColor = new Color(255, 196, 255);
    public Color NoramlColor = new Color(255, 255, 255);

    private Image[] speedButtons;


    public void Start() {
        speedButtons = GetComponentsInChildren<Image>();
        SetActiveButton(Time.timeScale);
    }//Start


    public void SetGameSpeedMultiplier(float speed) {
        if (LevelManager.Instance == null) {
            GameUtils.Utils.WarningMessage("LevelManager instance is not set to change game speed value!");
            return;
        }//if levelmanager not set yet

        LevelManager.Instance.SetGameSpeed(speed);
        SetActiveButton(speed);
    }//ChangeGameSpeed


    /// <summary>
    ///  Set color of the game speed button based of its game object name.
    /// E.g. if speed=1, it will look for GO name "x1" to set its color.
    /// </summary>
    /// <param name="speed"></param>
    public void SetActiveButton(float speed) {
        string btnName = "x" + speed;
        for (int i = 0; i < speedButtons.Length; i++) {
            speedButtons[i].color = NoramlColor;
            if(!speedButtons[i].name.Equals(btnName))
                continue;
            speedButtons[i].color = ActiveColor;
        }//for
    }//SetActiveButton


    public void Update() {
        float gameSpeed = -1;
        if(Input.GetAxis("GameSpeed_x1") > 0)
            gameSpeed = 1;
        if (Input.GetAxis("GameSpeed_x2") > 0)
            gameSpeed = 2;
        if (Input.GetAxis("GameSpeed_x3") > 0)
            gameSpeed = 3;
        if (Input.GetAxis("GameSpeed_x5") > 0)
            gameSpeed = 10;

        if (gameSpeed != -1)
            SetGameSpeedMultiplier(gameSpeed);
    }//Update

}//class GameSpeed
