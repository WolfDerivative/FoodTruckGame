using UnityEngine;

public class GameSpeed : MonoBehaviour {

    public void SetGameSpeedMultiplier(float speed) {
        if (LevelManager.Instance == null) {
            GameUtils.Utils.WarningMessage("LevelManager instance is not set to change game speed value!");
            return;
        }

        LevelManager.Instance.SetGameSpeed(speed);
    }//ChangeGameSpeed


}//class GameSpeed
