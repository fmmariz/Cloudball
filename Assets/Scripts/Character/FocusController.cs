using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusController : MonoBehaviour
{
    private bool _focus = false;
    private float _angle = 0;
    private float _time = 0;

    [SerializeField] public GameObject _focusBall;
    [SerializeField] public GameObject _focusReticule1;
    [SerializeField] public GameObject _focusReticule2;

    private SpriteRenderer _sr0;
    private SpriteRenderer _sr1;
    private SpriteRenderer _sr2;

    private float _alpha = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _sr0 = _focusBall.GetComponent<SpriteRenderer>();
        _sr1 = _focusReticule1.GetComponent<SpriteRenderer>();
        _sr2 = _focusReticule2.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Transparency();
        _alpha -= 0.1f;
        if (_alpha < 0f) _alpha = 0f;
        if (!_focus) return;
        _angle += 5f;
        _time += Time.deltaTime;
        _alpha += 0.2f;
        if (_alpha >= 0.7) _alpha = 0.7f;
        SpinFocus(_angle);
    }

    public void ToggleFocus(bool focus)
    {
        _focus = focus;
        _time = 0;
    }

    public void SpinFocus(float angle)
    {
        _focusReticule1.transform.localEulerAngles += Quaternion.Euler(0,0,angle) * new Vector3(0, 0, 1);
        _focusReticule2.transform.localEulerAngles += Quaternion.Euler(0,0,angle) * new Vector3(0, 0, -1);
    }

    public void Transparency()
    {
        _sr1.color = new Color(1, 1, 1, _alpha);
        _sr2.color = new Color(1, 1, 1, _alpha);
        _sr0.color = new Color(1, 1, 1, _alpha);

    }


}
