using UnityEngine;
using UnityEngine.UI;

public class UILevelComplete : UIMenuPopup {

    public GameObject GradeValueGO;
    public GameObject ServedValueGO;
    public GameObject ReputationValueGO;

    private Text tGradeValue;
    private Text tServedValue;
    private Text tRepValue;
    private bool bIsStatsShown;


    public override void Start() {
        base.Start();
        tGradeValue = this.getTextCmp(GradeValueGO);
        tServedValue = this.getTextCmp(ServedValueGO);
        tRepValue = this.getTextCmp(ReputationValueGO);
        this.bIsStatsShown = false;
    }//Start


    public void Update() {
        if (LevelManager.Instance.IsLevelComplete) {
            if(bIsStatsShown)
                return;
            this.Show();
            this.bIsStatsShown = true;
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
