using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class FloatingText : MonoBehaviour {

    [Tooltip("How long text will live in the worldspace before auto destroyed.")]
    public float Lifespan = 1.0f;

    protected Text _textUI;


    public void OnEnable() {
        if(_textUI == null)
            _textUI = GetComponent<Text>();
    }//Start


    /// <summary>
    ///  Show text at given position and destroy itself after Lifespan seconds.
    /// </summary>
    /// <param name="txt">Text to be shown.</param>
    /// <param name="position">Position to be shown at.</param>
    public void ShowText(string txt, Vector2 position) {
        if(_textUI == null) {
            GameUtils.Utils.WarningMessage("FloatingText's 'Text' component in '" + this.name + "' is not set!");
            return;
        }//if
        this.transform.SetParent(UIManager.Instance.transform);

        _textUI.text = txt;
        _textUI.rectTransform.localScale = Vector3.one;
        this.transform.position = position;
        Destroy(this.gameObject, Lifespan);
    }//ShowText
 

}//class
