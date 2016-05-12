using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TimerManager : MonoBehaviour {

    //public static float time = 120f;
    public static float time = 1f;

    public Text timeText;
    public Text scoreText;
	
	// Update is called once per frame
	void Update () {
        time -= Time.deltaTime;
        TimeSpan ts = TimeSpan.FromMilliseconds(time * 60000);
        timeText.text = ts.ToString().Substring(1, 7);


        if (time < 0)
        {
            Debug.Log("Game Over Time");
        }
    }
}
