using UnityEngine;
using System.Collections;

public class HighScores : MonoBehaviour {

    #region Constants
    /// <summary>
    /// Constants to keep the scores info reference
    /// </summary>
    const string privateCode = "GaOGpRBzPE-NnICkZjiMpAmFDFyzyqkkG7Ijw3Iq01eA"; 
    const string publicCode = "5739ea846e51b60f80ccc20a";
    const string webURL = "http://dreamlo.com/lb/";
    #endregion

    #region Public attributes
    /// <summary>
    /// List of the high scores
    /// </summary>
    public HighScore[] highScoresList;
    #endregion

    #region Private attributes

    /// <summary>
    /// Reference to menu controller
    /// </summary>
    MenuController menuController;
    #endregion

    #region Public Methods
    void Awake() {

        // Get reference to menu controller
        menuController = GameObject.FindGameObjectWithTag("UI").GetComponent<MenuController>();

        // Get the scores
        DownloadHighScores();
    }

    /// <summary>
    /// Allows to add a new high score
    /// </summary>
    /// <param name="userName">Name to add</param>
    /// <param name="score">Score to add</param>
    public void AddNewHighScore(string userName, int score) {
        StartCoroutine(UploadNewHighScore(Clean(userName), score));
    }

    /// <summary>
    /// Download the highscore
    /// </summary>
    public void DownloadHighScores() {
        StartCoroutine("DownloadHighScoreFromDatabase");
    }

    #endregion

    #region Private attributes

    /// <summary>
    /// Update the highscore to the server
    /// </summary>
    /// <param name="userName">Name to add</param>
    /// <param name="score">Score to add</param>
    IEnumerator UploadNewHighScore(string username, int score) {

        // Build www whith correct url
        WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(username) + "/" + score);
        yield return www;

        // Check if it works
        if (string.IsNullOrEmpty(www.error)) {
            print("Upload succes");
            DownloadHighScores();
        } else {
            print("Error uploading");
            Debug.Log("Error uploading:" + www.error);
        }
    }

    /// <summary>
    /// Download the highscore from the server
    /// </summary>
    IEnumerator DownloadHighScoreFromDatabase() {

        // Build www whith correct url
        WWW www = new WWW(webURL + publicCode + "/pipe");
        yield return www;

        // Check if it works
        if (string.IsNullOrEmpty(www.error)) {
            FormatHighScores(www.text);
            menuController.OnHighScoresDownloaded(highScoresList);
        } else {
            print("Error downloading");
            Debug.Log("Error downloading:" + www.error);
        }
    }

    /// <summary>
    /// Parses the score to desired format
    /// </summary>
    /// <param name="textStream">string with all data</param>
    void FormatHighScores(string textStream) {

        // Split scores
        string[] entries = textStream.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);

        // Create new entry
        highScoresList = new HighScore[entries.Length];

        // Format the string
        for (int i = 0; i <entries.Length; ++i) {
            // Split entries of single score
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
    #endregion
}

#region Public Struct
/// <summary>
/// Struct to allocate high scores
/// </summary>
public struct HighScore {
    public string username;
    public int score;

    public HighScore(string _username, int _score) {
        username = _username;
        score = _score;
    }
}
#endregion