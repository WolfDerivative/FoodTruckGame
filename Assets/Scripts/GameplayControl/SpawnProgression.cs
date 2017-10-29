using UnityEngine;


public class SpawnProgression : MonoBehaviour {

    public AnimationCurve SpawnCurve;
    public float SpawnDelay = 0.5f;
    /// <summary>
    ///  Number of Keys (spawns) defined on the curve.
    /// </summary>
    public int Length { get { return SpawnCurve.keys.Length; } }
    public Keyframe[] CurveFrame { get { return SpawnCurve.keys; } }

    /// <summary>
    ///  Return Value for the given keyframe index.
    /// Value represents the number of Spawns at given time.
    /// </summary>
    /// <param name="keyIndex">Index to get a value on the curve.</param>
    public float GetValue(int keyIndex) {
        Keyframe[] keys = SpawnCurve.keys;
        if (keyIndex > keys.Length - 1)
            return -1;
        return keys[keyIndex].value;
    }//GetValue


    /// <summary>
    ///  Time value for the given keyframe on the curve.
    /// </summary>
    public float GetTime(int keyIndex) {
        Keyframe[] keys = SpawnCurve.keys;
        if (keyIndex > keys.Length - 1)
            return -1;
        return keys[keyIndex].time;
    }//GetTime


    public float GetSubwaveDelay(int currentIndex) {
        Keyframe[] keys = SpawnCurve.keys;
        if (currentIndex >= keys.Length - 1)
            return -1;
        return keys[currentIndex + 1].time - keys[currentIndex].time;
    }//GetSubwaveDelay


    public void SetSpawnCurve(Keyframe[] keys) {
        if (keys == null) {
            GameUtils.Utils.WarningMessage("Setting null to SetSpawnCurve of SpawnProgression("+this.name+")");
            return;
        }
        SpawnCurve = new AnimationCurve(keys);
    }//SetCurve


    public void SetSpawnDelay(float value) {
        SpawnDelay = value;
    }//SetSpawnDelay


    /// <summary>
    ///  Return sum of all the values on the SpawnCurve.
    /// </summary>
    public float GetTotalValues() {
        float total = 0;
        for (int i = 0; i < Length; i++) {
            total += GetValue(i);
        }//for
        return total;
    }//GetTotalValues


    public override string ToString() {
        return string.Format("Name: {0}; Length: {1};",
            this.name,
            Length);
    }//ToString
}//Property
