using UnityEngine;
using System.Collections;

public class DataController : MonoBehaviour {

    #region Public attributes
    /// <summary>
    /// total score
    /// </summary>
    public float score = 00000000;

    /// <summary>
    /// Actual level
    /// </summary>
    public int level = 0;

    /// <summary>
    /// Max level
    /// </summary>
    public int maxLevel = 1;

    /// <summary>
    /// Ddefault player name
    /// </summary>
    public string playerName = "Anonimous";

    #endregion

    #region Public Methods
    void Awake() {
        // Keep this object alive
        DontDestroyOnLoad(transform.gameObject);
    }

    /// <summary>
    /// Checks if this level is the lastlevel
    /// </summary>
    /// <returns>if this level is the lastlevel</returns>
    public bool IsLastLevel() {
        return level == maxLevel;
    }
    #endregion

}
