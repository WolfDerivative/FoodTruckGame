using UnityEngine;

[System.Serializable]
public class Staff : CookingMod{

    [Tooltip("How many orders can this staff cook at a time.")]
    [Range(1, 3)]public int SimultaneousOrders;

}//class Staff
