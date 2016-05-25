using UnityEngine;
using System.Collections;

public class CursorController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DisableCursor();
    }

    public void EnableCursor() {
        Cursor.visible = true;
    }

    public void DisableCursor() {
        Cursor.visible = false;
    }
}
