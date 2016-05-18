using UnityEngine;
using System.Collections;

public class HighScores : MonoBehaviour {

    const string privateCode = "GaOGpRBzPE-NnICkZjiMpAmFDFyzyqkkG7Ijw3Iq01eA";
    const string publicCode = "5739ea846e51b60f80ccc20a";
    const string webURL = "http://dreamlo.com/lb/";

    public HighScore[] highScoresList;

    static HighScores instance;

    MenuController menuController;

    void Awake() {
        instance = this;
        menuController = GetComponent<MenuController>();

        //DownloadHighScores();
    }

    public void AddNewHighScore(string userName, int score) {
        StartCoroutine(UploadNewHighScore(Clean(userName), score));
    }

    IEnumerator UploadNewHighScore(string username, int score) {

        WWW www = new WWW(webURL + privateCode + "/add-pipe/" + WWW.EscapeURL(username) + "/" + score);
        yield return www;

        if (string.IsNullOrEmpty(www.error)) {
            print("Upload succes");
            //DownloadHighScores();
        } else {
            print("Error uploading");
            Debug.Log("Error uploading:" + www.error);
        }
    }

    public void DownloadHighScores() {
        StartCoroutine("DownloadHighScoreFromDatabase");
    }

    IEnumerator DownloadHighScoreFromDatabase() {
        WWW www = new WWW(webURL + publicCode + "/pipe");
        yield return www;

        if (string.IsNullOrEmpty(www.error)) {
            FormaHighScores(www.text);
            menuController.OnHighScoresDownloaded(highScoresList);
        } else {
            print("Error downloading");
            Debug.Log("Error downloading:" + www.error);
        }
    }

    void FormaHighScores(string textStream) {
        string[] entries = textStream.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);

        highScoresList = new HighScore[entries.Length];

        for (int i = 0; i <entries.Length; ++i) {
            string[] entryInfo = entries[i].Split(new char[] { '|' }, System.StringSplitOptions.RemoveEmptyEntries);
            string username = entryInfo[0];
            int score = int.Parse(entryInfo[1]);
            highScoresList[i] = new HighScore(username, score);
            Debug.Log(highScoresList[i].username + ": " + highScoresList[i].score);
        }
    }


    // Keep pipe and slash out of names

    string Clean(string s) {
        s = s.Replace("/", "");
        s = s.Replace("|", "");
        return s;

    }
}


public struct HighScore {
    public string username;
    public int score;

    public HighScore(string _username, int _score) {
        username = _username;
        score = _score;
    }
}