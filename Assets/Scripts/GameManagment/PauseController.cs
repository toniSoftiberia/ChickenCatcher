using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseController : MonoBehaviour {

    public Text gamePausedText;
    public Button MainMenuButton;

    public AudioSource pauseAudio;

    CatcherController catcherController;
    GameController gameController;
    FirstPersonController playerController;
    CursorController cursorController;

    // Game state
    bool paused = false;

    void Start() {
        catcherController = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<CatcherController>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
        gameController = GetComponent<GameController>();
        cursorController = GetComponent<CursorController>();
    }

    void Update () {
	    if (Input.GetButtonDown("Escape")) {
            if (paused)
                UnpauseGame();
            else
                PauseGame();
        }
	}

    void PauseGame() {
        if (gameController.gameStated && !gameController.gameOver) { 
            cursorController.EnableCursor();
            gameController.StopBirds();
            catcherController.enabled = false;
            playerController.enabled = false;
            gameController.enabled = false;
            gamePausedText.gameObject.SetActive(true);
            MainMenuButton.gameObject.SetActive(true);
            pauseAudio.Play();
            paused = !paused;
        }
    }

    void UnpauseGame() {
        catcherController.enabled = true;
        playerController.enabled = true;
        gameController.enabled = true;
        gamePausedText.gameObject.SetActive(false);
        MainMenuButton.gameObject.SetActive(false);
        gameController.FreeBirds();
        cursorController.DisableCursor();
        pauseAudio.Play();
        paused = !paused;
    }

}
