using UnityEngine;

/// <summary>
///  This class acts as a middle man or a manager for Thoughts rendering
///  of the pedestrians. It uses it children object to get the Thoughts
///  component to then set its sprite based on requested thought from the
///  client\pedestrian\Ai
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class Thoughts: MonoBehaviour {

    public bool IsActive            { get { return _spriteRenderer.sprite == null; } }
    public Vector3 OriginPosition   { get { return vOriginPosition; } }

    protected SpriteRenderer    _spriteRenderer;
    protected Vector3           vOriginPosition; //original position of  the icon


    public virtual void Start() {
        if (_spriteRenderer == null)
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        vOriginPosition = this.transform.localPosition;
    }//Start


    public virtual void Hide() {
        _spriteRenderer.sprite = null;
    }//Hide


    public virtual void SetIcon(Sprite icon, bool state) {
        _spriteRenderer.enabled = state;
        _spriteRenderer.sprite = icon;
        #if UNITY_EDITOR
            if (_spriteRenderer.enabled && _spriteRenderer.sprite == null)
                Debug.LogWarning("FoodThought set to True, but Sprite passed is null!");
        #endif
    }//SetIcon


    public void OnEnable() {
        Start();
    }

}//class
