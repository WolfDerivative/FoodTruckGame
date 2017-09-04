using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ActionThoughts : Thoughts {

    public enum Actions { StandingInLine, WaitingForOrder }

    public Sprite StandingInLine;
    public Sprite WaitingForOrder;


    public void SetIcon(Actions actionType, bool state) {
        if(actionType == Actions.StandingInLine)
            _spriteRenderer.sprite = StandingInLine;

        if(actionType == Actions.WaitingForOrder)
            _spriteRenderer.sprite = WaitingForOrder;

        #if UNITY_EDITOR
            if(_spriteRenderer.sprite == null)
                Debug.LogWarning("Sprite for " + EnumToString(actionType) + " is null!");
        #endif
    }//SetIcon


    public string EnumToString(Actions act) {
        if (act == Actions.StandingInLine)
            return "Ordering";
        if (act == Actions.WaitingForOrder)
            return "WaitingForOrder";

        return "UnparsedTOSTRINGAction"; //should never happened.
    }//EnumToString

}//class
