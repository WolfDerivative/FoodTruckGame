[System.Serializable]
public class Storage : Resources{

    public void Substruct(Recepe r) {
        Brains.Subtract(r.Brains.Count);
        Seasonings.Subtract(r.Seasonings.Count);
        Drinks.Subtract(r.Drinks.Count);
    }//TakeIngredients

}//class
