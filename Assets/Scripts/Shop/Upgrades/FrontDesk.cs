using System.Collections.Generic;
using UnityEngine;

public class FrontDesk : MonoBehaviour {
    
    [Tooltip("How many mods can be applied at a time.")]
    [Range(1, 3)] public int ModSlots = 1;
    [Tooltip("Mods to attach to this compartment to receive additional bonuses.")]
    public List<CashierMod> mods;

    public FrontDesk() {
        this.mods = new List<CashierMod>();
        this.mods.Add(new CashierMod());
    }//default ctor


    /// <summary>
    ///  Add a new mod to the mod slots stack. Return
    /// True if new mod was added. False - no more slots
    /// available, so newMod was not added.
    /// </summary>
    /// <param name="newMod">Mod to add to the slot stack.</param>
    public bool AddMod(CashierMod newMod) {
        if (mods.Count >= ModSlots)
            return false;
        mods.Add(newMod);
        return true;
    }//SetMod


    /// <summary>
    ///  Return max number of clients cashier can take order from at a time.
    /// </summary>
    public int GetMaxOrders() {
        int total = 0;
        for (int i = 0; i < this.mods.Count; i++) {
            total += this.mods[i].MaxOrders;
        }//for
        return total;
    }//GetMaxOrders

}//class FrontDesk
