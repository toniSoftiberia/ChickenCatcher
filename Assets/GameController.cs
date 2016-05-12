using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour {
    //public static float time = 120f;
    public static float time = 60f;

    public static float score = 0;

    public Text timeText;
    public Text scoreText;
    public Text gameOverText;

    // Use this for initialization
    void Start()
    {
        score = GameObject.FindGameObjectsWithTag("Bird").Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (time < 0)
        {
            Debug.Log("Game Over Time");
            timeText.text = "0:00:00";
            GameOver();
        }
        else
        {
            time -= Time.deltaTime;
            TimeSpan ts = TimeSpan.FromMilliseconds(time * 60000);
            timeText.text = ts.ToString().Substring(1, 7);
        }

        scoreText.text = score.ToString();
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over!!";
        gameOverText.color = Color.red; 
        gameOverText.gameObject.SetActive(true);
    }


    public void LevelComplete()
    {
        gameOverText.text = "Congratulations!";
        gameOverText.color = Color.cyan;
        gameOverText.gameObject.SetActive(true);
    }

    
}
