using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScriptWoo2 : MonoBehaviour
{
    public int dir = 1;
    public CubeScriptWoo cs;
    private void Update()
    {
        dir = cs.dir;

        transform.position += new Vector3(dir * 0.1f, 0, 0);
    }
}
