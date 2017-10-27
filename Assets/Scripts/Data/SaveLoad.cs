using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoad {

    public string SaveFile { get {
            //path must not start with slash
            if (!this.saveFile.StartsWith("/"))
                return "/" + this.saveFile;
            return this.saveFile;
        }//get
    }//SaveFile
    public Savable DataBuffer {
        get {
            if(this.savefileData == null)
                this.savefileData = Load();
            return this.savefileData;
        }//get
    }//DataBuffer

    private string saveFile = "/save.dat";
    private Savable savefileData;


    public SaveLoad() {
        Load();
    }//SaveLoad


    public Savable Save(Savable data, bool writeToFile=false) {
        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Open(Application.persistentDataPath + SaveFile, FileMode.Create);

        bf.Serialize(file, JsonUtility.ToJson(data));
        this.savefileData = data;
        file.Close();
        return this.savefileData;
    }//Save


    public Savable Load(bool isReLoad=false) {
        if(this.savefileData != null) {
            if (!isReLoad)
                return this.savefileData;
        }//if already loaded

        if (!File.Exists(Application.persistentDataPath + SaveFile)) {
            this.savefileData = new Savable();
            return this.savefileData;
        }//if

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + SaveFile, FileMode.Open);
        this.savefileData = JsonUtility.FromJson<Savable>(bf.Deserialize(file).ToString());

        file.Close();
        return this.savefileData;
    }//Load


    public override string ToString() {
        if (this.savefileData == null)
            return "No Data";
        return JsonUtility.ToJson(this.savefileData);
    }
}//class