using System.Collections.Generic;
using UnityEngine;

public class LevelStats : MonoBehaviour {

    public static LevelStats Instance;

    public List<ConsumerReport> CustomersServed { get { return this.reports; } }
    public int MaxWaitingCustomers { get { return Shop.Instance.MaxWaitingQueue; } }

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
            weight = Mathf.FloorToInt(weight / LevelManager.Instance.TotalPotentialCustommers);
        Rating rating = new Rating();
        return rating.IntToEnumGrade(Mathf.FloorToInt(weight));
    }//GetAverageGrade


    /// <summary>
    ///  How long did customers stand in line (on average) before
    /// making an order.
    /// </summary>
    /// <returns></returns>
    public float AvgWaitTimeInLine() {
        float totalTime = 0;
        for (int i = 0; i < reports.Count; i++) {
            totalTime += reports[i].TimeWaitedInLine;
        }//for
        return totalTime / reports.Count;
    }//AvgWaitTimeInLine


    /// <summary>
    ///  How long did customers waited for their order to arrive.
    /// </summary>
    /// <returns></returns>
    public float AvgWaitTimeForOrder() {
        float totalTime = 0;
        for (int i = 0; i < reports.Count; i++) {
            totalTime += reports[i].TimeWaitedForOrder;
        }//for
        return totalTime / reports.Count;
    }//AvgWaitTimeForOrder

}//class LevelStats


/// <summary>
///  Collect Service Reports from consumers when they recieve their order.
/// </summary>
[System.Serializable]
public class ConsumerReport {

    public ConsumerRating Rating    { get { return this._rating; } }
    public string Name              { get { return this.sConsumerName; } }
    public float TimeWaitedInLine   { get { return this.timeWaitedInLine; } }
    public float TimeWaitedForOrder { get { return this.timeWaitedForOrder; } }

    protected ConsumerRating _rating;
    protected string sConsumerName;
    protected float timeWaitedInLine;
    protected float timeWaitedForOrder;

    public ConsumerReport(ConsumerRating r, BasicAI client) {
        this._rating = r;
        this.sConsumerName = client.name;
        this.timeWaitedInLine = client.TimeWaitedInLine;
        this.timeWaitedForOrder = client.TimeWaitedForOrder;
    }//ctor


    public override string ToString() {
        string result = "";
        result += Name + ": ";
        result += _rating;
        return result;
    }//ToString
}//class
