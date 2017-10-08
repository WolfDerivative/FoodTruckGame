using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecepeModifier : MonoBehaviour {

    public void Update() {
        if (Shop.Instance == null)
            return;
        Add("AddBrain");
        Add("AddBread");
        Add("AddDrink");
        Add("AddPrice");
    }//Update


    public void Add(string AxisName) {
        bool isKeyDown = Input.GetButtonDown(AxisName);
        int sign = Mathf.FloorToInt(Mathf.Sign(Input.GetAxis(AxisName)));

        if (!isKeyDown)
            return;

        if (AxisName.ToLower().Contains("brain"))
            Shop.Instance.RecepeToServe.Add(sign * 1, Resources.ResourceType.Brains);
        if (AxisName.ToLower().Contains("bread"))
            Shop.Instance.RecepeToServe.Add(sign * 1, Resources.ResourceType.Seasonings);
        if (AxisName.ToLower().Contains("drink"))
            Shop.Instance.RecepeToServe.Add(sign * 1, Resources.ResourceType.Drinks);
        if (AxisName.ToLower().Contains("price"))
            Shop.Instance.RecepeToServe.Add(sign * 1, Resources.ResourceType.Money);

    }//Add

}//class
