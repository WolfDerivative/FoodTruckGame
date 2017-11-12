[System.Serializable]
public class Ingredient {

    /// <summary>
    /// Number of ingredients needed to cook, when it is used
    /// in the recepe.
    /// Or number of ingredients available in the storage.
    /// </summary>
    public float Count = 0;
    public float Max = 50;


    public Ingredient() { }


    public Ingredient(float count, float max) {
        this.Count = count;
        this.Max = max;
    }//ctor


    public void Add(float amount) {
        Count += amount;
        if (Count < 0)
            Count = 0;
    }//Add


    public void Subtract(float amount) {
        Add(-amount);
    }//Subtract


    public void Reset() {
        Count = 0;
    }//Reset


    public override string ToString() {
        return string.Format("Count: {0}; Max: {1}", Count, Max);
    }//ToString

}//class
