using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsController : MonoBehaviour
{
    public static EffectsController Instance { get; private set; }
    private void Awake() 
    { 
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 

        _inactiveEnemyExplosions = new List<GameObject>();
        _inactiveBulletExplosions = new List<GameObject>();
    }

    [SerializeField]
    private GameObject _enemyExplosion;
    [SerializeField]
    private GameObject _enemyImplosion;
    [SerializeField]
    private GameObject _bulletExplosion;
    [SerializeField]
    private GameObject _bulletImplosion;
    
    private List<GameObject> _inactiveEnemyExplosions;
    private List<GameObject> _inactiveBulletExplosions;



    public void CreateBulletImplosion(Vector3 position, float speed){
        if(_inactiveBulletExplosions.Count>0){
            GameObject oldExp = _inactiveBulletExplosions[0];
            _inactiveBulletExplosions.RemoveAt(0);
            oldExp.transform.position = position;
            oldExp.SetActive(true);
        }else{
            GameObject explosion = Instantiate(_bulletExplosion,position, Quaternion.identity,null);
        }
    }

    public void CreateBulletExplosion(Vector3 position, float speed){
        GameController.Instance.soundManager.PlaySoundEffect("hit");
        if(_inactiveBulletExplosions.Count>0){
            GameObject oldExp = _inactiveBulletExplosions[0];
            _inactiveBulletExplosions.RemoveAt(0);
            oldExp.GetComponent<Animator>().SetBool("explosion", true);
            oldExp.transform.position = position;
            oldExp.SetActive(true);
        }else{
            GameObject explosion = Instantiate(_bulletExplosion,position, Quaternion.identity,null);
            explosion.GetComponent<Animator>().SetBool("explosion", true);
        }
    }

    public void CreateEnemyImplosion(Vector3 position, float speed){
        if (_inactiveEnemyExplosions.Count>0){
            GameObject oldExp = _inactiveEnemyExplosions[0];
            _inactiveEnemyExplosions.RemoveAt(0);
            oldExp.GetComponent<Animator>().SetBool("explosion",false);
            oldExp.GetComponent<Animator>().SetBool("implosion", true);
            oldExp.transform.position = position;
            oldExp.SetActive(true);
        }else{
            GameObject explosion = Instantiate(_enemyExplosion,position, Quaternion.identity,null);
            explosion.GetComponent<Animator>().SetBool("explosion",false);
            explosion.GetComponent<Animator>().SetBool("implosion", true);
        }
    }

    public void CreateEnemyExplosion(Vector3 position, float speed){
        if(_inactiveEnemyExplosions.Count>0){
            GameObject oldExp = _inactiveEnemyExplosions[0];
            _inactiveEnemyExplosions.RemoveAt(0);
            oldExp.transform.position = position;
            oldExp.SetActive(true);
        }else{
            GameObject explosion = Instantiate(_enemyExplosion,position, Quaternion.identity,null);
        }
    }

    public void AddToInactiveBullet(GameObject disabledObject){
        disabledObject.SetActive(false);
        _inactiveBulletExplosions.Add(disabledObject);
    }

    public void AddToInactiveEnemy(GameObject disabledObject){
        disabledObject.SetActive(false);
        _inactiveEnemyExplosions.Add(disabledObject);
    }

}
