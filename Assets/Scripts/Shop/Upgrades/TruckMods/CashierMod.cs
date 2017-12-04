using UnityEngine;

[System.Serializable]
public class CashierMod {

    [Tooltip("How many orders can be taken at a time.")]
    [Range(1, 10)] public int MaxOrders = 1;
    [Tooltip("How many people can be standing in line.")]
    [Range(10, 50)] public int MaxLineSize = 5;

    /// <summary>
    ///  Default constructor to use min value for each parameter.
    /// </summary>
    public CashierMod() {
        this.MaxOrders = 1;
        this.MaxLineSize = 10;
    }//default ctor


}//class CashierMod
