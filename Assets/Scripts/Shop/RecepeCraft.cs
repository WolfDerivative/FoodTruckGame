using UnityEngine;

public class RecepeCraft : ResourceModifier {

    public override Ingredient IngredientType {
        get {
            return Shop.Instance.RecepeToServe.ObjectFromType(this.ModType);
        }//get
    }//IngredientType


    public override float Add(float amount = float.NegativeInfinity) {
        float unitsToAdd = base.Add(amount);
        //update cuurent value units
        tValue.text = (unitsToAdd + this.CurrentValue).ToString();

        Shop.Instance.RecepeToServe.Add(unitsToAdd, ModType);
        return unitsToAdd;
    }//Add


    public override float Substruct(float amount = float.NegativeInfinity) {
        float toSubstruct = base.Substruct(amount);
        //update cuurent value units
        tValue.text = (this.CurrentValue - toSubstruct).ToString();
        Shop.Instance.RecepeToServe.Substruct(toSubstruct, ModType);
        return toSubstruct;
    }//Substruct


}//class