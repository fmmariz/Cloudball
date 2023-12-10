using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
{

    [SerializeField]
    private float paddleSpeed;
    [SerializeField]
    private GameObject leftThresholdObject;
    [SerializeField]
    private GameObject rightThresholdObject;


    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private float _leftThreshold;
    private float _rightThreshold;
    private Animator _animator;
    private bool _focus;




    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        _listeners = new List<PaddleCopyController>();

        float extra = ((_sr.bounds.max.x - _sr.bounds.min.x) / 2) * 0.8f;
        _leftThreshold = GameController.Instance.GetLeftThreshold() + extra;
        _rightThreshold = GameController.Instance.GetRightThreshold() - extra;
    }

    void FixedUpdate()
    {


        GetComponent<FocusController>().ToggleFocus(false);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _focus = true;
            GetComponent<FocusController>().ToggleFocus(true);
        }

        Move();

        if (_focus)
        {
            _sr.color = new Color(0.6f, 0.6f, 0.6f);
        }
        else
        {
            _sr.color = new Color(1f, 1f, 1f);
        }




        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            _animator.SetBool("isCrouhcing", false);
            foreach (var listener in _listeners)
            {
                listener.SetAction((animator) =>
                {
                    animator.SetBool("isCrouhcing", false);
                });
            }
        }

        _focus = false;


    }

    private void Update()
    {

        #region Power Up Updates


        if (_multipaddleDuration > 0 && !multiFox.activeSelf)
        {
            multiFox.SetActive(true);
        }
        if (_multipaddleDuration < 0 && multiFox.activeSelf)
        {
            multiFox.SetActive(false);
        }


        if (_laserDuration > 0 && !laser.activeSelf)
        {
            laser.SetActive(true);
            if (laser.TryGetComponent<LaserController>(out LaserController c))
            {
                c.ActivateLaser(60f);
            }
        }
        if (_laserDuration < 0)
        {
            if (laser.TryGetComponent<LaserController>(out LaserController c))
            {
                c.DeactivateLaser(60f);
            }
        }

        _multipaddleDuration -= Time.deltaTime;
        _laserDuration -= Time.deltaTime;

        #endregion
    }

    private float _stepSoundDelay = 0.5f;
    private float _stepSoundCooldown = 0f;

    private void Move()
    {
        if(_stepSoundCooldown <= 0)
        {
            _stepSoundCooldown = _stepSoundDelay;
            GameController.Instance.soundManager.PlaySoundEffect("step");
        }
        float direction = Input.GetAxisRaw("Horizontal");

        if (direction < 0)
        {
            _animator.SetBool((_focus ? "walking" : "running"), true);
            _sr.flipX = true;
            foreach (var listener in _listeners)
            {
                listener.Side(true);
                listener.SetAction((animator) =>
                {
                    animator.SetBool((_focus ? "walking" : "running"), true);
                });
            }
            _stepSoundCooldown -= _focus ? Time.deltaTime : Time.deltaTime * 2;

        }
        else if (direction > 0)
        {
            _stepSoundCooldown -= _focus ? Time.deltaTime : Time.deltaTime * 2;

            _animator.SetBool((_focus ? "walking" : "running"), true);
            _sr.flipX = false;

            foreach (var listener in _listeners)
            {
                listener.Side(false);
                listener.SetAction((animator) =>
                {
                    animator.SetBool((_focus ? "walking" : "running"), true);
                });
            }
        }

        float paddleFinalSpeed = paddleSpeed * (_focus ? 1 : 2);
        float newX = Mathf.Clamp(_rb.position.x + direction * paddleFinalSpeed * Time.fixedDeltaTime, _leftThreshold, _rightThreshold);
        _rb.MovePosition(new Vector2(newX, _rb.position.y));

        if (direction == 0)
        {
            _animator.SetBool("walking", false);
            _animator.SetBool("running", false);
            foreach (var listener in _listeners)
            {
                listener.SetAction((animator) =>
                {
                    animator.SetBool("walking", false);
                    animator.SetBool("running", false);
                });
            }
            _rb.velocity = Vector2.zero;
        }
    }



    #region PowerUps
    [Header("Power Ups Settings")]

    #region Laser
    private float _laserDuration;
    [SerializeField] GameObject laser;
    public void ActivateLaser(float laserDuration)
    {
        _laserDuration = laserDuration;
    }

    public void ForceStopLaser()
    {
        _laserDuration = 0;
    }

    #endregion

    #region MultiPaddle

    private float _multipaddleDuration;
    private List<PaddleCopyController> _listeners;
    [SerializeField] GameObject multiFox;

    public void ActivateMultipaddle(float multipaddleDuration)
    {
        _multipaddleDuration = multipaddleDuration;
    }

    public void ForceStopMultipaddle()
    {
        _multipaddleDuration = 0;
    }

    public void AddListener(PaddleCopyController paddleCopyController)
    {
        _listeners.Add(paddleCopyController);
    }

    public void RemoveListener(PaddleCopyController paddleCopyController)
    {
        _listeners.Remove(paddleCopyController);
    }

    public void FlushListeners()
    {
        _listeners.Clear();
    }
    #endregion

    #endregion

}
