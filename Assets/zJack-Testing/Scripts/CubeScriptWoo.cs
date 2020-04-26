using UnityEngine;

public class CubeScriptWoo : MonoBehaviour
{
    float ticks = 0;
    public int dir = 1;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) Debug.Break();

        transform.position += new Vector3(dir*0.1f, 0, 0);
        ticks += 1*Time.deltaTime*20;
        if (ticks > 30)
        {
            dir *= -1;
            ticks = 0;
        }
    }
}
