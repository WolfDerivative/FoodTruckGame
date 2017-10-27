using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Savable {

    public GameData GameStateData {
        get {
            if(this.gameData == null)
                this.gameData = new GameData();
            return this.gameData;
        }//get
    }//GameStateData

    private List<DistrictData> districtsList;
    [SerializeField] private DistrictData[] districts;
    [SerializeField] private GameData gameData;


    /// <summary>
    ///  Save data of the current district.
    /// </summary>
    /// <param name="data">Current district savable data.</param>
    public void SaveDistrict(DistrictData data) {
        if (this.districtsList == null)
            this.districtsList = new List<DistrictData>();

        bool dataFound = false;
        for(int i=0; i < this.districtsList.Count; i++) {
            if (!this.districtsList[i].Name.Equals(data.Name))
                continue;
            this.districtsList[i] = data;
            dataFound = true;
            break;
        }//for
        //At this point, 'data' district was not found in the save data
        //therefore, add a new entry.
        if(!dataFound)
            this.districtsList.Add(data);
        this.districts = this.districtsList.ToArray();
    }//SetDistrict


    /// <summary>
    ///  Returns DistrictData if it exists in the savedata file with
    /// passed name. Otherwise - return default/empty DistrictData obj.
    /// </summary>
    /// <param name="name">District name to get.</param>
    public DistrictData GetDistrictDataByName(string name) {
        if (this.districtsList == null)
            this.districtsList = new List<DistrictData>();

        for (int i = 0; i < this.districtsList.Count; i++) {
            if (this.districtsList[i].Name.Equals(name))
                return this.districtsList[i];
        }//for
        return new DistrictData(name);
    }//GetDistrictDataByName


    public void SetGameStateData(GameData gd) {
        this.gameData = gd;
    }//SetGameData


    public DistrictData[] GetDistricts() {
        return this.districts;
    }//GetDistricts

}//class


[System.Serializable]
public class DistrictData {
    public string Name { get { return this.name; } }

    [SerializeField] private string name;
    [SerializeField] private float reputation;


    public DistrictData(string n) {
        this.name = n;
    }//ctor

    /// <summary>
    ///  Reputation valu to be saved.
    /// </summary>
    public void SetReputation(float val) {
        reputation = val;
    }//SetReputation

    public float GetReputationStatus() { return this.reputation; }
}//class DistrictData


[System.Serializable]
public class GameData {
    public int CurrentDay       { get { return this.currentDay; } }
    public Storage SavedStorage { get { return _storage; } }

    [SerializeField]private int currentDay;
    [SerializeField]private Storage _storage;


    public void SetCurrentDay(int day) { this.currentDay = day; }
    public void SetStorage(Storage storage) { this._storage = storage; }
}//class GameData