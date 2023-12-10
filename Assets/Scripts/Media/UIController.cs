using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI _scoreNumber;

    public void UpdateScoreNumber(int number)
    {
        _scoreNumber.text = number.ToString();
    }

    public void UpdateUmbrellaHitsNumber(int number)
    {
    }

}
