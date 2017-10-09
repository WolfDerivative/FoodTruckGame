using UnityEngine;

public class UILevelComplete : UIMenuPopup {

    public void Update() {
        if (LevelManager.Instance.IsLevelComplete)
            this.Show();
        else
            this.Hide();
    }//Update


}//class
