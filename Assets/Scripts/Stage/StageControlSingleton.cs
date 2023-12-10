using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageControlSingleton : MonoBehaviour
{
    public static StageControlSingleton instance;


    private int stageN = 0;
    private void Awake()
    {
        instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    public void OpenStage1()
    {
        SetStage(1);
        SceneManager.LoadScene("MainGameScene");
    }

    public void OpenStage2()
    {
        SetStage(2);
        SceneManager.LoadScene("MainGameScene");

    }
    public void OpenStage3()
    {
        SetStage(3);
        SceneManager.LoadScene("MainGameScene");
    }

    public void SetStage(int i)
    {
        stageN = i;
    }

    public int GetStage()
    {
        return stageN;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
