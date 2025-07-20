using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRB;

    private GameController _gameController;
    private MusicManager _controllMusic;
    private MenuOptions _menuOptions;

    public GameObject playerSombra;

    private SpriteRenderer playerSr;
    public SpriteRenderer fumacaSr;

    public Transform weaponPosition;
    public float speedBullet;

    public float speedPlayer;
    public int idBullet;

    public tagBullets tagBullet;

    public Color corInvencible;

    public float delayPiscar;

    public int hit;
    private bool isPlayerAlive;
    public bool IsPlayerAlive { get => isPlayerAlive; set => isPlayerAlive = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        _gameController = FindFirstObjectByType<GameController>();
        playerSr = GetComponent<SpriteRenderer>();
        _controllMusic = FindFirstObjectByType<MusicManager>();
        _menuOptions = FindFirstObjectByType<MenuOptions>();
        _gameController._playerController = this;
        _gameController.isAlivePlayer = true;
        playerSombra.SetActive(false);
        IsPlayerAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameController.currentState == gameState.gameplay)
        {
            if (Input.GetButtonDown("Fire1") && !_menuOptions.MenuAberto)
            {
                Shot();
                _controllMusic.FxTiroHelicoptero();
            }
        }
    }

    private void FixedUpdate()
    {
        if (_gameController.currentState == gameState.gameplay) 
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            playerRB.linearVelocity = new Vector2(horizontal * speedPlayer, vertical * speedPlayer);
        }
       
    }

    void Shot()
    {
        GameObject temp = Instantiate(_gameController.prefabBullets[idBullet]);
        temp.transform.tag = _gameController.ApplyTag(tagBullet);
        temp.transform.position = weaponPosition.position;
        temp.GetComponent<Rigidbody2D>().linearVelocityY = speedBullet;

    }
    IEnumerator Invencible()
    {
        Collider2D coll = GetComponent<Collider2D>();
        coll.enabled = false;
        playerSr.color = corInvencible;
        fumacaSr.color = corInvencible;
        yield return new WaitForSeconds(_gameController.timeInvunerability);
        coll.enabled = true;
        playerSr.color = Color.white;
        fumacaSr.color = Color.white;
        playerSr.enabled = true;
        fumacaSr.enabled = true;
        StopCoroutine("SpritePiscaPisca");
    }

    IEnumerator SpritePiscaPisca()
    {
        yield return new WaitForSeconds(delayPiscar);
        playerSr.enabled = !playerSr.enabled;
        fumacaSr.enabled = !fumacaSr.enabled;
        StartCoroutine("SpritePiscaPisca");

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "EnemyShot":
                _gameController.HitPlayer();
                Destroy(collision.gameObject);
                break;
            case "Enemy":
                _gameController.HitPlayer();
                Destroy(collision.gameObject);
                break;
            case "EnemyBoss":
                _gameController.HitPlayer();
                break;
        }
        if (collision.CompareTag("Life"))
        {
            _gameController.LifePlus();
            if (_gameController.extraLifes < 3)
            {
                Destroy(collision.gameObject);
            }

        }
        else if (collision.CompareTag("Coin"))
        {
            _gameController.ScorePlus();
            Destroy(collision.gameObject);

        }
        else if (collision.CompareTag("ShotPlus"))
        {
            _gameController.ShotPlus();


            hit += _gameController.shotPlus;
            Destroy(collision.gameObject);

        }
    }

}
