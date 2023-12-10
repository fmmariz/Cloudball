using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaserController : MonoBehaviour
{

    [SerializeField]
    private float laserFlowSpeed;
    [SerializeField]
    private SpriteRenderer _laserSr;
    [SerializeField]
    private SpriteRenderer _baseSr;
    private Collider2D _collider;
    private float _offset;

    private float _widthPct = 0;
    private bool activated = false;

    // Start is called before the first frame update

    void Start()
    {
        _collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(0f+_widthPct, 1f, 1f);
        if (activated)    
        {
            _baseSr.transform.localScale = new Vector3(1, 1 + 0.2f * Mathf.Sin(_offset *5), 1);
            _baseSr.transform.localPosition = new Vector3(0, 1.06f + 0.05f * Mathf.Sin(_offset * 5), 1);
            _laserSr.material.mainTextureOffset = new Vector2(0, _offset);
            _offset -= laserFlowSpeed * Time.deltaTime;
        }
    }

    public void ActivateLaser(float dur)
    {
        if (!activated)
        {
            activated = true;
            StartCoroutine(ProcLaser());
            StartCoroutine(StartLaser(dur));
        }
    }

    private bool coolingDown = false;
    public void DeactivateLaser(float dur)
    {
        if (activated && !coolingDown)
        {
            coolingDown = true;
            StartCoroutine(StopLaser(dur));
        }
    }

    IEnumerator StartLaser(float windupTime)
    {
        _widthPct = 0.05f;
        yield return new WaitForSeconds(1f);
        for(float i = 0f; i <= 1f; i += 1f / windupTime)
        {
            _widthPct = Mathf.SmoothStep(0.05f, 1f, i);
            yield return null;
        }
    }

    IEnumerator StopLaser(float winddownTime)
    {
        for (float i = 0f; i < 1f; i += 1f / winddownTime)
        {
            _widthPct = 1f - Mathf.SmoothStep(0f, 0.99f, i);
            yield return null;
        }
        yield return new WaitForSecondsRealtime(2f);
        _widthPct = 0f;
        activated = false;
        gameObject.SetActive(false);
        coolingDown = false;
    }

    IEnumerator ProcLaser()
    {
        while (activated)
        {
            GameController.Instance.soundManager.PlaySoundEffect("laserLoop");
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("ball"))
        {
            if (collision.TryGetComponent<BallController>(out BallController bc))
            {
                bc.DamageByLaser();
            }
        }
        else if (collision.CompareTag("block"))
        {
            if (collision.TryGetComponent<BlockController>(out BlockController bc))
            {
                bc.DamageByLaser();
            }
        }
    }

}
