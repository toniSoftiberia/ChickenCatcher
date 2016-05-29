using UnityEngine;
using System.Collections;

public class CatcherRotation : MonoBehaviour {

    public Transform player;
    public Transform head;
    public Transform childs;

    Quaternion startRotation;
    Quaternion lastRotation;

    // Use this for initialization
    void Start () {
        startRotation = transform.localRotation;
        lastRotation = transform.localRotation;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        /*
        var angle = Quaternion.Angle( lastRotation, transform.localRotation);

        if (lastRotation.y > transform.localRotation.y )
            angle *= -1;
        childs.transform.RotateAround(player.position, Vector3.up, angle);

        lastRotation = transform.localRotation;*/

        childs.localRotation = new Quaternion(childs.localRotation.x, head.localRotation.y, childs.localRotation.z, head.localRotation.w);
    }
}
