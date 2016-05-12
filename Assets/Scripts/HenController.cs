using UnityEngine;
using System.Collections;

public class HenController : MonoBehaviour {

    public float extraTime = 10f;

    public TimerManager timer;

    int targets = 0;

    CatcherController catcherController;

    GameController gameController;

    void Start() {
        catcherController = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<CatcherController>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponentInChildren<GameController>();
    }

    void OnTriggerEnter(Collider other) {

        if (other.tag == "Bird") {
            --GameController.score;
            Destroy(other.gameObject);
            catcherController.ResetAndFlipPosition();
            GameController.time += extraTime;
            if (GameController.score == 0)
            {
                Debug.Log("Level Complete");
                gameController.LevelComplete();
            }
        }
    }
}
