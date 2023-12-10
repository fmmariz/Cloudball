using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField]
    private int upforce;
    private Rigidbody2D _rb;
    // Start is called before the first frame update

    [SerializeField]
    private int kickupformaxdmg;
    private int _kickup;

    private float _blockHitDelay = 0.2f;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
       
    }

    // Update is called once per frame
    void Update()
    {
        damageByLaserCountdown -= Time.deltaTime;

        if (_blockHitDelay > 0)
        {
            _blockHitDelay -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("paddle") )
        {
            GameController.Instance.soundManager.PlaySoundEffect("bouncewall");

            KickUp();
            _kickup += 1;
            GameController.Instance.uiManager.UpdateUmbrellaHitsNumber(_kickup);
        }else if (collision.gameObject.CompareTag("block"))
        {

            if(_blockHitDelay <= 0)
            {
                GameController.Instance.soundManager.PlaySoundEffect("bouncecloud");
                Vector3 contact = collision.contacts[0].normal;
                if(contact.y > 0) _rb.AddForce(contact * 5f);
                collision.gameObject.GetComponent<BlockController>().
                    DealDamage( 1,
                    collision.GetContact(0).normal,
                    _rb.velocity);
            }
        }
        else if (collision.gameObject.CompareTag("edge_ground"))
        {
            GameController.Instance.soundManager.PlaySoundEffect("bouncefloor");

            _kickup = 0;
            GameController.Instance.uiManager.UpdateUmbrellaHitsNumber(_kickup);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("laser"))
        {
            _rb.AddForce(Vector2.up * upforce * 1.2f);
            _kickup += 1;
        }
    }

    public void KickUp()
    {
        _rb.AddForce(Vector2.up * upforce * 1.2f);
    }

    private float damageByLaserDelay = 0.5f;
    private float damageByLaserCountdown = 0f;
    public void DamageByLaser()
    {
        if(damageByLaserCountdown <= 0)
        {
            damageByLaserCountdown = damageByLaserDelay;
            KickUp();
        }

    }
}
