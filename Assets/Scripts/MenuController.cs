using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour {

    public GameObject start;
    public GameObject ranking;
    public GameObject credits;
    public GameObject exit;
    public GameObject back;
    public GameObject creditsText;
    public GameObject tittle;
    public GameObject rankingTittle;
    public GameObject rankingText;
    public Text nameInput;
    public GameObject twitterText;
    public GameObject thanksText;
    public GameObject leftImage;
    public GameObject rightImage;

    public Text[] highscoreScores;
    public Text[] highscoreNames;
    HighScores highscoreManager;

    DataController dataController;

    void Start() {
        dataController = GameObject.FindGameObjectWithTag("Data").GetComponent<DataController>();
        highscoreManager = GameObject.FindGameObjectWithTag("Data").GetComponent<HighScores>();

        for (int i = 0; i < highscoreNames.Length; ++i) {
            highscoreNames[i].text = i + 1 + ". Anonimus...";
        }

        EventSystem.current.SetSelectedGameObject(start);

        StartCoroutine("RefreshHighScores");
    }

    IEnumerator RefreshHighScores() {
        while (true) {
            highscoreManager.DownloadHighScores();
            yield return new WaitForSeconds(30);
        }
    }

    public void OnHighScoresDownloaded(HighScore[] highscoreList) {
        for (int i = 0; i < highscoreNames.Length; ++i) {
            highscoreNames[i].text = i + 1 + ". ";
            if(highscoreList.Length > i) {
                highscoreNames[i].text += highscoreList[i].username ;
                highscoreScores[i].text = highscoreList[i].score.ToString("000000");
            }
        }
    }

    public void StartGame() {
        ++dataController.level;
        if (!string.IsNullOrEmpty(nameInput.text))
            dataController.playerName = nameInput.text;
        // IF VR
        SceneManager.LoadScene("VRLevel 1");
    }

    public void ShowRanking() {
        Debug.Log("Show Ranking");
        start.SetActive(false);
        ranking.SetActive(false);
        credits.SetActive(false);
        exit.SetActive(false);
        tittle.SetActive(false);
        back.SetActive(true);
        rankingText.SetActive(true);
    }

    public void ShowCredits() {
        start.SetActive(false);
        ranking.SetActive(false);
        credits.SetActive(false);
        exit.SetActive(false);
        tittle.SetActive(false);
        back.SetActive(true);
        creditsText.SetActive(true);
        twitterText.SetActive(true);
        thanksText.SetActive(true);
        leftImage.SetActive(false);
        rightImage.SetActive(false);
    }

    public void BackToMainMenu() {
        start.SetActive(true);
        ranking.SetActive(true);
        credits.SetActive(true);
        exit.SetActive(true);
        tittle.SetActive(true);
        back.SetActive(false);
        creditsText.SetActive(false);
        rankingTittle.SetActive(false);
        rankingText.SetActive(false);
        twitterText.SetActive(false);
        thanksText.SetActive(false);
        leftImage.SetActive(true);
        rightImage.SetActive(true);
}

    public void ExitGame() {
        Application.Quit();
    }
}
