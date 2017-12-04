using UnityEngine;

[System.Serializable]
public class ExteriorMod {

    [Tooltip("How much Attraction value to add/substract from default ChanceOfAttraction.")]
    [Range(0f, 0.5f)] public float AttractionChance = 0.001f;   
    [Tooltip("How much cents can be added/reduced to the district's acceptable Price range")]
    [Range(0f, 5f)] public float PriceSwing = 0.1f;


    /// <summary>
    /// Set defaults. AttractionChance=0.5; PriceSwing = 0;
    /// </summary>
    public ExteriorMod() {
        this.AttractionChance = 0.5f;
        this.PriceSwing = 0;
    }//default ctor


}//class ExteriorMod
