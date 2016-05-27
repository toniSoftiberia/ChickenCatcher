using UnityEngine;
using System.Collections;

public class AddPointEffect : MonoBehaviour {

    public GameObject addPointEffect;
    public float timeEffect = 0.25f;

    public AudioSource AddPointAudio;

    float elapsedTimeEffect = 0;

    // Use this for initialization
    void Start () {

        addPointEffect.SetActive(false);
    }

    // Update is called once per frame

    void Update() {


        if (timeEffect > elapsedTimeEffect) {
            elapsedTimeEffect += Time.deltaTime;
            addPointEffect.SetActive(true);
        } else {
            addPointEffect.SetActive(false);
        }
    }

    public void Run() {
        elapsedTimeEffect = 0;
        AddPointAudio.Play();
    }
}
