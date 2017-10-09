using UnityEngine;


public class UIMenuPopup : MonoBehaviour {

    protected RectTransform MenuUI;


    public virtual void Start() {
        //Since GetComponentInChildren starts search from parent(itself)...
        //need to manually loop through children and break on first found cmp.
        for (int i = 0; i < this.gameObject.transform.childCount; i++) {
            Transform childObj = this.gameObject.transform.GetChild(i);
            MenuUI = childObj.GetComponent<RectTransform>();
            if (MenuUI != null)
                break;
        }//for
    }//Start


    public  virtual void Show() {
        MenuUI.gameObject.SetActive(true);
    }//Show


    public virtual void Hide() {
        MenuUI.gameObject.SetActive(false);
    }//Hide

}//class
