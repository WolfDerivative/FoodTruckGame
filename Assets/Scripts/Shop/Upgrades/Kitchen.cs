using UnityEngine;

public class Kitchen : MonoBehaviour{

    [Tooltip("How many Stove can be placed.")]
    [Range(1, 3)] public int StoveSlots = 1;

    public StoveMod[] stoves;
    private Staff[] assignedStaff;


    public Kitchen() {
        this.stoves = new StoveMod[StoveSlots];
        this.stoves[0] = new StoveMod(true);
    }//ctor


    public void SetStove(StoveMod newStove, int slotIndex) {
        if(this.stoves == null)
            this.stoves = new StoveMod[StoveSlots];

        if (slotIndex > StoveSlots - 1) {
            GameUtils.Utils.WarningMessage("Slot index " + slotIndex + " out of range!");
            return;
        }

        this.stoves[slotIndex] = new StoveMod(newStove);
    }//SetStove


    public void AssignStaff(Staff newStaff, int staffIndex) {
        if(this.assignedStaff == null)
            this.assignedStaff = new Staff[StoveSlots];

        if (staffIndex> StoveSlots - 1) {
            GameUtils.Utils.WarningMessage("Staff index " + staffIndex + " out of range!");
            return;
        }

        this.assignedStaff[staffIndex] = newStaff;
    }//AssignStaff


    /// <summary>
    ///  Return sum of default FastCookChance value with it
    /// Stoves' mods. If no mods attached - return truck's default.
    /// </summary>
    public float GetFastCookChance() {
        if (this.stoves.Length == 1) //no mods attached
            return this.stoves[0].FastCookChance;
        return SumAllFastCookMods();
    }//GetFastCookChance


    /// <summary>
    ///  Return sum of all FastCookMod of the available/attached
    /// stoves in the Stove slots. Use only those that occupied by
    /// a staff member. If no staff member is present, but multiple
    /// stoves are available, use the one with the highest chance.
    /// </summary>
    public float SumAllFastCookMods() {
        float total = 0;
        float highestChance = 0;
        for (int i = 0; i < this.stoves.Length; i++) {
            if(!this.stoves[i].IsActive)
                continue;
            if(highestChance < this.stoves[i].FastCookChance)
                highestChance = this.stoves[i].FastCookChance;
            //Use only those that are occupied by a staff member.
            if(this.stoves[i].IsOccupied)
                total += this.stoves[i].FastCookChance;
        }//for
        if(total == 0)
            total = highestChance;
        return total;
    }//SumAllFastCookMods

}//class Kitchen
