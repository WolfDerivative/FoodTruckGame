using UnityEngine;

public class RecepeCraft : ResourceModifier {

    public override int Add(int amount = int.MaxValue) {
        int unitsToAdd = base.Add(amount);
        //update cuurent value units
        tValue.text = (unitsToAdd + this.CurrentValue).ToString();
        Shop.Instance.RecepeToServe.Add(unitsToAdd, ModType);
        return unitsToAdd;
    }//Add


    public override int Substruct(int amount = int.MinValue) {
        int toSubstruct = base.Substruct(amount);
        //update cuurent value units
        tValue.text = (this.CurrentValue - toSubstruct).ToString();
        Shop.Instance.RecepeToServe.Substruct(toSubstruct, ModType);
        return toSubstruct;
    }//Substruct


}//class