using UnityEngine;
using System.Collections;

public class HenController : MonoBehaviour {

    public float extraTimeChick = 20f;
    public float extraTimeChicken = 30f;
    public float extraTimeRunner = 40f;

    CatcherController catcherController;

    GameController gameController;

    void Start() {
        catcherController = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<CatcherController>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponentInChildren<GameController>();
    }

    void OnTriggerEnter(Collider other) {

        if (other.tag == "Bird") {
            if (!gameController.lost) {
                gameController.birds.Remove(other.gameObject);
                Destroy(other.gameObject);
                catcherController.ResetState();
                ChickenMovement.BirdType bird = other.gameObject.GetComponent<ChickenMovement>().birdType;
                if(bird == ChickenMovement.BirdType.Chick)
                    gameController.time += extraTimeChick;
                else if (bird == ChickenMovement.BirdType.Chicken)
                    gameController.time += extraTimeChicken;
                else if (bird == ChickenMovement.BirdType.Runner)
                    gameController.time += extraTimeRunner;

                gameController.AddPoint(bird);
                if (gameController.birds.Count == 0) {
                    Debug.Log("Level Complete");
                    gameController.LevelComplete();
                }
            }
        }
    }
}
