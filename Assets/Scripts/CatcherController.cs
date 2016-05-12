using UnityEngine;
using System.Collections;

public class CatcherController : MonoBehaviour {

    public float rotateSmooth = 10;

    private bool catching = false;
    private bool catched = false;

    Quaternion initialRot;
    Quaternion initialParentRot;
    GravityAttraction planetGravity;

    // Use this for initialization
    void Start () {
        initialRot = transform.rotation;
        initialParentRot = transform.parent.rotation;

        planetGravity = GameObject.FindGameObjectWithTag("Planet").GetComponent<GravityAttraction>();
    }
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetButtonDown("Fire") && !catching && !catched) {
            catching = true;
        }
        
        if  (catching ) {
            Debug.Log("Moving");
            transform.Rotate(0,-1f * rotateSmooth, 0);

           /* Vector3 targetDir = (transform.position - transform.forward).normalized;
            Quaternion targetRotation = (this.transform.Rotate(0, 0, 90));

            transform.rotation = Quaternion.FromToRotation(transform.right, targetDir) * transform.rotation;*/
        }

    }

    void OnTriggerEnter(Collider other) {

        if(other.tag == "Bird") {
            Debug.Log("Catched");
            catching = false;
            catched = true;
            transform.Rotate(0, 3f * rotateSmooth, 180f);
            transform.position = transform.position  + (transform.parent.up * 0.4f);

            
            other.transform.position = transform.position + transform.forward * 2.3f;
            other.transform.parent = this.transform;
            other.GetComponent<ChickenMovement>().catched = true;
        }
        if(other.tag == "Planet" && !catched) {
            Debug.Log("Planet");
            catching = false;
            ResetPosition();
            //transform.rotation = initialRot;
            //transform.Rotate(transform.parent.rotation);
            //transform.Rotate(0, 90f * rotateSmooth, 0);
        }

    }



    void OnTriggerExit(Collider other) {

        if (other.tag == "Bird") {
            Debug.Log("Escaped");

            ResetAndFlipPosition();
            //ResetPosition();

            //other.transform.position = planetGravity.GetGround(other.transform) * 1.02f;
            other.GetComponent<ChickenMovement>().catched = false;
            other.transform.parent = null;
            //other.GetComponent<ChickenMovement>().transform.position = other.transform.forward * 0.5f;
            //other.GetComponent<ChickenMovement>().movingState = ChickenMovement.MovingState.Run;

        }
    }


    public void ResetAndFlipPosition() {
        transform.position = transform.position - (transform.parent.up * 0.4f);
        transform.Rotate(0, 0, 180f);
        ResetPosition();
    }


    public void ResetPosition() {

        catching = false;
        catched = false;
        //transform.rotation = Quaternion.FromToRotation(transform.position, transform.up) * transform.rotation;
        transform.Rotate(0, 90f, 0f);

    }
}
