using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Image))]
public class UINavbarButton : HoldableButton {

    public GameObject Content;

    private bool bIsSelected;
    [Tooltip("Save initial Image sprite to indicated btn's inactive state.")]
    private Sprite defaultState;


    public override void Start() {
        base.Start();
        this.bIsSelected = false;
        if(ImageCmp.sprite != null)
            this.defaultState = ImageCmp.sprite;
    }//Start


    public override void LateUpdate() {
        base.LateUpdate();

        if(!this.bIsSelected)
            return;
        Select();
    }//LateUpdate


    public void Register(UINavBar navBar) {
        ButtonCmp.onClick.AddListener(delegate { navBar.Select(this); });
        if (this.defaultState == null) {
            this.defaultState = ImageCmp.sprite;
        }
    }//Register


    public void Select(bool state=true) {
        Content.SetActive(state);
         this.bIsSelected = state;

        if (ButtonCmp.spriteState.pressedSprite == null) {
            Debug.LogWarning("Navbar's button '" + this.name + "' pressedSprite is not set!");
            return;
        }

        if (state) {
            if (ImageCmp.sprite != ButtonCmp.spriteState.pressedSprite)
                ImageCmp.sprite = ButtonCmp.spriteState.pressedSprite;
        } else {
            if (ImageCmp.sprite != this.defaultState)
                ImageCmp.sprite = this.defaultState;
        }//if/else state
    }//Select


    public void Deselect() {
        Select(false);
    }//Deselect

}//class UINavbarButton
