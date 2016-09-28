using UnityEngine;
using System.Collections;

public class CatcherController : MonoBehaviour {

    public float rotateSmooth = 10;
    [HideInInspector]
    public GameObject chickensPool;

    private bool catching = false;
    [HideInInspector]
    public bool catched = false;

    public GameObject impactCloud;
    public float timeEffect = 0.25f;

    public AudioSource escapeAudio;

    public AudioSource movementAudio;
    public AudioSource impactAudio;

    float elapsedTimeEffect = 0;

    Quaternion initialRot;
    Quaternion initialParentRot;
    GameController gameController;
    GameObject henHouse;

    bool upped;

    BoxCollider trigger;


    // Use this for initialization
    void Start () {
        impactCloud.SetActive(false);

        initialRot = transform.rotation;
        initialParentRot = transform.parent.rotation;
        
        chickensPool = GameObject.FindGameObjectWithTag("Birds");
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        henHouse = GameObject.FindGameObjectWithTag("HenHouse");
        trigger = GetComponentInChildren<BoxCollider>();
    }
	
	// Update is called once per frame
	void Update () {
        if (gameController.gameStated)
            if (Input.GetButtonDown("Fire") && !catching && !catched) {
                catching = true;
                movementAudio.Play();
            }
        
        if  (catching )
            transform.Rotate(0,-1f * rotateSmooth, 0);

        if (catched) {
            ShowIndicator();
        }

        if (timeEffect > elapsedTimeEffect) {
            elapsedTimeEffect += Time.deltaTime;
            impactCloud.SetActive(true);
        } else {
            impactCloud.SetActive(false);
        }

    }

    void OnTriggerEnter(Collider other) {

        if(other.tag == "Bird" && catching && !catched) {
            BirdCatched();

            other.transform.position = transform.position + (transform.forward * 2.9f) + (other.transform.up * -0.7f);
            
            other.transform.parent = this.transform;
            other.GetComponent<ChickenMovement>().catched = true;
        }

        if ((other.tag == "Planet" || other.gameObject.layer == LayerMask.GetMask("Scene")) && catching && !catched) {
            catching = false;

            elapsedTimeEffect = 0;
            
            impactCloud.transform.position = trigger.center;

            impactAudio.Play();

            ResetPosition();
        }

    }
    

    void OnTriggerExit(Collider other) {

        if (catched && other.tag == "Bird") {
            Debug.Log("Escaped");

            escapeAudio.Play();

            ResetState();

            other.GetComponent<ChickenMovement>().catched = false;
            other.GetComponent<ChickenMovement>().movingState = ChickenMovement.MovingState.Run;
            other.transform.parent = chickensPool.transform;

        }
    }

    public void ResetState() {

        catching = false;
        catched = false;
        ResetPosition();
    }

    public void ResetPosition() {
        DownPosition();
        Quaternion actualParentRot = transform.parent.rotation;
        transform.parent.rotation = initialParentRot;
        transform.rotation = initialRot;
        transform.parent.rotation = actualParentRot;
    }

    void UpPosition() {
        if (!upped) {
            upped = true;
            transform.position = transform.position + (transform.parent.up);
        }
    }

    void DownPosition() {
        if (upped) {
            upped = false;
            transform.position = transform.position - (transform.parent.up);
        }
    }

    void BirdCatched() {
        catching = false;
        catched = true;

        transform.Rotate(0, 0, 180f);
        UpPosition();
    }



    void ShowIndicator() {
        Vector3 relativePosition = Camera.main.transform.InverseTransformPoint(henHouse.transform.position);

        string print = "";

        if (!henHouse.GetComponent<Renderer>().IsVisibleFrom(Camera.main)) {
            if (relativePosition.z > 0) {
            } else {
                gameController.ShowIndicatorBottom(ChickenMovement.BirdType.none);
            }

            if (relativePosition.x > 0) {
                gameController.ShowIndicatorRight(ChickenMovement.BirdType.none);
            } else {
                gameController.ShowIndicatorLeft(ChickenMovement.BirdType.none);
            }
        }

        Debug.Log(print);
    }
}
