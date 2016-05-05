using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
public class GravityBody : MonoBehaviour {

    GravityAttraction planet;

	void Awake () {
        planet = GameObject.FindGameObjectWithTag("Planet").GetComponent<GravityAttraction>();
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate() {
        planet.Attract(transform);
    }
}
