using UnityEngine;
using UnityEngine.UI;

public class UILevelComplete : UIMenuPopup {

    public GameObject GradeValueGO;
    public GameObject ServedValueGO;
    public GameObject ReputationValueGO;
    
    private Text tGradeValue;
    private Text tServedValue;
    private Text tRepValue;


    public override void Start() {
        base.Start();
        tGradeValue = this.getTextCmp(GradeValueGO);
        tServedValue = this.getTextCmp(ServedValueGO);
        tRepValue = this.getTextCmp(ReputationValueGO);
    }//Start


    public void LateUpdate() {
        if (LevelManager.Instance.IsLevelComplete) {
            if(IsStatsShown)
                return;
            this.Show();
        } else {
            this.Hide();
        }
    }//Update


    private Text getTextCmp(GameObject go) {
        if(go == null) {
            return null;
        }
        Text txt = go.GetComponent<Text>();
        if (txt == null)
            GameUtils.Utils.WarningMessage("No Text cpm in GO " + go.name + "!");
        return txt;
    }//getTextCmp


    public override void Show() {
        base.Show();

        this.showLevelGrade();
        this.showSpawns();
        this.showReputation();

        Debug.Log("Waited in line: " + LevelStats.Instance.AvgWaitTimeInLine());
        Debug.Log("Waited for order: " + LevelStats.Instance.AvgWaitTimeForOrder());
        Debug.Log("Max Waiting Customers: " + LevelStats.Instance.MaxWaitingCustomers);
    }//Show


    protected void showLevelGrade() {
        this.tGradeValue.text = LevelStats.Instance.GetAverageGrade().ToString();
    }//showLevelGrade


    protected void showSpawns() {
        int totalSpawns = LevelManager.Instance.TotalPotentialCustommers;
        int served = LevelStats.Instance.CustomersServed.Count;
        this.tServedValue.text = "Served: " + served + " out of " + totalSpawns;
    }//showSpawns


    protected void showReputation() {
        int rep = Mathf.FloorToInt(District.Instance.ReputationStatus);
        int max = Mathf.FloorToInt(District.Instance.MaxReputationValue);
        this.tRepValue.text = rep + " / " + max;
    }//showReputation
}//class
