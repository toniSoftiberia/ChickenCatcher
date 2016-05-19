using UnityEngine;
using System.Collections;

public class SetCountdown : MonoBehaviour {

    private GameController gameController;

    public void SetCountdownNow() {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        gameController.StartGame();

    }
}
