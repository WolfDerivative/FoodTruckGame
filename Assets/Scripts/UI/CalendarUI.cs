using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CalendarUI : UISetter {

    private string todayGOName = "Today";
    private Text tTodaysDay;


    public void Start() {
        this.tTodaysDay = GetTextCmp(todayGOName);
    }//Start


    public void Update() {
        if(Calendar.Instance == null)
            return;
        if(tTodaysDay.text.Equals(Calendar.Instance.Today.ToString()))
            return;

        SetTodaysDayTxt(Calendar.Instance.Today.ToString());
    }//Update


    public void SetTodaysDayTxt(string txt) {
        if (tTodaysDay == null)
            this.tTodaysDay = GetTextCmp(todayGOName);
        tTodaysDay.text = txt;
    }//SetTodaysDayTxt


    public void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }//OnEnable


    public void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }//OnDisable


    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if(Calendar.Instance == null)
            return;
        //SetTodaysDayTxt(Calendar.Instance.Today.ToString());
    }//OnSceneLoaded

}//class CalendarUI
