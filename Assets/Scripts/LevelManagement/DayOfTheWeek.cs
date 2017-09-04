using UnityEngine;

[System.Serializable]
public class DayOfTheWeek : MonoBehaviour{

    public static DayOfTheWeek Instance;

    public enum Day { Mon, Tue, Wed, Thus, Fri, Sat, San }
    public Day ActiveDay;
    [Tooltip("Min and Max amount of potential customers for this day.")]
    public Vector2 CustomerRange = new Vector2(15f, 20f);

}//class DayOfTheWeek