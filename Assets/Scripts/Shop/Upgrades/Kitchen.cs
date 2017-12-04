using System.Collections.Generic;
using UnityEngine;

public class Kitchen : MonoBehaviour{

    [Tooltip("How many Stove can be placed.")]
    [Range(1, 3)] public int StoveSlots = 1;

    public List<StoveMod> stoves;
    private List<Staff> assignedStaff;


    public Kitchen() {
        this.stoves = new List<StoveMod>();
        this.stoves.Add(new StoveMod(true));
    }//ctor


    public bool AddStove(StoveMod newStove) {
        if(stoves.Count >= StoveSlots)
            return false;
        this.stoves.Add(new StoveMod(newStove));
        return true;
    }//SetStove


    public bool AddStaff(Staff newStaff) {
        if(this.assignedStaff.Count >= StoveSlots)
            return false;
        this.assignedStaff.Add(newStaff);
        return true;
    }//AssignStaff


    /// <summary>
    ///  Return sum of all FastCookMod of the available/attached
    /// stoves in the Stove slots. Use only those that occupied by
    /// a staff member. If no staff member is present, but multiple
    /// stoves are available, use the one with the highest chance.
    /// </summary>
    public float GetFastCookChance() {
        float total = 0;
        float highestChance = 0;
        for (int i = 0; i < this.stoves.Count; i++) {
            if (!this.stoves[i].IsActive)
                continue;
            if (highestChance < this.stoves[i].FastCookChance)
                highestChance = this.stoves[i].FastCookChance;
            //Use only those that are occupied by a staff member.
            if (this.stoves[i].IsOccupied)
                total += this.stoves[i].FastCookChance;
        }//for
        if (total == 0)
            total = highestChance;
        return total;
    }//GetFastCookChance

}//class Kitchen
