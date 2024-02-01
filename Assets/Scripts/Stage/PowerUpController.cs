using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PowerUpManager;

public class PowerUpController : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve curve;

    private float spinTime = 0.5f;
    private float _time;
    private float _downspeed;

    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private PowerUpType _type;
    private Sprite _sprite;
    private float _intensity;
    private float _spunTime;
    private float _leftThreshold;
    private float _rightThreshold;
    private float _bottomThreshold;

    private float _decay = -999f;
    [SerializeField] private float timeToDecay;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = Vector2.up * 3f *_intensity 
            + (Vector2.left * Random.Range(-3, 3) *_intensity);
        _sr = GetComponent<SpriteRenderer>();
        if(_sprite != null)
        {
            _sr.sprite = _sprite;
        }
        _spunTime = spinTime;

        _leftThreshold = GameController.Instance.GetLeftThreshold();
        _rightThreshold = GameController.Instance.GetRightThreshold();
        _bottomThreshold = GameController.Instance.GetBottomThreshold();
    }

    void FixedUpdate()
    {
        _downspeed += 0.05f;
        transform.rotation = Quaternion.Euler(0, 0, 0);

        if (_time >= 0)
        {
            if (_time < spinTime)
            {
                transform.rotation = Quaternion.Euler(0, 0, (_time-_spunTime) * 720f);
            }
            _time += Time.deltaTime;
        }

        if (transform.position.x >= _rightThreshold || transform.position.x <= _leftThreshold)
        {
            Vector2 newVelocity = _rb.velocity;
            newVelocity.x = -newVelocity.x;
            _rb.velocity = newVelocity;
        }



        if (_downspeed >= 4f) { _downspeed =4f; }
        if(_rb.velocity.y < 0)
        {
            Vector2 disableHorizontalMovement = new Vector2(0, _rb.velocity.y);
            _rb.velocity = disableHorizontalMovement;
        }
        _rb.velocity += (Vector2.down)*5*Time.deltaTime;
        if(_rb.velocity.y < -2)
        {
            Vector2 vector = _rb.velocity;
            vector.y = -2;
            _rb.velocity = vector;
        }
        if (Vector2.Distance(transform.position, GameController.Instance.GetPlayerPosition()) < 3f)
        {
            _rb.velocity += ((GameController.Instance.GetPlayerPosition() - _rb.position).normalized);
        }

        if(_rb.position.y <= _bottomThreshold)
        {
            if(_decay < 0) { _decay = 0; }
            Vector2 newPosition = _rb.position;
            newPosition.y = _bottomThreshold;
            _rb.position = newPosition;
        }

        if(_decay >= 0)
        {
            StartCoroutine(KillAfter(timeToDecay));
            StartCoroutine(TimingOut());
        }

    }
    IEnumerator KillAfter(float timeToDecay)
    {
        yield return new WaitForSeconds(timeToDecay);
        Destroy(gameObject);
    }

    IEnumerator TimingOut()
    {
        float delay = 2f;
        while(gameObject != null)
        {
            _sr.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(delay);
            _sr.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(0.3f);

        }
    }

    public void UpdateAlpha(Color color)
    {
        _sr.color = color;
    }

    public void InitializePowerUp(PowerUpManager.PowerUpType powerUpType, Sprite selectedSprite, float intensity)
    {
        _type = powerUpType;
        _sprite = selectedSprite;
        _intensity = intensity;
        if (_sr != null)
        {
            _sr.sprite = selectedSprite;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("paddle"))
        {
            GameObject.Destroy(gameObject);
            ApplyPowerUp();
        }
    }

    private void ApplyPowerUp()
    {
        switch (_type)
        {
            case PowerUpType.WIDEPADDLE:
                GameController.Instance.
                    player.ActivateMultipaddle(
                    GameController.Instance.
                    powerUpManager.multipaddleDuration);
                GameController.Instance.soundManager.PlaySoundEffect("powerup");
                break;
            case PowerUpType.LIFE:
                GameController.Instance.soundManager.PlaySoundEffect("powerup");

                break;
            case PowerUpType.SCORE:
                GameController.Instance.scoreManager.IncreaseScore(2);
                GameController.Instance.soundManager.PlaySoundEffect("score");
                break;
            case PowerUpType.MOREBALL:
                GameController.Instance.
                    powerUpManager.SpawnExtraBall(transform.position);
                GameController.Instance.soundManager.PlaySoundEffect("powerup");
                break;
            case PowerUpType.LASER:
                GameController.Instance.
                    player.ActivateLaser(
                    GameController.Instance.
                    powerUpManager.laserDuration);
                GameController.Instance.soundManager.PlaySoundEffect("powerup");
                break;
            case PowerUpType.SLOWDOWN:
                GameController.Instance.soundManager.PlaySoundEffect("powerup");
                break;

        }
    }

}
