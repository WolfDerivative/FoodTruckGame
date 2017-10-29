using UnityEngine;

public class GameSpeed : MonoBehaviour {

    public void SetGameSpeedMultiplier(float speed) {
        if (LevelManager.Instance == null) {
            GameUtils.Utils.WarningMessage("LevelManager instance is not set to change game speed value!");
            return;
        }

        LevelManager.Instance.SetGameSpeed(speed);
    }//ChangeGameSpeed


    public void Update() {
        float gameSpeed = -1;
        if(Input.GetAxis("GameSpeed_x1") > 0)
            gameSpeed = 1;
        if (Input.GetAxis("GameSpeed_x2") > 0)
            gameSpeed = 2;
        if (Input.GetAxis("GameSpeed_x3") > 0)
            gameSpeed = 3;
        if (Input.GetAxis("GameSpeed_x5") > 0)
            gameSpeed = 9;

        if (gameSpeed != -1)
            LevelManager.Instance.SetGameSpeed(gameSpeed);
    }//Update

}//class GameSpeed
