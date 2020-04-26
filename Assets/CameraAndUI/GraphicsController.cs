using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsController : MonoBehaviour
{
    public void UpdateGraphicsRotation()
    {
        Graphics[] graphics = FindObjectsOfType<Graphics>();

        foreach (var gr in graphics)
        {
            gr.LookAtCamera();
        }
    }
}
