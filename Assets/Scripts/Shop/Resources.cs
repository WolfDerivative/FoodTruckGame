public class Resources {

    public enum ResourceType { Brains, Seasonings, Drinks, Money, Waittime }

    //public float Cash;
    public Money        Cash;
    public Brain        Brains;
    public Seasoning    Seasonings;
    public Drink        Drinks;


    public Ingredient ObjectFromType(ResourceType t) {
        switch (t) {
            case (ResourceType.Brains):
                return Brains;
            case (ResourceType.Seasonings):
                return Seasonings;
            case (ResourceType.Drinks):
                return Drinks;
            case (ResourceType.Money):
                return Cash;
            default:
                return null;
        }//switch
    }//ObjectType


    public virtual void Substruct(float amount, ResourceType rType) {
        ObjectFromType(rType).Subtract(amount);
    }//Substruct


    public virtual void Add(float amount, ResourceType rType) {
        ObjectFromType(rType).Add(amount);
    }//Add


}//class