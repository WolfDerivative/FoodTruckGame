using UnityEngine;

public class Calendar : MonoBehaviour {
    public static Calendar Instance;
    public enum Day { Mon, Tue, Wed, Thu, Fri, Sat, Sun }

    [Tooltip("For debugging and testing")]
    public bool IsOverrideToday = false;
    public Day OverrideToday = Day.Mon;
    public Day Today { get { return this.eToday; } }

    private Day eToday;


    public void Start(){
        if (Instance == null)
            Instance = this;
        else {
            DestroyImmediate(this.transform.parent.gameObject); //To ensure only one copy exists.
        }
        #if UNITY_EDITOR
        if (IsOverrideToday)
                SetDay(OverrideToday);
        #endif
    }//Start


    /// <summary>
    ///  Switch today's day to the next day of the week.
    /// If it is already Sunday, then it will jump to Monday.
    /// This function will be called by LevelManager when
    /// level is completed.
    /// </summary>
    /// <returns></returns>
    public Day NextDay() {
        this.eToday += 1;
        if((int)this.eToday > 6)
            this.eToday = 0;
        return this.eToday;
    }//NextDay


    /// <summary>
    ///  Set today's day.
    /// </summary>
    public void SetDay(Day d) { this.eToday = d; }

}//class