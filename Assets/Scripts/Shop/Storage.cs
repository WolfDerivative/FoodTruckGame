using System.Collections.Generic;

[System.Serializable]
public class Storage {

    public enum StorageType { Brains, Seasonings, Drinks }

    public float Cash;
    public Brain Brains;
    public Seasoning Seasonings;
    public Drink Drinks;


    public Ingredient ObjectFromType(StorageType t){
        switch (t) {
            case (StorageType.Brains):
                return Brains;
            case (StorageType.Seasonings):
                return Seasonings;
            case (StorageType.Drinks):
                return Drinks;
            default:
                return null;
        }//switch
    }//ObjectType


    public void ReduceCash(float amount) {
        Cash -= amount;
    }//ReduceCash


    public void Substruct(Recepe r) {
        Brains.Subtract(r.Brains.Count);
        Seasonings.Subtract(r.Seasoning.Count);
        Drinks.Subtract(r.Drinks.Count);
    }//TakeIngredients

}//class
