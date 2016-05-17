using UnityEngine;
using System.Collections;

public class DataController : MonoBehaviour {

    public float score = 00000000;
    public int level = 0;
    public int maxLevel = 1;
    public string playerName = "Anonimous";

    void Awake() {
        DontDestroyOnLoad(transform.gameObject);
    }

    public bool IsLastLevel() {
        return level == maxLevel;
    }

}
