using UnityEngine;
using System.Collections;

public class HenController : MonoBehaviour {

    public float extraTimeChick = 20f;
    public float extraTimeChicken = 30f;
    public float extraTimeRunner = 40f;


    float extraTime;

    CatcherController catcherController;

    GameController gameController;

    AddPointEffect addPointEffect;

    void Start() {
        catcherController = GameObject.FindGameObjectWithTag("Catcher").GetComponent<CatcherController>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponentInChildren<GameController>();
        addPointEffect = GetComponent<AddPointEffect>();
    }

    void OnTriggerEnter(Collider other) {

        if (other.tag == "Bird") {
            if (!gameController.lost) {
                gameController.birds.Remove(other.gameObject);
                Destroy(other.gameObject);
                catcherController.ResetState();
                ChickenMovement.BirdType bird = other.gameObject.GetComponent<ChickenMovement>().birdType;
                if (bird == ChickenMovement.BirdType.Chick) {
                    extraTime = extraTimeChick;
                } else if (bird == ChickenMovement.BirdType.Chicken) {
                    extraTime = extraTimeChicken;
                } else if (bird == ChickenMovement.BirdType.Runner) {
                    extraTime = extraTimeRunner;
                }
                gameController.AddPoint(bird, extraTime);
                addPointEffect.Run();
                if (gameController.birds.Count == 0) {
                    Debug.Log("Level Complete");
                    gameController.LevelComplete();
                } 
            }
        }
    }
}
