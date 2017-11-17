using UnityEngine;

[System.Serializable]
public class CashierMod {

    [Tooltip("How many orders can be taken at a time.")]
    [Range(1, 10)] public int MaxOrders = 1;
    [Tooltip("How many people can be standing in line.")]
    [Range(10, 50)] public int MaxLineSize = 5;


    public CashierMod() {
        this.MaxOrders = 0;
        this.MaxLineSize = 0;
    }//ctor


}//class CashierMod
