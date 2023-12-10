using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedBallScript : MonoBehaviour
{

    private float _time;
    void Start()
    {
    }

    void Update()
    {
        if(_time <= 0)
        {
            Destroy(this.gameObject);
        }
        _time -= Time.deltaTime;
    }

    public void SetTimer(float timer)
    {
        _time = timer;
    }
}
