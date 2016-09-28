using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ChickenMovement))]
public class DustCloudParticle : MonoBehaviour {

    public GameObject dustCloud;

    ChickenMovement runnerMovement;

    void Start () {

        dustCloud.SetActive(false);
        runnerMovement = GetComponent<ChickenMovement>();
    }
	
	void Update () {

        if (runnerMovement.speed == 0)
            dustCloud.SetActive(false);
        else
            dustCloud.SetActive(true);
    }
}
