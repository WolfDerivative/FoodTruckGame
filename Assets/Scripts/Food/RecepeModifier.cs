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

        if (AxisName.ToLower().Contains("brain")) Shop.Instance.RecepeToServe.AddBrain(sign * 1);
        if (AxisName.ToLower().Contains("bread")) Shop.Instance.RecepeToServe.AddSeasoning(sign * 1);
        if (AxisName.ToLower().Contains("drink")) Shop.Instance.RecepeToServe.AddDrink(sign * 1);
        if (AxisName.ToLower().Contains("price")) Shop.Instance.RecepeToServe.AddPrice(sign * 1);

    }//Add

}//class
