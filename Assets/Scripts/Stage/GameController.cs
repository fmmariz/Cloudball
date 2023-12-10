using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

public class GameController : MonoBehaviour
{

    public static GameController Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    [SerializeField]
    public GameObject _ground;
    public GameObject _ceiling;

    [SerializeField]
    public GameObject _lWall;
    public GameObject _rWall;

    private float _leftThreshold;
    private float _rightThreshold;

    [SerializeField]
    private SpriteRenderer _background;
    [SerializeField]
    private List<Sprite> _backroundSprites;


    private BlockManager _blockManager;
    public ScoreManager scoreManager;
    public UIManager uiManager;
    public PaddleController player;
    public PowerUpManager powerUpManager;
    public ShotManager shotManager;
    public SFXManager soundManager;

    void Start()
    {
        _ground.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        _ceiling.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        _lWall.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        _rWall.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        _leftThreshold = _lWall.gameObject.GetComponent<Collider2D>().bounds.max.x;
        _rightThreshold = _rWall.gameObject.GetComponent<Collider2D>().bounds.min.x;

        scoreManager = GetComponent<ScoreManager>();
        uiManager = GetComponent<UIManager>();
        powerUpManager = GetComponent<PowerUpManager>();
        soundManager = GetComponent<SFXManager>();
        shotManager = GetComponent<ShotManager>();

        _blockManager = GetComponent<BlockManager>();

        switch (StageControlSingleton.instance.GetStage())
        {
            case 1:
                InitiateStage1();
                break;
            case 2:
                InitiateStage2();
                break;
            case 3:
                InitiateStage3();
                break;
        }



    }

    public void InitiateStage1()
    {
        _background.sprite = _backroundSprites[0];
        GameController.Instance.soundManager.PlaySoundEffect("bg1");
        float left = _leftThreshold + 0.5f;
        float right = _rightThreshold - 0.5f;
        float top = 3;
        float bottom = 0;

        int rows = 5;
        int columns = 4;


        for (int o = 0; o <= rows; o++)
        {
            for (int i = 0; i <= columns; i++)
            {
                float x = Mathf.Lerp(left, right, (float)o / (float)rows);
                float y = Mathf.Lerp(top, bottom, (float)i / (float)columns);

                PowerUpManager.PowerUpType? getPowerUp = powerUpManager.GetTypeRandomly();

                _blockManager.CreateBlockAt(new Vector2(x, y), 4 - i, getPowerUp);
            }
        }
        //StartCoroutine(Droplets());
    }

    public void InitiateStage2()
    {
        _background.sprite = _backroundSprites[1];
        GameController.Instance.soundManager.PlaySoundEffect("bg2");
        float left = _leftThreshold + 0.5f;
        float right = _rightThreshold - 0.5f;
        float top = 3;
        float bottom = 0;
        float cloudNumber = 30;
        float centerx = (left + right) / 2;
        float centery = (bottom + top) / 2;
        float step = 360f / cloudNumber;
        for (float i = 0; i < cloudNumber; i++)
        { 
            float x = centerx + 2f * Mathf.Sin(i * step* Mathf.Deg2Rad);
            float y = centery + 1f * Mathf.Cos(i * step * Mathf.Deg2Rad);
            PowerUpManager.PowerUpType? getPowerUp = powerUpManager.GetTypeRandomly();

            _blockManager.CreateBlockAt(new Vector2(x, y),2, getPowerUp);
        }
    }

    public void InitiateStage3()
    {
        _background.sprite = _backroundSprites[2];
        GameController.Instance.soundManager.PlaySoundEffect("bg3");
        float left = _leftThreshold + 0.5f;
        float right = _rightThreshold - 0.5f;
        float top = 3;
        float bottom = 0;

        float cloudNumber = 4f;
        float centerx = (left + right) / 2f;
        float centery = (bottom + top) / 2f;
        float spiral = 0.5f;
        float wings = 6f;
        float wingVar = 360f / wings;
        for (float i = 0; i < cloudNumber; i++)
        {
            for (float o = 0; o < wings; o++)
            {
                Debug.Log(o * wingVar);
                float x = centerx + spiral * 2 * Mathf.Cos(i*(45/cloudNumber) + o * wingVar * Mathf.Deg2Rad);
                float y = centery + spiral * Mathf.Sin(i * (45 / cloudNumber) + o *wingVar * Mathf.Deg2Rad);
                PowerUpManager.PowerUpType? getPowerUp = powerUpManager.GetTypeRandomly();

                _blockManager.CreateBlockAt(new Vector2(x, y), (int) (4 - i), getPowerUp);
            }
        
            spiral += 0.4f;

        }
    }

    IEnumerator Droplets()
    {
        while (true)
        {
            shotManager.EnemyShoot(new Vector3(
                Random.Range(GetLeftThreshold(), GetRightThreshold()),
                4f,
                0f),
                1f,
                180f,
                1);
            yield return new WaitForSeconds(0.3f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
        
    }

    public float GetLeftThreshold()
    {
        return _leftThreshold;
    }

    public float GetRightThreshold()
    {
        return _rightThreshold;
    }

    public float GetBottomThreshold()
    {
        return player.transform.position.y;
    }

    public Vector2 GetPlayerPosition()
    {
        return player.transform.position;
    }

}
