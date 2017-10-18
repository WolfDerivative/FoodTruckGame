using UnityEngine;


[System.Serializable]
public class Rating {
    public enum Grade { F = 1, C = 2, B = 3, A = 4 }

    public float F = -1;
    public float C = 0;
    public float B = 1;
    public float A = 2;

    public virtual float Points { get { return finalScore; } }
    public virtual Grade FinalGrade { get { return finalGrade; } }

    private Grade finalGrade; //set when RatioByRange is called.
    private float finalScore;

    /// <summary>
    ///  Return points value\bonus for the provided grade. Note: it is not
    /// the enum value!
    /// </summary>
    /// <param name="grade">Grade to get value bonus of.</param>
    /// <returns>Value bonus of the given grade.</returns>
    public float PointsFromGrade(Grade grade) {
        switch (grade) {
            case (Grade.A):
                return A;
            case (Grade.B):
                return B;
            case (Grade.C):
                return C;
            default:
                return F;
        }//swotch
    }//PointsFromGrade


    /// <summary>
    ///  Get the grade for the recieved value in a given range.
    /// Return Grade when "recieved" falls into the given "range".
    /// </summary>
    /// <param name="range">Range defining the grade. X = C, Y = A...</param>
    /// <param name="received">Feedback value given by the customer.</param>
    /// <param name="isReverse">X becomes upper bracket (e.g. Y).</param>
    public virtual Grade GradeByRange(float received) {
        return Grade.F;
    }//SatisfactionValue


    /// <summary>
    ///  Return grade that is falling into the points values defined
    /// by A,B,C and F values. E.g. when A=4 and B=2 and you recieved 3 points,
    /// then the Grade would fall between A and B - therefore return B.
    /// </summary>
    /// <param name="poinst"></param>
    /// <returns></returns>
    public Grade GradeFromPoints(float poinst) {
        Vector2 range = new Vector2(A, float.PositiveInfinity); // A grade
        if (this.isInRange(range, poinst))
            return Grade.A;

        range = new Vector2(B, A - 1);  // B grade
        if (this.isInRange(range, poinst))
            return Grade.B;

        range = new Vector2(C, B - 1); //C grade
        if (this.isInRange(range, poinst))
            return Grade.C;

        return Grade.F;
    }//PointsToGrade


    /// <summary>
    ///  Check if given value is fallin into range (x and y inclusive).
    /// </summary>
    /// <param name="range">Range to check value agains.</param>
    /// <param name="val">Value to check if falling in range.</param>
    private bool isInRange(Vector2 range, float val) {
        if (val >= range.x && val <= range.y)
            return true;
        return false;
    }//isInRange


    /// <summary>
    ///  Convert int value to EnumGrade object.
    ///  E.g. return F, if the value less than 1. Or A if 4 and higher
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public Grade IntToEnumGrade(int val) {
        if (val < 1)
            return Grade.F;
        if (val > Grade.GetNames(typeof(Grade)).Length)
            return Grade.A;
        return (Grade)val;
    }//IntToEnumGrade


    public void SetGrade(Grade grd) { this.finalGrade = grd; }
    public void SetScore(float score) { this.finalScore = score; }
}//Rating
