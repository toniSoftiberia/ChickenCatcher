using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    public static float score = 0;

    Text text;

	// Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();
        score = GameObject.FindGameObjectsWithTag("Bird").Length;
	}
	
	// Update is called once per frame
    void Update()
    {
        text.text = score.ToString();
	}
}
