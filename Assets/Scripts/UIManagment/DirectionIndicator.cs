using UnityEngine;

public class DirectionIndicator : MonoBehaviour {

    public float nearDistance = 5f;
    public bool closest = false;

    public ChickenMovement.BirdType birdType;

    new MeshRenderer renderer;
    GameController gameController;
    

    // Use this for initialization
    void OnEnable() {
        renderer = GetComponentInChildren<MeshRenderer>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update() {
        
        renderer.enabled = false;

        if (closest) {
            if ((Camera.main.transform.position - transform.position).magnitude > nearDistance) {
                if (renderer.IsVisibleFrom(Camera.main)) {
                    renderer.enabled = true;
                } else {
                    ShowIndicator();
                }
            }
        }
    }

    void ShowIndicator() {
        Vector3 relativePosition = Camera.main.transform.InverseTransformPoint(transform.position);
            
        if (relativePosition.z > 0) {
        } else {
            gameController.ShowIndicatorBottom(birdType);
        }

        if (relativePosition.x > 0) {
            gameController.ShowIndicatorRight(birdType);
        } else {
            gameController.ShowIndicatorLeft(birdType);
        }
    }



}
