using System.Collections.Generic;

[System.Serializable]
public class Storage {

    public enum Type { Brains, Seasonings, Drinks }

    public float Cash;
    public Brain Brains;
    public Seasoning Seasonings;
    public Drink Drinks;


    public Ingredient ObjectFromType(Type t){
        switch (t) {
            case (Type.Brains):
                return Brains;
            case (Type.Seasonings):
                return Seasonings;
            case (Type.Drinks):
                return Drinks;
            default:
                return null;
        }//switch
    }//ObjectType


    public void ReduceCash(float amount) {
        Cash -= amount;
    }//ReduceCash


}//class
