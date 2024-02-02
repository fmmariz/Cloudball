using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Sprite _fullHp;
    [SerializeField]
    private Sprite _damaged;
    [SerializeField]
    private Sprite _lowHp;
    private int _level;
    private int _hitsToDestroy;
    private SpriteRenderer _sr;
    private Rigidbody2D _rb;
    private Color _objectColor;
    private PowerUpManager.PowerUpType? _powerUpWithin;
    
    [SerializeField] private AnimationCurve _bounceAnimationCurve;
    

    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _sr.sprite = _fullHp;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (_objectColor != null)
        {
            _sr.color = _objectColor;
        }

    }

    // Update is called once per frame
    void Update()
    {
        damageByLaserCountdown -= Time.deltaTime;
    }

    public void SetPowerUpDrop(PowerUpManager.PowerUpType powerUpType)
    {
        _powerUpWithin = powerUpType;
    }

    public void SetCloudLevel(int level)
    {
        _hitsToDestroy = level + 1;
        _level = level;
        _objectColor = BlockLevelColors.Colors[level];
        if (_sr != null)
        {
            _sr.color = _objectColor;
        }
    }

    public void DealDamage(int damage, Vector3 directionHit, Vector3 speed)
    {
        _hitsToDestroy -= damage;
        StartCoroutine(Bounce(-directionHit, speed.magnitude, 100));

        if(_hitsToDestroy == 1)
        {
            _sr.sprite = _lowHp;
        }
        else
        {
            _sr.sprite = _damaged;
        }

        if (_hitsToDestroy <= 0)
        {
            if (_powerUpWithin != null)
            {
                GameController.Instance.powerUpManager.
                    SpawnPowerUp(transform.position, (PowerUpManager.PowerUpType)_powerUpWithin, 2);
            }
            GameController.Instance.powerUpManager.
                SpawnPowerUp(transform.position, PowerUpManager.PowerUpType.SCORE,1);
            GameController.Instance.powerUpManager.
                SpawnPowerUp(transform.position, PowerUpManager.PowerUpType.SCORE,1);

            GameObject.Destroy(gameObject);
        }
        else
        {
            _sr.color = BlockLevelColors.Colors[_hitsToDestroy - 1];
        }
    }

    IEnumerator Bounce(Vector3 direction, float intensity,  float totalBounceTime)
    {
        Vector3 originalPosition = transform.position;
        for (float time = 0f; time <= 1; time += 1/totalBounceTime)
        {
            Vector3 movement = _bounceAnimationCurve.Evaluate(time) * (intensity * 0.1f) * direction.normalized;
            transform.position = originalPosition +movement ;
            yield return null;
        }
    }

    private float damageByLaserDelay = 0.5f;
    private float damageByLaserCountdown = 0f;
    public void DamageByLaser()
    {
        if(damageByLaserCountdown <= 0f)
        {
            damageByLaserCountdown = damageByLaserDelay;
            DealDamage(1, Vector3.up, new Vector3(5f, 0f, 0f));
        }
    }
}
