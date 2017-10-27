using UnityEngine;

[System.Serializable]
public class DayModifier : Modifier {
    [Tooltip("Day of the week to apply this class' property")]
    public Calendar.Day ActiveDay;

    public DayModifier(Calendar.Day day) {
        this.ActiveDay = day;
    }//ctor


    public override string ToString() {
        return string.Format("ActiveDay: {0}; {1}",
            ActiveDay.ToString(), base.ToString());
    }//ToString
}//class DayModifier
