using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int _score;
    void Start()
    {
        _score = 0;
    }

    public void IncreaseScore(int score)
    {
        _score += score;
        GameController.Instance.uiManager.UpdateScoreNumber(_score);
    }

    public int GetScore() { return _score; }
}
