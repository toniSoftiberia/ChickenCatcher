using UnityEngine;
using System.Collections;

public class HenController : MonoBehaviour {

    public float points = 0;
    public float extraTime = 10f;

    public TimerManager timer;

    int targets = 0;

    CatcherController catcherController;

    void Start() {
        catcherController = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<CatcherController>();
        targets = GameObject.FindGameObjectsWithTag("Bird").Length;
    }

    void OnTriggerEnter(Collider other) {

        if (other.tag == "Bird") {
            ++points;
            Destroy(other.gameObject);
            catcherController.ResetAndFlipPosition();
            TimerManager.time += extraTime;
            if (points == targets) {
                Debug.Log("Game Over");
            }
        }
    }
}
