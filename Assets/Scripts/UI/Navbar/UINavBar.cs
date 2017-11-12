using System.Collections.Generic;
using UnityEngine;

public class UINavBar : MonoBehaviour {

    private List<UINavbarButton> buttons;

    public void Start() {
        buttons = new List<UINavbarButton>();
        foreach (Transform child in this.transform) {
            UINavbarButton navbarBtn = child.GetComponent<UINavbarButton>();
            if(navbarBtn == null)
                continue;
            navbarBtn.Register(this);
            buttons.Add(navbarBtn);
        }//foreach

        if (buttons.Count == 0) {
            GameUtils.Utils.WarningMessage(this.name + " UINavBar didnt find any UINavbarButton in its children!");
            return;
        }//if no buttons found

        Select(this.buttons[0]);
    }//Start


    public void Select(UINavbarButton targetBtn) {
        for (int i = 0; i < this.buttons.Count; i++) {
            var btn = this.buttons[i];
            btn.Deselect();
        }//for
        targetBtn.Select();
    }//Select

}//class UINavBar
