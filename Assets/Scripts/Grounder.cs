using UnityEngine;
using System.Collections;

public class Grounder : MonoBehaviour {

    GravityAttraction planet;

    public LayerMask collisionMask;

    Ray ray;
    RaycastHit hit;

    // Use this for initialization
    void Start () {
        planet = GameObject.FindGameObjectWithTag("Planet").GetComponent<GravityAttraction>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 res = transform.position;


        Vector3 targetDir = (transform.position - planet.transform.position).normalized;

        ray = new Ray(transform.position, planet.transform.position);

        Debug.DrawRay(ray.origin, ray.direction * targetDir.sqrMagnitude, Color.black);

        if (Physics.Raycast(ray, out hit, targetDir.sqrMagnitude, collisionMask)) {
            res = hit.point + (targetDir);
        }

        transform.position = res;
        //transform.position = planet.GetGround(transform.position + transform.up);

    }
}
