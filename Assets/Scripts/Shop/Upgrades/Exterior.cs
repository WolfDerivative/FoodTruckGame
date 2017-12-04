using System.Collections.Generic;
using UnityEngine;

public class Exterior : MonoBehaviour {

    [Tooltip("How many mods can be applied at a time.")]
    [Range(1, 5)] public int ModSlots = 1;
    [Tooltip("Mods to attach to this compartment to receive additional bonuses.")]
    public List<ExteriorMod> mods;


    public Exterior() {
        this.mods = new List<ExteriorMod>();
        this.mods.Add(new ExteriorMod());
    }//default ctor


    /// <summary>
    ///  Return sum of default AttractionChance value with it
    /// Exterior mods. If no mods attached - return truck's default.
    /// </summary>
    public float GetAttractionChance() {
        float total = 0;
        for (int i = 0; i < this.mods.Count; i++) {
            total += this.mods[i].AttractionChance;
        }//for
        return total;
    }//GetAttractionChance


    /// <summary>
    ///  How much price can be leveraged with the Disctrict expectations.
    /// </summary>
    public float GetPriceSwing() {
        float total = 0;
        for (int i = 0; i < this.mods.Count; i++) {
            total += this.mods[i].PriceSwing;
        }//for
        return total;
    }//GetPriceSwing

}//class Exterior
