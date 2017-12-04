using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocalizationManager : MonoBehaviour {

    public static LocalizationManager Instance;

    private string language = "eng";
    /// Directory name under StreamingAssets containing all of the localization data.
    private string localization_dir = "localization";
    private Dictionary<string, Dictionary<string, string>> localized;


    public void Start() {
        if (Instance == null)
            Instance = this;
        else
            DestroyImmediate(this.gameObject);

        this.localized = new Dictionary<string, Dictionary<string, string>>();
        //string stovesUpgradesLocale = "stoves_upgrade_selection";
        //this.localized.Add(stovesUpgradesLocale, ReadLocalizationFile(stovesUpgradesLocale));
    }//Start


    public string ReadLocalizationFile(string fileName) {
        //Dictionary<string, string> locale = new Dictionary<string, string>();
        //create path to Language's dictionary
        string streamingAssetsPath = ChooseLanguage(this.language);
        if(streamingAssetsPath == null) //error message handled by ChooseLanguage
            return null;

        //building final destination to a fileName
        streamingAssetsPath = Path.Combine(streamingAssetsPath, fileName);
        if (!File.Exists(streamingAssetsPath)) {
            GameUtils.Utils.ErrorMessage("Localization File '" + fileName + 
                                            "' was not found at '" + streamingAssetsPath + "'!");
            return null;
        }//if not exists
        string fileContent = File.ReadAllText(streamingAssetsPath);
        /*
        LocalizationData localiData = JsonUtility.FromJson<LocalizationData>(fileContent);

        for (int i = 0; i < localiData.Entries.Length; i++) {
            LocalizationItem entry = localiData.GetEntry(i);
            locale.Add(entry.Key, entry.Value);
        }//for

        Debug.Log("Localization loaded " + locale.Count + " entries");
        return locale;
        */
        return fileContent;
    }//ReadLocalizationFile


    public virtual void ParseLocaleJson(string content) {
        throw new System.NotImplementedException();
    }


    /// <summary>
    ///  Choose which lang dictionary to use for localization.
    /// This is a name of the dictionary under StreamingAssets/localization/.
    /// Meaning, if there is a folder "eng", then path a string of the same name.
    /// </summary>
    /// <param name="lang">Language folder to use from StreamingAssets/localization/ </param>
    /// <returns>Full path to a StreamingAssets/localization/<lang> directory. 
    ///          Null - if not found.
    /// </returns>
    public string ChooseLanguage(string lang) {
        string languageDir = Path.Combine(Application.streamingAssetsPath, this.localization_dir);
        languageDir = Path.Combine(languageDir, lang);

        if (!Directory.Exists(languageDir)) {
            GameUtils.Utils.WarningMessage("Localization type '" + lang + "' not found!");
            return null;
        }//file does not exists

        this.language = lang;
        return languageDir;
    }//ChooseLanguage

}//class LocalizationManager
