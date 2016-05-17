using UnityEngine;
using System.Collections;

public class GravityAttraction : MonoBehaviour {

    public float gravity = 10f;
    public LayerMask collisionMask;

    Ray ray;
    RaycastHit hit;

    public void Attract(Transform body) {
        Vector3 targetDir = (body.position - transform.position).normalized;
        Vector3 bodyUp = body.up;

        body.rotation = Quaternion.FromToRotation(bodyUp, targetDir) * body.rotation;
        body.GetComponent<Rigidbody>().AddForce(targetDir * -gravity);
    }

    public Vector3 GetGround(Transform body) {
        Vector3 res = Vector3.zero;


        Vector3 targetDir = (body.position - transform.position).normalized;

        if (Physics.Raycast(ray, out hit, targetDir.sqrMagnitude, collisionMask)) {
            res = hit.point + (targetDir) ;
        }

        return res;
    }

}
