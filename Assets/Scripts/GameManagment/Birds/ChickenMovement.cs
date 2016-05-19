using UnityEngine;
using System.Collections;

public class ChickenMovement : MonoBehaviour {


    public enum MovingState {
        Idle,
        Walk,
        Run
    }

    public enum BirdType {
        none = -1,
        Chick,
        Chicken,
        Runner
    }

    public LayerMask avoidMask;

    public BirdType birdType;

    private Animator animator;
    private float animationSpeed;
    public float walkSpeed = 1;
    public float runSpeed = 4;
    public float acceleration = 30;
    private float targetSpeed;
    public MovingState movingState = MovingState.Idle;
    public float seekDistance = 2;
    public float rotateSpeed = 10f;
    public float jumpForce = 10;

    public float minState = 1f;
    public float maxState = 5f;

    public bool catched = false;

    Rigidbody rb;

    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;

    Ray ray;
    Ray rayRight;
    Ray rayCenterRight;
    Ray rayLeft;
    Ray rayCenterLeft;
    RaycastHit hit;

    float stateTimer;

    [HideInInspector]
    public float speed = 0;

    int rotate = 0;

    int rotateDir;    

    void Start () {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        int idleAnimId = Random.Range(0, 3);
        animator.SetFloat("IdleHandler", idleAnimId);

        stateTimer = Random.Range(minState, maxState);
    }
	
	// Update is called once per frame
	void Update () {

        stateTimer -= Time.deltaTime;
        if (movingState == MovingState.Idle || catched) {
            speed = 0;
            if (stateTimer < 0) {
                animator.SetFloat("IdleHandler", Random.Range(0, 3));
                stateTimer = Random.Range(minState, maxState);
                if (catched) {
                    movingState = MovingState.Idle;
                    if(Random.Range(0, 100) > 80) {
                        // Escape
                        movingState = MovingState.Run;
                    }
                } else
                    movingState = MovingState.Walk;
                rotate = 10;
                rotateDir = Random.Range(-1, 1);
            }
        } else if (movingState == MovingState.Walk) {
            speed = walkSpeed;
            if (stateTimer < 0) {
                animator.SetFloat("IdleHandler", Random.Range(0, 3));
                stateTimer = Random.Range(minState, maxState);
                movingState = MovingState.Idle;
                speed = 0;
            }
        } else if (movingState == MovingState.Run) {
            speed = runSpeed;
            if (stateTimer < 0) {
                animator.SetFloat("IdleHandler", Random.Range(0, 3));
                stateTimer = Random.Range(minState, maxState);
                movingState = MovingState.Idle;
                speed = 0;
            }
        }     

        // Animate it
        animator.SetFloat("Speed", speed);

    }


    void FixedUpdate() {
            Vector3 targetMoveAmount = Vector3.forward * speed;
            if (CheckForward()) {

                if (rotate > 0) {
                    int randomRotation = Random.Range(15, 90) * rotateDir;
                    transform.RotateAround(transform.position, transform.up, Time.deltaTime * randomRotation);
                    --rotate;

                }
            }

            moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);

            rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.deltaTime);
    }

    public bool CheckForward() {

        int collisionSide = 0;

        if (!catched) {

            Vector3 origin = transform.position;
            origin.y += GetComponent<SphereCollider>().radius;
            Vector3 destinationCenter = (transform.forward * 2) * seekDistance;
            Vector3 destinationCenterRight = ((transform.forward * 2) + (transform.right / 2)) * seekDistance;
            Vector3 destinationRight = ((transform.forward * 2) + transform.right) * seekDistance;
            Vector3 destinationCenterLeft = ((transform.forward * 2) + (-transform.right / 2)) * seekDistance;
            Vector3 destinationLeft = ((transform.forward * 2) + -transform.right) * seekDistance;

            ray = new Ray(origin, destinationCenter);
            rayRight = new Ray(origin, destinationRight);
            rayCenterRight = new Ray(origin, destinationCenterRight);
            rayLeft = new Ray(origin, destinationLeft);
            rayCenterLeft = new Ray(origin, destinationCenterLeft);

            Debug.DrawRay(ray.origin, ray.direction * seekDistance, Color.yellow);
            Debug.DrawRay(rayRight.origin, rayRight.direction * seekDistance, Color.cyan);
            Debug.DrawRay(rayCenterRight.origin, rayCenterRight.direction * seekDistance, Color.cyan);
            Debug.DrawRay(rayLeft.origin, rayLeft.direction * seekDistance, Color.red);
            Debug.DrawRay(rayCenterLeft.origin, rayCenterLeft.direction * seekDistance, Color.red);
            int playerFront = 0;


            float rotationVelocity = Time.deltaTime * rotateSpeed * Mathf.Clamp(speed, 1, runSpeed);

            if (Physics.Raycast(rayLeft, out hit, seekDistance, avoidMask)) {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player")) {
                    Debug.Log("Player Left");
                    playerFront = 1;
                } else {
                    Debug.Log("Collision Left");
                    collisionSide = 1;
                }
            } else

            if (Physics.Raycast(rayCenterLeft, out hit, seekDistance, avoidMask)) {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player")) {
                    Debug.Log("Player Left");
                    playerFront = 1;
                } else {
                    Debug.Log("Collision Left");
                    collisionSide = 1;
                }
            } else

            if (Physics.Raycast(rayRight, out hit, seekDistance, avoidMask)) {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player")) {
                    Debug.Log("Player Right");
                    playerFront = 1;
                } else {
                    Debug.Log("Collision Right");
                    collisionSide = 1;
                }
            } else

            if (Physics.Raycast(rayCenterRight, out hit, seekDistance, avoidMask)) {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player")) {
                    Debug.Log("Player Right");
                    playerFront = 1;
                } else {
                    Debug.Log("Collision Right");
                    collisionSide = 1;
                }
            }

            if (Physics.Raycast(ray, out hit, seekDistance, avoidMask)) {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player")) {
                    Debug.Log("Player Center");
                    playerFront = 1;
                } else {
                    Debug.Log("Collision Center");
                    collisionSide = 1;
                }
            }
            if (collisionSide != 0) {
                transform.RotateAround(transform.position, transform.up, rotationVelocity * collisionSide);
                rotate = 0;
            }
            if (playerFront != 0) {
                //rb.AddForce(transform.up * jumpForce);
                transform.RotateAround(transform.position, transform.up, Random.Range(135, 225));
                stateTimer = Random.Range(minState, maxState);
                movingState = MovingState.Run;
            }


        }

        return collisionSide == 0;
    }
}
