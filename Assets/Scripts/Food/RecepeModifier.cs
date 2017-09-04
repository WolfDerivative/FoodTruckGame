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
    }//Update


    public void Add(string AxisName) {
        bool isKeyDown = Input.GetButtonDown(AxisName);
        int sign = Mathf.FloorToInt(Mathf.Sign(Input.GetAxis(AxisName)));

        if (!isKeyDown)
            return;

        if (AxisName.ToLower().Contains("brain")) Shop.Instance.Recepe.AddBrain(sign * 1);
        if (AxisName.ToLower().Contains("bread")) Shop.Instance.Recepe.AddSeasoning(sign * 1);
        if (AxisName.ToLower().Contains("drink")) Shop.Instance.Recepe.AddDrink(sign * 1);

    }//Add

}//class
