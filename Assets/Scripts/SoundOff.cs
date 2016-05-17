using UnityEngine;

public class SoundOff : MonoBehaviour {

    public GameObject audioSource;
    bool soundToggle = true;


    void OnGUI() {
        soundToggle = !soundToggle;
        if (soundToggle) {
            audioSource.SetActive(true);
        } else {
            audioSource.SetActive(false);
        }
    }
}
