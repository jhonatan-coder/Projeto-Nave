using System.Collections;
using UnityEngine;

public class EnemyControll : MonoBehaviour
{
    private PlayerController _instancePlayer;
    private GameController _gameController;

    private LootEnemy _instanceDeathEnemy;

    public GameObject explosionPrefab;

    public Transform weapom;
    //public GameObject bulletPrefab01;
    // public GameObject bulletPrefab02;
    public int idBullet;

    //controla o tempo de tiros
    public float[] timeShot;
    public float spriteRotationOffset = -90f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _instancePlayer = FindFirstObjectByType<PlayerController>();
        _gameController = FindFirstObjectByType<GameController>();
        _instanceDeathEnemy = GetComponent<LootEnemy>();
        StartCoroutine("TiroLentoInimigo");
        StartCoroutine("TiroRapidoInimigo");
    }


    //CONTROLE DO TIRO DO INIMIGO
    IEnumerator TiroLentoInimigo()    {

        if (_instancePlayer == null) yield break;
        TiroLento();
        yield return new WaitForSeconds(Random.Range(5f, 15f));
        StartCoroutine("TiroLentoInimigo");
       
    }

    IEnumerator TiroRapidoInimigo()
    {
        if (_instancePlayer == null) yield break;
        yield return new WaitForSeconds(Random.Range(timeShot[0], timeShot[1]));
        TiroRapido();
        StartCoroutine("TiroRapidoInimigo");
    }

    public void TiroLento()
    {
        //Faz a armaBasica girar em direção ao player, caso houver uma armaBasica visível
        if (_gameController.isAlivePlayer)
        {
            AnguloDoTiro(5f);
        }
    }

    public void TiroRapido()
    {
        //Faz a armaBasica girar em direção ao player, caso houver uma armaBasica visível
        if (_gameController.isAlivePlayer)
        {
            AnguloDoTiro(7f);
        }
    }

    public void AnguloDoTiro(float shotVelocity)
    {
        weapom.right = _instancePlayer.transform.position - transform.position;
        GameObject temp = Instantiate(_gameController.prefabBullets[idBullet], weapom.position, weapom.localRotation);
        Vector2 direction = (_instancePlayer.transform.position - weapom.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        temp.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        temp.GetComponent<Rigidbody2D>().linearVelocity = direction * shotVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "PlayerShot":
                Destroy(collision.gameObject);
                GameObject temp = Instantiate(explosionPrefab, transform.position, transform.localRotation);
                Destroy(temp.gameObject, 0.4f);
                _instanceDeathEnemy.SpawnLoot();
                Destroy(this.gameObject);
                
                break;
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
