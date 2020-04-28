using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCursor : MonoBehaviour
{
    [SerializeField]
    PlayerController PlayerController;

    private void Update() {
        Vector3 CursorPos = PlayerController.getPointerPos();
        transform.LookAt(new Vector3(CursorPos.x, 0.1f, CursorPos.z), Vector3.up);
        transform.Rotate(new Vector3(90, 0, 0));
    }
}
