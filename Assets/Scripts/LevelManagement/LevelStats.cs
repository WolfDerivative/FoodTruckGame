using System.Collections.Generic;
using UnityEngine;

public class LevelStats : MonoBehaviour {

    public static LevelStats Instance;

    public List<ConsumerReport> CustomersServed {
        get {
            return this.reports;
        }
    }//CustomersServed

    private List<ConsumerReport> reports;


    public void Start() {
        Instance = this;
        reports = new List<ConsumerReport>();
    }//Start


    public void AddReport(ConsumerReport report) {
        reports.Add(report);
    }//AddReport


    public Rating.Grade GetAverageGrade() {
        float weight = 0;
        for(int i=0; i < reports.Count; i++) {
            Rating.Grade grade = reports[i].Rating.ServiceGrade;
            weight += (int)reports[i].Rating.ServiceGrade;
        }//for
        if (reports.Count > 0)
            weight = Mathf.FloorToInt(weight / reports.Count);
        Rating rating = new Rating();
        return rating.IntToEnumGrade(Mathf.FloorToInt(weight));
    }//GetAverageGrade

}//class LevelStats


/// <summary>
///  Collect Service Reports from consumers when they recieve their order.
/// </summary>
[System.Serializable]
public class ConsumerReport {

    public ConsumerRating Rating    { get { return this._rating; } }
    public string Name              { get { return this.sConsumerName; } }
    public float TimeWaited         { get { return this.timeWaited; } }

    protected ConsumerRating _rating;
    protected string sConsumerName;
    protected float timeWaited;

    public ConsumerReport(ConsumerRating r, string name, float waited) {
        this._rating = r;
        this.sConsumerName = name;
        this.timeWaited = waited;
    }//ctor


    public override string ToString() {
        string result = "";
        result += Name + ": ";
        result += _rating;
        return result;
    }//ToString
}//class
