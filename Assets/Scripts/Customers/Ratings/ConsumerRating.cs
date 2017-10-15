using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConsumerRating : Rating {

    public IngredientRating BrainsRating = new IngredientRating(Resources.ResourceType.Brains);
    public IngredientRating SeasoningsRating = new IngredientRating(Resources.ResourceType.Seasonings);
    public IngredientRating DrinksRating = new IngredientRating(Resources.ResourceType.Drinks);
    public WaittimeRaiting WaitTimeRating;
    public float ServiceScore {
        get {
            return PointsFromGrade(ServiceGrade);
        }
    }//ServiceScore
    public Grade ServiceGrade { get { return this.calculateServiceGrade(); } }

    private List<IngredientRating> _ratingTypes;


    private Grade calculateServiceGrade() {
        float total = 0;
        total += BrainsRating.Points;
        total += SeasoningsRating.Points;
        total += DrinksRating.Points;
        total += WaitTimeRating.Points;
        Grade finalGrade = GradeFromPoints(total);
        return finalGrade;
    }//calculateServiceScore


    public override string ToString() {
        string result = "";
        result += "Brains: " + BrainsRating.FinalGrade + "\n";
        result += "Seasonings: " + SeasoningsRating.FinalGrade + "\n";
        result += "Drinks: " + DrinksRating.FinalGrade + "\n";
        result += "WaitTime: " + WaitTimeRating.FinalGrade;
        return result;
    }//ToString


    public IngredientRating GetLowestGrade() {
        Grade lowest = Grade.F;
        IngredientRating ingRating = new IngredientRating();
        for(int i=0; i<GetRatingTypes().Count; i++) {
            Grade grade = GetRatingTypes()[i].FinalGrade;
            if (grade < lowest) {
                lowest = grade;
                ingRating = new IngredientRating(GetRatingTypes()[i].ServiceType);
            }//if
        }//for

        ingRating.SetGrade(lowest);
        return ingRating;
    }//GetLowestGrade


    public List<IngredientRating> GetRatingTypes() {
        if (this._ratingTypes == null) {
            this._ratingTypes = new List<IngredientRating>() {
                            BrainsRating,
                            SeasoningsRating,
                            DrinksRating,
                            WaitTimeRating
                        };
        }//if
        return this._ratingTypes;
    }//GetRatingTypes

}//class


[System.Serializable]
public class IngredientRating : Rating{

    public Vector3 Range;
    public Storage.ResourceType ServiceType;

    public IngredientRating(){}

    public IngredientRating(Storage.ResourceType rtype) {
        this.ServiceType = rtype;
    }//ctor


    public override string ToString() {
        return this.ServiceType + " : " + this.FinalGrade;
    }


    public override Grade RatioByRange(float received) {
        Grade grade = Grade.F;

        if (received < Range.x)
            grade = Grade.F;

        if (received >= Range.x && received < Range.y)
            grade = Grade.C;

        if (received >= Range.y && received < Range.z)
            grade = Grade.B;

        if (received == Range.z)
            grade = Grade.A;

        if (received > Range.z)
            grade = Grade.F;
        
        this.SetGrade(grade);
        return grade;
    }//RatioByRange
}//Rating class


[System.Serializable]
public class WaittimeRaiting : IngredientRating {
    public override Grade RatioByRange(float received) {
        Grade grade = Grade.F;

        if (received < Range.x)
            grade = Grade.A;

        if (received >= Range.x && received < Range.y)
            grade = Grade.B;

        if (received >= Range.y && received < Range.z)
            grade = Grade.C;

        if (received >= Range.z)
            grade = Grade.F;
        
        this.SetGrade(grade);
        return grade;
    }//RatioByRange
}