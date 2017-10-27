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


    public ConsumerRating.Grade GetAverageGrade() {
        int avg = 0;
        for(int i=0; i < reports.Count; i++) {
            int grade = (int)reports[i].Rating.FinalGrade;
            avg += grade;
        }//for
        Rating rating = new Rating();
        int gradeInt = 0;
        if(reports.Count > 0)
            Mathf.FloorToInt(avg / reports.Count);
        return rating.IntToEnumGrade(gradeInt);
    }//GetAverageGrade
}//class


[System.Serializable]
public class ConsumerReport {

    public ConsumerRating Rating { get { return this._rating; } }
    public string Name { get { return this.sConsumerName; } }
    public float TimeWaited { get { return this.timeWaited; } }

    protected ConsumerRating _rating;
    protected string sConsumerName;
    protected float timeWaited;

    public ConsumerReport(ConsumerRating r, string name, float waited) {
        this._rating = r;
        this.sConsumerName = name;
        this.timeWaited = waited;
    }


    public override string ToString() {
        string result = "";
        result += Name + ": ";
        result += _rating;
        return result;
    }
}//class
