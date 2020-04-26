using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graphics : MonoBehaviour
{
    public void LookAtCamera()
    {
        transform.eulerAngles = new Vector3(Camera.main.transform.eulerAngles.x, 0, 0);
    }   
}
