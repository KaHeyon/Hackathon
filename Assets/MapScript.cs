using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("map1");
    }

    public void NextGame()
    {
        SceneManager.LoadScene("map2");
    }

    public void OnMouseDown()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log(sceneName);
        if (sceneName == "start")
        {
            SceneManager.LoadScene("map1");
        }
        if (sceneName == "map1")
        {
            SceneManager.LoadScene("Level1");
        }
        if (sceneName == "map2")
        {
            SceneManager.LoadScene("Level2");
        }
        if (sceneName == "map3")
        {
            SceneManager.LoadScene("Level3");
        }
    }
}
