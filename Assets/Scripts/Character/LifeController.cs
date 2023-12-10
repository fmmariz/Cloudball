using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    private int _lives;
    public void DeductLife()
    {
        _lives--;
    }

    public int GetLives()
    {
        return _lives;
    }

    public void AddLife()
    {
        _lives++;
    }
}
