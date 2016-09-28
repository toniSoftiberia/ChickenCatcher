using UnityEngine;
using System.Collections;

public class ChickenMovement : MonoBehaviour {

    #region Enums
    /// <summary>
    /// Enum to handle the moving states
    /// </summary>
    public enum MovingState {
        Idle,
        Walk,
        Run
    }

    /// <summary>
    /// Enum to handle the bird type
    /// </summary>
    public enum BirdType {
        none = -1,
        Chick,
        Chicken,
        Runner
    }
    #endregion

    #region Public Attributes
    /// <summary>
    /// Mask to collision
    /// </summary>
    public LayerMask avoidMask;

    /// <summary>
    /// Type of the bird
    /// </summary>
    public BirdType birdType;

    /// <summary>
    /// Speed while walking
    /// </summary>
    public float walkSpeed = 1;

    /// <summary>
    /// Speed while running
    /// </summary>
    public float runSpeed = 4;

    /// <summary>
    /// Acceleration of the bird
    /// </summary>
    public float acceleration = 30;

    /// <summary>
    /// Handles the moving state
    /// </summary>
    public MovingState movingState = MovingState.Idle;

    /// <summary>
    /// Distance for seek
    /// </summary>
    public float seekDistance = 2;

    /// <summary>
    /// Speed while rotating to change direction
    /// </summary>
    public float rotateSpeed = 10f;

    /// <summary>
    /// Force when jumping
    /// </summary>
    public float jumpForce = 10;

    /// <summary>
    /// Minimum time to change state
    /// </summary>
    public float minState = 1f;

    /// <summary>
    /// Maximum time to change state
    /// </summary>
    public float maxState = 5f;

    /// <summary>
    /// Handles if the bird is cathced
    /// </summary>
    public bool catched = false;

    /// <summary>
    /// Sound to play when idle
    /// </summary>
    public AudioSource idleAudio;

    /// <summary>
    /// Sound to play when running away
    /// </summary>
    public AudioSource runAwayAudio;

    #endregion

    #region Private Attributes

    /// <summary>
    /// Stores the reference to the animator
    /// </summary>
    Animator animator;

    /// <summary>
    /// Speed of the animations
    /// </summary>
    float animationSpeed;

    /// <summary>
    /// Stores the reference to the rigidbody
    /// </summary>
    Rigidbody rb;

    /// <summary>
    /// Clamps the movement
    /// </summary>
    Vector3 moveAmount;

    /// <summary>
    /// Used to modify the velocity
    /// </summary>
    Vector3 smoothMoveVelocity;

    /// <summary>
    /// Reference to some raycast needed an the raycast hit
    /// </summary>
    Ray ray;
    Ray rayRight;
    Ray rayCenterRight;
    Ray rayLeft;
    Ray rayCenterLeft;
    RaycastHit hit;

    /// <summary>
    /// Intern state timer to switch between states
    /// </summary>
    float stateTimer;

    /// <summary>
    /// Current speed of the bird
    /// </summary>
    [HideInInspector]
    public float speed = 0;

    /// <summary>
    /// Rotation of the bird
    /// </summary>
    int rotate = 0;

    /// <summary>
    /// Direction to rotate
    /// </summary>
    int rotateDir;

    #endregion

    #region Public Methods
    void Start () {

        // Get the references
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // Choose a random idle animation and set it
        int idleAnimId = Random.Range(0, 3);
        animator.SetFloat("IdleHandler", idleAnimId);

        // Set random state duration
        stateTimer = Random.Range(minState, maxState);
    }
	

	void Update () {

        stateTimer -= Time.deltaTime;

        // Handle states
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
                idleAudio.Play();
            }
        } else if (movingState == MovingState.Run) {
            speed = runSpeed;
            if (stateTimer < 0) {
                animator.SetFloat("IdleHandler", Random.Range(0, 3));
                stateTimer = Random.Range(minState, maxState);
                movingState = MovingState.Idle;
                idleAudio.Play();
                runAwayAudio.Stop();
                speed = 0;
            }
        }     

        // Animate it
        animator.SetFloat("Speed", speed);

    }

    /// <summary>
    /// Checks if the bird is seeing you
    /// </summary>
    /// <returns>if bird has seen you</returns>
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
                runAwayAudio.Play();
            }


        }else {
            idleAudio.Stop();
            runAwayAudio.Stop();
        }

        return collisionSide == 0;
    }
    #endregion

    #region Private Methods
    void FixedUpdate() {

        /// Move the bird
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
    #endregion
}
