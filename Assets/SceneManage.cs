using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneManage : MonoBehaviour
{

    public void MoveLevel()
    {
        SceneManager.LoadScene(""); // ·¹º§ ¾À Ãß°¡ ÈÄ º¯°æ ÇÇ·æ
    }

    public void MoveMap1()
    {
        SceneManager.LoadScene("Map1");
    }

    public void MoveMain1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void MoveMini2()
    {
        SceneManager.LoadScene("MiniGame2");
    }


    public void MoveMap2()
    {
        SceneManager.LoadScene("Map2");
    }


    public void MoveMain2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void MoveMap3()
    {
        SceneManager.LoadScene("Map3");
    }

    public void MoveMain3()
    {
        SceneManager.LoadScene("Level3");
    }


    public void MoveClear()
    {
        SceneManager.LoadScene("Clear");
    }


    public void NextScene(string str)
    {
        SceneManager.LoadScene(str);
    }
}
