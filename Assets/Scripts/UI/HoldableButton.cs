using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoldableButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler {

    public bool IsButtonDown        { get { return this.bIsPressed; } }
    public bool IsButtonReleased    { get { return this.bIsReleased; } }
    public bool IsHoverOver         { get { return this.bIsHoveredOver; } }
    public bool IsHoverOut          { get { return this.bIsHoveredOut; } }
    public KeyCode ButtonUsed { get { return InputButtonToKeycode(this.mouseBtnUsed); } }

    public float HoldTime       { get { return this.holdTime; } }
    public Button ButtonCmp {
        get {
            if(this.btn == null)
                this.btn = GetComponent<Button>();
            return this.btn;
        }//get
    }//ButtonCmp

    private bool bIsPressed;
    private bool bIsReleased;
    private bool bIsHoveredOver;
    private bool bIsHoveredOut;
    private float holdTime;
    PointerEventData.InputButton mouseBtnUsed;
    private Button btn;


    public void Start() {
        this.bIsPressed = this.bIsReleased = false;
        this.bIsHoveredOver = this.bIsHoveredOut = false;
        holdTime = 0;
        btn = GetComponent<Button>();
    }//Start


    public void Update() {
        if (this.bIsPressed)
            this.holdTime += Time.deltaTime;
    }//Update


    /// <summary>
    ///  Release "out" flags (e.g. Released, HoveredOut etc) so that they stay
    /// True just for one frame. Helps to detect those "out" events during runtime.
    /// </summary>
    public void LateUpdate() {
        if(this.bIsReleased)
            this.bIsReleased = false;
        if(this.bIsHoveredOut)
            this.bIsHoveredOut = false;
    }//LateUpdate


    public void ResetHoldTime() { this.holdTime = 0; }


    public KeyCode InputButtonToKeycode(PointerEventData.InputButton inputBtn) {
        switch (inputBtn) {
            case (PointerEventData.InputButton.Left):
                return KeyCode.Mouse0;
            case (PointerEventData.InputButton.Right):
                return KeyCode.Mouse1;
            case (PointerEventData.InputButton.Middle):
                return KeyCode.Mouse2;
            default:
                return KeyCode.Mouse0;
        }//Switch
    }//InputButtonToKeycode


    public void OnPointerDown(PointerEventData eventData) {
        this.bIsPressed = true;
        this.holdTime = 0;
        this.mouseBtnUsed = eventData.button;
    }//OnPointerDown


    public void OnPointerUp(PointerEventData eventData) {
        this.bIsPressed = false;
        this.bIsReleased = true;
    }//OnPointerDown


    public void OnPointerEnter(PointerEventData eventData) {
        this.bIsHoveredOver = true;
    }//OnPointerEnter


    public void OnPointerExit(PointerEventData eventData) {
        this.bIsHoveredOver = true;
    }//OnPointerExit


}//class HoldableButton
