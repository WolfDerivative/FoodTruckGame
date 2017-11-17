using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConsumerRating {

    public IngredientRating BrainsPref        = new IngredientRating(Resources.ResourceType.Brains);
    public IngredientRating SeasoningsPref    = new IngredientRating(Resources.ResourceType.Seasonings);
    public IngredientRating DrinksPref        = new IngredientRating(Resources.ResourceType.Drinks);
    public WaittimeRaiting WaitTimePref       = new WaittimeRaiting(Resources.ResourceType.Waittime);
    public Rating.Grade ServiceGrade { get { return this.finalGrade; } }

    private List<IngredientRating> _ratingTypes;
    private Rating.Grade finalGrade;


    public IngredientRating GetLowestGrade() {
        Rating.Grade lowest = Rating.Grade.F;
        IngredientRating ingRating = new IngredientRating();
        var typesOfService = GetRatingTypes();
        for (int i=0; i < typesOfService.Count; i++) {
            Rating.Grade grade = typesOfService[i].FinalGrade;
            if (grade < lowest) {
                lowest = grade;
                ingRating = new IngredientRating(typesOfService[i].ServiceType);
            }//if
        }//for

        //ingRating.SetGrade(lowest);
        return ingRating;
    }//GetLowestGrade


    public List<IngredientRating> GetRatingTypes() {
        if (this._ratingTypes == null) {
            this._ratingTypes = new List<IngredientRating>() 
                                { BrainsPref, SeasoningsPref, DrinksPref, WaitTimePref };
        }//if
        return this._ratingTypes;
    }//GetRatingTypes


    /// <summary>
    ///     Calculate average grade based of the recepe recieved.
    /// This will also set the ServiceGrade value.
    /// </summary>
    /// <param name="received">Recepe that was served to the customer.</param>
    /// <param name="timeWaited">How long did customer wait to get the order.</param>
    public ConsumerRating RateService(Recepe received, float timeWaited) {
        Rating ratingObj = new Rating();
        float avg = 0;
        avg += (float)BrainsPref.GradeByRange(received.Brains.Count);
        avg += (float)SeasoningsPref.GradeByRange(received.Seasonings.Count);
        avg += (float)DrinksPref.GradeByRange(received.Drinks.Count);

        avg = avg / 3;
        int roundedAvg = 0;
        if( ((decimal)avg % 1.0m) > (decimal)0.5)
            roundedAvg = Mathf.CeilToInt(avg);
        else
            roundedAvg = Mathf.FloorToInt(avg);

        this.finalGrade = ratingObj.IntToEnumGrade(roundedAvg);

        return this;
    }//GetSatisfactionRatio


    public override string ToString() {
        string result = "";
        result += "Brains: " + BrainsPref.FinalGrade + "\n";
        result += "Seasonings: " + SeasoningsPref.FinalGrade + "\n";
        result += "Drinks: " + DrinksPref.FinalGrade + "\n";
        result += "WaitTime: " + WaitTimePref.FinalGrade;
        return result;
    }//ToString
}//class


[System.Serializable]
public class IngredientRating : Rating{

    [Tooltip("Type of service/ingredient to be rated.")]
    public Storage.ResourceType ServiceType;


    public IngredientRating(){} //default ctor


    public IngredientRating(Storage.ResourceType rtype) {
        this.ServiceType = rtype;
    }//ctor


    public override string ToString() {
        return this.ServiceType + " : " + this.FinalGrade;
    }//ToString


    public override Grade GradeByRange(float received) {
        Grade grade = Grade.F;

        if (received <= F || received > A)
            grade = Grade.F;

        if (received > F && received <= C)
            grade = Grade.C;

        if (received > C && received <= B)
            grade = Grade.B;

        if ((received > B && received <= A) || received == A)
            grade = Grade.A;
        
        this.SetGrade(grade);
        return grade;
    }//RatioByRange
}//class Rating


[System.Serializable]
public class WaittimeRaiting : IngredientRating {

    public WaittimeRaiting(Storage.ResourceType rtype) {
        this.ServiceType = rtype;
    }

    public override Grade GradeByRange(float received) {
        Grade grade = Grade.F;

        if (received <= A)
            grade = Grade.A;

        if (received > A && received <= B)
            grade = Grade.B;

        if (received > B && received <= C)
            grade = Grade.C;

        if (received > C)
            grade = Grade.F;

        this.SetGrade(grade);
        return grade;
    }//RatioByRange
}//class WaittimeRaiting