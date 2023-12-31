using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D _rb;
    private float _speed;
    private float _size;
    private Vector3 _direction;
    private bool _isActive;
    private GameObject explosionAnimation;
    private bool _playerBullet;

    private float _bottomThreshold;

    void Start()
    {
        _bottomThreshold = GameController.Instance.GetBottomThreshold();
    }

    public void SetupBullet(float angle, float speed, float size){
        
        _direction = transform.up;
        _speed = speed;
        _size = size;
        _rb = GetComponent<Rigidbody2D>();


        _rb.velocity = _direction * _speed;
        transform.localScale = new Vector3(size, size, 1f);
    }

    public void SetupStartResistant(){

    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y <= _bottomThreshold)
        {
            Alive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag.Equals("screenEdge")){
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
}
