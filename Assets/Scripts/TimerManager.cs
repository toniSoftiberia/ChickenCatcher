using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TimerManager : MonoBehaviour {

    public static float time = 120f;

    Text text;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        time -= Time.deltaTime;
        TimeSpan ts = TimeSpan.FromMilliseconds(time * 60000);
        text.text = ts.ToString().Substring(1, 7);


        /*
          print("foo bar".Substring(2, 5));
will output:

 o ba
         */
    }
}
