using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public Text timeText;
    public Text extraTimeText;
    public Text scoreTextChick;
    public Text scoreTextChicken;
    public Text gameOverText;
    public Text scoreCountText;
    public Text pressKey;

    public Button submitScore;
    public Button MainMenuButton;

    public Image leftIndicator;
    public Image rightIndicator;
    public Image bottomIndicator;
    public Image groundIndicator;
    public GameObject groundCanvas;

    public Image scoreChick;
    public Image scoreChicken;

    public Image pointIndicatorChick;
    public Image pointIndicatorChicken;
    public Image countDownIndicator;

    public float time = 60f;

    public float countDuration = 2f;

    public bool helpActive = true;

    float puntuation = 0;
    float totalPuntuation = 0;
    float puntuationStep = 0;

    Color yellow;
    Color green;
    Color red;
    Color blue;

    bool win = false;
    [HideInInspector]
    public bool lost = false;

    public bool gameOver = false;
    public bool gameStated = false;
    [HideInInspector]
    public List<GameObject> birds;

    HenController henController;
    FirstPersonController playerController;
    CatcherController catcherController;
    DataController dataController;
    HighScores highscoreManager;

    float level = 0;

    bool dataSaved = false;


    public AudioSource startAudio;
    public AudioSource pauseAudio;
    public AudioSource nextLevelAudio;
    public AudioSource gameCompleteAudio;
    public AudioSource gameOverAudio;

    AudioSource backgroundMusic;

    CursorController cursor;

    // Use this for initialization
    void Start() {
        birds = (GameObject.FindGameObjectsWithTag("Bird")).ToList(); ;
        LoadData();

        yellow = new Color(1, 1, 0, .5f);
        green = new Color(0, 1, 0, .5f);
        blue = new Color(0, 0, 1, .5f);
        red = new Color(1, 0, 0, .5f);

        henController = GameObject.FindGameObjectWithTag("HenHouse").transform.parent.GetComponent<HenController>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
        catcherController = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<CatcherController>();
        highscoreManager = GameObject.FindGameObjectWithTag("Data").GetComponent<HighScores>();

        scoreTextChick.text = (countBirds(ChickenMovement.BirdType.Chick) + countBirds(ChickenMovement.BirdType.Runner)).ToString();
        scoreTextChicken.text = countBirds(ChickenMovement.BirdType.Chicken).ToString();

        groundCanvas.transform.localScale = Vector3.zero;

        backgroundMusic = Camera.main.GetComponent<AudioSource>();

        cursor = GetComponent<CursorController>();

        dataController = GameObject.FindGameObjectWithTag("Data").GetComponent<DataController>();
    }

    // Update is called once per frame
    void Update() {

        if (gameStated) {
            if (gameOver) {
                if (puntuation < totalPuntuation) {
                    puntuation += puntuationStep * Time.deltaTime;
                    scoreCountText.text = puntuation.ToString("00000000");

                } else {
                    puntuation = totalPuntuation;
                    if (win) {
                        playerController.ActicateWinEffect();
                        if (Input.anyKey && !dataSaved) {
                            if (!dataController.IsLastLevel()) {
                                Debug.Log("Load next level");
                                dataSaved = true;
                                dataController.score += totalPuntuation;
                                ++dataController.level;
                                SceneManager.LoadScene("VRLevel " + dataController.level);
                            }
                        }
                    }
                }

                TimeSpan ts = TimeSpan.FromMilliseconds(time * 60000);
                timeText.text = ts.ToString().Substring(1, 7);

            } else {
                if (time < 0) {
                    Debug.Log("Game Over Time");
                    timeText.text = "0:00:00";
                    scoreCountText.text = "Time is over!";
                    GameOver();
                } else {
                    time -= Time.deltaTime;
                    TimeSpan ts = TimeSpan.FromMilliseconds(time * 60000);
                    timeText.text = ts.ToString().Substring(1, 7);
                }

                MarkClosestChicken();
            }

            scoreTextChick.text = (countBirds(ChickenMovement.BirdType.Chick) + countBirds(ChickenMovement.BirdType.Runner)).ToString();
            scoreTextChicken.text = countBirds(ChickenMovement.BirdType.Chicken).ToString();

            if (addPoint != ChickenMovement.BirdType.none)
                AddPointAnimation(addPoint);
        }

    }

    public void GameOver() {
        lost = true;

        gameOverAudio.Play();
        backgroundMusic.Stop();
        cursor.EnableCursor();

        henController.enabled = false;
        playerController.enabled = false;
        catcherController.enabled = false;

        submitScore.gameObject.SetActive(true);
        MainMenuButton.gameObject.SetActive(true);

        gameOverText.text = "Game Over!";
        gameOverText.color = Color.red;
        ShowPuntuationCounter();
    }

    public void StartGame() {
        gameStated = true;
        FreeBirds();
        countDownIndicator.gameObject.SetActive(false);
        startAudio.Play();
    }

    public void LevelComplete() {
        if (!lost) {

            win = true;

            cursor.EnableCursor();

            henController.enabled = false;
            playerController.enabled = false;
            catcherController.enabled = false;

            if (!dataController.IsLastLevel()) {
                nextLevelAudio.Play();
                backgroundMusic.Stop();
                pressKey.gameObject.SetActive(true);
            } else {
                gameCompleteAudio.Play();
                backgroundMusic.Stop();
                submitScore.gameObject.SetActive(true);
                MainMenuButton.gameObject.SetActive(true);
            }

            gameOverText.text = "Congratulations!";
            gameOverText.color = Color.cyan;
            ShowPuntuationCounter();
        }
    }

    void LoadData() {
        bool data = false;

        if (!data) {
            SaveData();
        }
    }

    void SaveData() {
        puntuation = 0;
    }

    void ShowPuntuationCounter() {

        gameOver = true;

        totalPuntuation = 33 * time;
        totalPuntuation += dataController.score;
        puntuation = dataController.score;
        puntuationStep = (totalPuntuation - puntuation) / (countDuration); 

        gameOverText.gameObject.SetActive(true);
        scoreCountText.text = puntuation.ToString("000000");
        scoreCountText.gameObject.SetActive(true);

        dataController.score = totalPuntuation;
    }

    GameObject FindClosestChicken() {

        ClearIndicators();

        GameObject closestChicken = null;

        if (birds != null) {

            closestChicken = birds[0];

            for (int i = 0; i < birds.Count; ++i) {
                birds[i].GetComponentInChildren<DirectionIndicator>().closest = false;
                //compares distances
                if (Vector3.Distance(Camera.main.transform.position, birds[i].transform.position) <= Vector3.Distance(Camera.main.transform.position, closestChicken.transform.position)) {
                    closestChicken = birds[i];
                }
            }

            closestChicken.GetComponentInChildren<DirectionIndicator>().closest = true;
        }

        return closestChicken;
    }

    int countBirds(ChickenMovement.BirdType bird) {
        int res = 0;

        if (birds != null) {
            for (int i = 0; i < birds.Count; ++i) {
                if(birds[i].GetComponent<ChickenMovement>().birdType == bird){
                    ++res;
                }
            }
        }

        return res;
    }

    void MarkClosestChicken() {
        if (helpActive && birds != null) {
            GameObject closestChicken = FindClosestChicken();
        }
    }

    public void ClearIndicators() {
        leftIndicator.gameObject.SetActive(false);
        rightIndicator.gameObject.SetActive(false);
        bottomIndicator.gameObject.SetActive(false);
        groundCanvas.transform.localScale = Vector3.zero;
    }

    public void ShowIndicatorLeft(ChickenMovement.BirdType bird = ChickenMovement.BirdType.none) {
        SetIndicatorColor(leftIndicator, bird);
        leftIndicator.gameObject.SetActive(true);
    }

    public void ShowIndicatorRight(ChickenMovement.BirdType bird = ChickenMovement.BirdType.none) {
        SetIndicatorColor(rightIndicator, bird);
        rightIndicator.gameObject.SetActive(true);
    }

    public void ShowIndicatorBottom(ChickenMovement.BirdType bird = ChickenMovement.BirdType.none) {
        SetIndicatorColor(bottomIndicator, bird);
        bottomIndicator.gameObject.SetActive(true);
    }

    public void ShowGroundIndicator(ChickenMovement.BirdType bird = ChickenMovement.BirdType.none) {
        SetIndicatorColor(groundIndicator, bird);
        if(!catcherController.catched)
            //groundCanvas.transform.localScale = Vector3.one;
            groundCanvas.transform.localScale = Vector3.zero;
    }

    private void SetIndicatorColor(Image img, ChickenMovement.BirdType bird) {
        if(bird == ChickenMovement.BirdType.Chick) {
            img.color = yellow;
        } else if (bird == ChickenMovement.BirdType.Chicken) {
            img.color = red;
        } else if (bird == ChickenMovement.BirdType.Runner) {
            img.color = blue;
        } else if (bird == ChickenMovement.BirdType.none) {
            img.color = green;
        }
    }

    public ChickenMovement.BirdType addPoint = ChickenMovement.BirdType.none;
    public float MinScale = .1f;
    public float MaxScale = 1f;
    public float speed = 10f;
    private int animState = 0;

    private Image plusPoint;

    private void IncreaseScale(Image img) {
        float newScale = Mathf.Lerp(img.transform.localScale.x, MaxScale, Time.deltaTime * speed);
        img.transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    public void AddPoint(ChickenMovement.BirdType bird, float extraTime) {

        addPoint = bird;

        if (bird == ChickenMovement.BirdType.Chick) {
            plusPoint = pointIndicatorChick;
            extraTimeText.color = Color.yellow;
        } else if (bird == ChickenMovement.BirdType.Chicken) {
            plusPoint = pointIndicatorChicken;
            extraTimeText.color = Color.red;
        } else if (bird == ChickenMovement.BirdType.Runner) {
            plusPoint = pointIndicatorChick;
            extraTimeText.color = Color.blue;
        }

        time += extraTime;
        extraTimeText.text = "+" + extraTime;

        plusPoint.gameObject.SetActive(true);
        plusPoint.transform.localScale = Vector3.zero;
        plusPoint.transform.localPosition = Vector3.zero;
    }

    public void AddPointAnimation(ChickenMovement.BirdType bird) {

        float newScaleExtraTIme = Mathf.Lerp(extraTimeText.transform.localScale.x, 2, Time.deltaTime * speed);
        extraTimeText.transform.localScale = new Vector3(newScaleExtraTIme, newScaleExtraTIme, newScaleExtraTIme);

        if (animState == 0) {
            float newScale = Mathf.Lerp(plusPoint.transform.localScale.x, MaxScale, Time.deltaTime * speed);
            plusPoint.transform.localScale = new Vector3(newScale, newScale, newScale);
            if (1f - newScale < 0.1f) {
                animState = 1;
            }
        } else {
            if (animState == 1) {
                float newScale = Mathf.Lerp(plusPoint.transform.localScale.x, MinScale, Time.deltaTime * speed);
                plusPoint.transform.localScale = new Vector3(newScale, newScale, newScale);
                if (newScale == 1f) {
                    animState = 1;
                }

                Vector3 targetPosition = scoreChick.transform.position;
                if ( bird == ChickenMovement.BirdType.Chicken)
                    targetPosition = scoreChicken.transform.position;

                Vector3 newPosition = Vector3.Lerp(plusPoint.transform.position, targetPosition, Time.deltaTime * speed);
                plusPoint.transform.position = newPosition;

                if ((plusPoint.transform.position - targetPosition).magnitude < 0.01f) {

                    addPoint = ChickenMovement.BirdType.none;
                    plusPoint.gameObject.SetActive(false);
                    animState = 0;
                    extraTimeText.transform.localScale = Vector3.zero;
                }
            }
            else {
                animState = 0;
                extraTimeText.transform.localScale = Vector3.zero;
            }
        }
    }

    public void SubmitScore() {
        Debug.Log("Submit Score");
        highscoreManager.AddNewHighScore(dataController.playerName, (int)dataController.score);
    }

    public void BackToMainManu() {
        Debug.Log("Main Menu");
        Destroy(dataController.gameObject);
        SceneManager.LoadScene("MainMenu");
    }

    public void StopBirds() {
        if (birds != null) {
            for (int i = 0; i < birds.Count; ++i)
                birds[i].GetComponent<ChickenMovement>().catched = true;
        }
    }

    public void FreeBirds() {
        if (birds != null) {
            for (int i = 0; i < birds.Count; ++i)
                birds[i].GetComponent<ChickenMovement>().catched = false;
        }
    }    
}