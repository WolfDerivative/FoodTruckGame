using UnityEngine;

[System.Serializable]
public class StoveMod : CookingMod {

    public bool IsActive = false;
    public bool IsOccupied = false;

    public StoveMod(bool isActive) {
        this.IsActive = isActive;
    }//default ctor

    public StoveMod(StoveMod newStove) {
        this.CookSpeed = newStove.CookSpeed;
        this.FastCookChance = newStove.FastCookChance;
        this.IsActive = true;
    }//ctor

}//class StoveMod
