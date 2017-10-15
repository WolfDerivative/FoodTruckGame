using UnityEngine;
using UnityEngine.UI;

public class UILevelComplete : UIMenuPopup {

    public GameObject GradeValueGO;
    public GameObject ServedValueGO;

    private Text tGradeValue;
    private Text tServedValue;


    public override void Start() {
        base.Start();
        tGradeValue = this.getTextCmp(GradeValueGO);
        tServedValue = this.getTextCmp(ServedValueGO);
    }//Start


    public void Update() {
        if (LevelManager.Instance.IsLevelComplete)
            this.Show();
        else
            this.Hide();
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
        this.tGradeValue.text = LevelStats.Instance.GetAverageGrade().ToString();

        int totalSpawns = LevelManager.Instance.GetTotalSpawns();
        int served = LevelStats.Instance.CustomersServed.Count;

        this.tServedValue.text = totalSpawns + "/" + served;
    }//Show
}//class
