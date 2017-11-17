using UnityEngine;

//FIXME: This whole thing should be depr! ConsumerRating does the same thing.

[System.Serializable]
public class ServicePreference {

    /// <summary>
    ///  Get the grade for the recieved value in a given range.
    /// Return Grade when "recieved" falls into the given "range".
    /// </summary>
    /// <param name="range">Range defining the grade. X = C, Y = A...</param>
    /// <param name="received">Feedback value given by the customer.</param>
    /// <param name="isReverse">X becomes upper bracket (e.g. Y).</param>
    //public virtual Rating.Grade RatioByRange(Vector3 range, float received) {
    //    Rating.Grade grade = Rating.Grade.F;

    //    if (received < range.x)
    //        grade = Rating.Grade.F;

    //    if (received >= range.x && received < range.y)
    //        grade = Rating.Grade.C;

    //    if (received >= range.y && received < range.z)
    //        grade = Rating.Grade.B;

    //    if (received == range.z)
    //        grade = Rating.Grade.A;

    //    if (received > range.z)
    //        grade = Rating.Grade.F;

    //    Debug.Log(grade + " -> " + received + " | " + range);
    //    return grade;
    //}//SatisfactionValue

}//class


[System.Serializable]
public class RecepePreference : ServicePreference{
    [Tooltip("Recepe prefs from C to A (F = outside range). Lower border inclusive.")]
    public Vector3 BrainsRange, SeasoningRange, DrinksRange;
}//class RecepePreference


[System.Serializable]
public class PricePreference : ServicePreference {
    public float Price = 5.0f;
    [Tooltip("[(Price * Satistaction) + Price] Highest price to satisfy a customer.")]
    [Range(0.0f, 1.0f)]
    public float SatisfactionSwing = 0.25f;
    [Tooltip("For every MismatchPenaltyRatio amount the desired price doesn't match shop's price.")]
    public float MismatchPenaltyRatio = 0.15f;
    [Tooltip("Reduce attraction chance by PenaltyPercent when desiered price doesn't match shop's price.")]
    [Range(0.0f, 1.0f)]
    public float PenaltyPercent = 0.1f;
}//PricePreference class


[System.Serializable]
public class WaitTimePreference : ServicePreference {
    [Tooltip("Range between X and Y is a Good satisfaction rate. Bellow X is excelent..")]
    public Vector3 WaitTime = new Vector3(2f, 3f, 4f);
    //used by BasicAI to get randoim wait value
    public Vector2 MaxWaitTime = new Vector2(4.5f, 6f);
}//PricePreference class
