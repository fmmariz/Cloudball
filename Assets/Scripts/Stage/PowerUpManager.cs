using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PowerUpManager : MonoBehaviour
{

    public enum PowerUpType
    {
        LIFE,
        SCORE,
        LASER,
        WIDEPADDLE,
        MOREBALL,
        SLOWDOWN
    }

    [SerializeField] 
    public GameObject powerUpPrefab;
    [SerializeField]
    public List<PowerUpType> powerUpTypes;
    [SerializeField]
    public List<Sprite> powerUpSprites;
    [SerializeField]
    public GameObject extraballPrefab;
    [SerializeField]
    public float extraBallDuration;
    [SerializeField]
    public float multipaddleDuration;
    [SerializeField]
    public float laserDuration;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPowerUp(Vector2 position, PowerUpType powerUpType, float intensity)
    {
        GameObject pup = Instantiate(powerUpPrefab, position, Quaternion.identity);
        if (pup.TryGetComponent<PowerUpController>(out PowerUpController pupController))
        {
            Sprite powerupSprite = powerUpSprites[powerUpTypes.IndexOf(powerUpType)];
            pupController.InitializePowerUp(powerUpType, powerupSprite, intensity);
        }
    }

    public void SpawnExtraBall(Vector3 position)
    {
        GameObject extraBall = Instantiate(extraballPrefab, position, Quaternion.identity);
        if(extraBall.TryGetComponent<TimedBallScript>(out TimedBallScript tbpScript))
        {
            tbpScript.SetTimer(extraBallDuration);
        }

    }

    [Header("Distribution of Power Ups")]
    [SerializeField] float nothing;
    [SerializeField] float lifeChance;
    [SerializeField] float wideChance;
    [SerializeField] float ballChance;
    [SerializeField] float laserChance;
    [SerializeField] float slowdownChance;
    public PowerUpType? GetTypeRandomly()
    {
        PowerUpType? returnedType = null;
        float proc = Random.Range(0f, nothing +lifeChance+wideChance+ballChance+laserChance+slowdownChance);
        if (proc > 0 && proc <= nothing)
        {
            return null;
        } else if (proc > nothing && proc <= (nothing + lifeChance))
        {
            return PowerUpType.LIFE;
        } else if (proc > (nothing + lifeChance) && proc <= (nothing+lifeChance+wideChance))
        {
            return PowerUpType.WIDEPADDLE;
        } else if (proc > (nothing + lifeChance + wideChance) 
            && proc <= (nothing + lifeChance + wideChance +ballChance))
        {
            return PowerUpType.MOREBALL;
        } else if (proc > (nothing + lifeChance + wideChance + ballChance)
            && proc <= (nothing + lifeChance + wideChance + ballChance + laserChance))
        {
            return PowerUpType.LASER;
        } else if (proc > (nothing + lifeChance + wideChance + ballChance + laserChance)
            && proc <= (nothing + lifeChance + wideChance + ballChance + laserChance + slowdownChance))
        {
            return PowerUpType.SLOWDOWN;
        }

        return returnedType;
    }
}
