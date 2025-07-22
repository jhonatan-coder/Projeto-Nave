using System.Collections;
using UnityEngine;



public class IATanque : MonoBehaviour
{
    private GameController _gameController;
    private MusicManager _musicManager;
    public GameObject prefabTank;
    public LootEnemy lootEnemy;

    public Transform arma;

    public int idBullet;
    public float velocidadeTiro;

    public int points;

    public tagBullets tagTiro;

    public float delayTiro;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lootEnemy = GetComponent<LootEnemy>();
        _gameController = FindFirstObjectByType<GameController>();
        _musicManager = FindFirstObjectByType<MusicManager>();
    }


    IEnumerator ControleTiro()
    {
        yield return new WaitForSeconds(delayTiro);
        if (_gameController.isAlivePlayer)
        {
            Atirar();
        }
        StartCoroutine("ControleTiro");
    }

    public void Atirar()
    {
        arma.up = _gameController._playerController.transform.position - transform.position;
        GameObject temp = Instantiate(_gameController.prefabBullets[idBullet], arma.position, arma.localRotation);
        temp.transform.tag = _gameController.ApplyTag(tagTiro);
        temp.GetComponent<Rigidbody2D>().linearVelocity = arma.up * velocidadeTiro;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "PlayerShot":
                GameObject temp = Instantiate(_gameController.PrefabExplosion, transform.position, transform.localRotation);
                //transforma a explosão em filho do cenario para que a explosão fique exatamente no local que estava
                temp.transform.parent = _gameController.cenario;
                Destroy(collision.gameObject);
                Destroy(this.gameObject);
                _gameController.pontosInimigos = 15;
                _gameController.ScoreGame();
                lootEnemy.SpawnLoot();
                _musicManager.FXGeral(_musicManager.FxExplosao);
                break;
        }
    }

    private void OnBecameVisible()
    {
        StartCoroutine("ControleTiro");
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
