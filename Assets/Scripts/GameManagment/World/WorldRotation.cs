using UnityEngine;

public class WorldRotation : MonoBehaviour {

    public float speed = 3f;

	void Update () {

        // Planet rotation depending of the speed
        transform.Rotate(0, Time.deltaTime * speed, 0);
	}
}
