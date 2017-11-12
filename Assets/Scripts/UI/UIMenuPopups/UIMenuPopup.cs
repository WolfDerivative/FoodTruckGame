using UnityEngine;


public class UIMenuPopup : MonoBehaviour {

    /// <summary>
    ///  Returns True if Child's object with RectTransform is enabled.
    /// False - otherwise. When it is enabled, it means Popup menu is
    /// shown.
    /// </summary>
    public virtual bool IsStatsShown {
        get {
            return _menuUiTransform.gameObject.activeSelf;
        }//get
    }//IsStatsShown

    protected RectTransform _menuUiTransform;


    public virtual void Start() {
        //Since GetComponentInChildren starts search from parent(itself)...
        //need to manually loop through children and break on first found cmp.
        for (int i = 0; i < this.gameObject.transform.childCount; i++) {
            Transform childObj = this.gameObject.transform.GetChild(i);
            _menuUiTransform = childObj.GetComponent<RectTransform>();
            if (_menuUiTransform != null)
                break;
        }//for
    }//Start


    public  virtual void Show() {
        _menuUiTransform.gameObject.SetActive(true);
    }//Show


    public virtual void Hide() {
        _menuUiTransform.gameObject.SetActive(false);
    }//Hide

}//class
