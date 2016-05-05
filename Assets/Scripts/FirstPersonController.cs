using UnityEngine;
using System.Collections;

public class FirstPersonController : MonoBehaviour {

    public float mouseSensitivityX = 250f;
    public float mouseSensitivityY = 250f;
    public float verticalLookAngle = 60f;
    public float walkSpeed = 4;
    public float jumpForce = 220;
    public LayerMask groundedMask;

    Transform cameraT;
    float verticalLookRotation;

    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;
    Rigidbody rb;

    bool grounded;

    // Use this for initialization
    void Start () {
        cameraT = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivityX);
        verticalLookRotation += Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivityX;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -verticalLookAngle, verticalLookAngle);
        cameraT.localEulerAngles = Vector3.left * verticalLookRotation;

        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        Vector3 targetMoveAmount = moveDir * walkSpeed;
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);

        if (Input.GetButtonDown("Jump") && grounded) {

            rb.AddForce(transform.up * jumpForce);
        }

        grounded = false;
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;
        float distance = GetComponent<CapsuleCollider>().height / 2 + 0.1f;
        if (Physics.Raycast(ray, out hit, distance, groundedMask)) {
            grounded = true;
        }
    }

    void FixedUpdate() {
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.deltaTime);
    }
}
