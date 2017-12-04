using UnityEngine;

[System.Serializable]
public class CookingMod {

    [Tooltip("How much seconds of cooktime will this stove reduce/add (per stove).")]
    [Range(0.5f, 5f)]
    public float CookSpeed = 0.1f;
    [Tooltip("How much fast cook chance will be added/reduced by this stove.")]
    [Range(0f, 1f)]
    public float FastCookChance = 0.001f;

    public string Description;
    public string DisplayName;


}//class CookingMod
