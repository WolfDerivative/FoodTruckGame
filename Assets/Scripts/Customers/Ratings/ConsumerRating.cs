using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConsumerRating : Rating {

    public IngredientRating BrainsPref        = new IngredientRating(Resources.ResourceType.Brains);
    public IngredientRating SeasoningsPref    = new IngredientRating(Resources.ResourceType.Seasonings);
    public IngredientRating DrinksPref        = new IngredientRating(Resources.ResourceType.Drinks);
    public WaittimeRaiting WaitTimePref       = new WaittimeRaiting(Resources.ResourceType.Waittime);

    public override float Points { get { return GetPoints(); } }
    public override Grade FinalGrade {
        get {
            return GradeFromPoints(Points);
        }
    }

    private List<IngredientRating> _ratingTypes;

    public override string ToString() {
        string result = "";
        result += "Brains: " + BrainsPref.FinalGrade + "\n";
        result += "Seasonings: " + SeasoningsPref.FinalGrade + "\n";
        result += "Drinks: " + DrinksPref.FinalGrade + "\n";
        result += "WaitTime: " + WaitTimePref.FinalGrade;
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

        //ingRating.SetGrade(lowest);
        return ingRating;
    }//GetLowestGrade


    public List<IngredientRating> GetRatingTypes() {
        if (this._ratingTypes == null) {
            this._ratingTypes = new List<IngredientRating>() {
                            BrainsPref,
                            SeasoningsPref,
                            DrinksPref,
                            WaitTimePref
                        };
        }//if
        return this._ratingTypes;
    }//GetRatingTypes


    public float GetPoints() {
        List<IngredientRating> ratings = GetRatingTypes();
        float total = 0;
        for (int i=0; i < ratings.Count; i++) {
            total += ratings[i].Points;
        }
        return total;
    }//GetPoints

    public ConsumerRating GetSatisfactionRatio(Recepe received, float timeWaited) {
        BrainsPref.GradeByRange(received.Brains.Count);
        SeasoningsPref.GradeByRange(received.Seasonings.Count);
        DrinksPref.GradeByRange(received.Drinks.Count);

        WaitTimePref.GradeByRange(timeWaited);

        return this;
    }//GetSatisfactionRatio
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


    public override Grade GradeByRange(float received) {
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
        this.SetScore(PointsFromGrade(grade));
        return grade;
    }//RatioByRange
}//Rating class


[System.Serializable]
public class WaittimeRaiting : IngredientRating {

    public WaittimeRaiting(Storage.ResourceType rtype) {
        this.ServiceType = rtype;
    }

    public override Grade GradeByRange(float received) {
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
        this.SetScore(PointsFromGrade(grade));
        return grade;
    }//RatioByRange
}