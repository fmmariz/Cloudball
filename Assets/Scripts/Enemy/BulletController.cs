using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private float _speed;
    private float _acceleration = 0.1f;
    private float _maxSpeed = 5f;
    private float _size;
    private Vector3 _direction;
    private bool _isActive;
    private GameObject explosionAnimation;
    private bool _playerBullet;

    private Color _bulletColor;
    private float _bulletScale;

    private float _bottomThreshold;

    void Start()
    {
        _bottomThreshold = GameController.Instance.GetBottomThreshold();
    }

    public void SetupBullet(float angle, float speed, float size, Color color){
        
        _direction = transform.up;
        _speed = speed;
        _size = size;
        _bulletColor = color;
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _sr.color = color;

        _rb.velocity = _direction * _speed;
        transform.localScale = new Vector3(size, size, 1f);
    }

    private void FixedUpdate()
    {
        if (transform.position.y <= _bottomThreshold)
        {
            Alive(false);
        }

        if (_rb != null)
        {
            Vector3 speed = _rb.velocity;
            float mag = speed.magnitude;
            mag += _acceleration;
            if (mag >= _maxSpeed)
            {
                speed = speed.normalized * _maxSpeed;
            }
            else
            {
                speed = mag * speed.normalized;
            }
            _rb.velocity = speed;

        }
    }

   public void SetupStartResistant(){

    }



    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag.Equals("edge_ground")
            || other.gameObject.tag.Equals("ball")
            || other.gameObject.tag.Equals("laser")
            || other.gameObject.tag.Equals("paddle")){
            Alive(false);
        }
    }

    public bool IsActive(){
        return _isActive;
    }

    public void Alive(bool active){
        if(!active){
            _rb.velocity = Vector3.zero;
            GameController.Instance.shotManager.AddToInactive(this);
        }
        _isActive = active;
        gameObject.SetActive(active);
    }


    public void SetBulletExplosion(GameObject explosion){
        explosionAnimation = explosion;
    }

    public void SetBulletColor(Color color)
    {
        _bulletColor = color;
    }

    public void SetBulletScale(float scale)
    {
        _bulletScale = scale;
    }
}
