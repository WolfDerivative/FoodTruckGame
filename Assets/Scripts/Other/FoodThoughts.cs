using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FoodThoughts: Thoughts {

    /*
    public override void SetIcon(Sprite icon, bool state) {
        _spriteRenderer.enabled = state;
        _spriteRenderer.sprite = icon;
        #if UNITY_EDITOR
            if(_spriteRenderer.enabled && _spriteRenderer.sprite == null)
                Debug.LogWarning("FoodThought set to True, but Sprite passed is null!");
        #endif
    }//SetIcon
    */
}//class
