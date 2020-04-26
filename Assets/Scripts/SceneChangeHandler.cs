using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeHandler : MonoBehaviour
{
    public int sceneIndex;
    public bool previousSceneCheck=false;
    public Transform spawnLoc;
    public GameObject player;

    private void Start()
    {
        if(PlayerPrefs.HasKey("LastScene")&&previousSceneCheck)
        {
            if(PlayerPrefs.GetInt("LastScene") == sceneIndex)
            {
                player.transform.position = spawnLoc.transform.position;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerPrefs.SetInt("LastScene", SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
    }
}
