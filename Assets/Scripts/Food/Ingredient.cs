

[System.Serializable]
public class Ingredient {


    /// <summary>
    /// Number of ingredients needed to cook, when it is used
    /// in the recepe.
    /// Or number of ingredients available in the storage.
    /// -1 means - no ingredients taken from the storage to the store.
    /// </summary>
    public int Count = 0;

    public void Add(int amount) {
        Count += amount;
        if (Count < 0)
            Count = 0;
    }//Add
}//class
