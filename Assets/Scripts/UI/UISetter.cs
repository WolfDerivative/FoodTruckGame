using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISetter : MonoBehaviour {

    public virtual Text GetTextCmp(string goName) {
        foreach (Transform transform in this.transform) {
            if(!goName.ToLower().Equals(transform.name.ToLower()))
                continue;
            Text txt = transform.gameObject.GetComponent<Text>();
            if(txt == null)
                GameUtils.Utils.WarningMessage("No Text cmp in '" + goName + "'!");
            return txt;
        }
        GameUtils.Utils.WarningMessage("No such game object '" + goName + "' found in '" + this.name + "'!");
        return null;
    }//GetTextCmp

}//class UISetter
