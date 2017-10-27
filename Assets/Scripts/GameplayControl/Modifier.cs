using UnityEngine;

[System.Serializable]
public class Modifier {

    [Tooltip("Wave curve multiplier. It is a Max spawn amount on the key curve. E.g. range=(curve_value, curve_value + curve_value*SpawnMultiplier);")]
    public float SpawnRange = 1;
    [Tooltip("Prefab with the SpawnProgression component to be used for spawn behaviour.")]
    public GameObject ProgressionPrefab;
    public SpawnProgression Progression { get { return this._progression; } }

    private SpawnProgression _progression;


    public void SetProgression(SpawnProgression sp) {
        _progression = sp;
    }//SetProgression


    public override string ToString() {
        string waveGoName = (ProgressionPrefab == null) ? "Not Set!" : ProgressionPrefab.name;
        bool isProgressionSet = this.Progression != null;
        return string.Format("Mulriplier: {0}; Wave: {1}; Progression: {2}",
            SpawnRange, waveGoName, isProgressionSet);
    }//ToString

}//class Modifier
