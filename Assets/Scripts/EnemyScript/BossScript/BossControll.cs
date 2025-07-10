using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossControll : MonoBehaviour
{
    public PlayerController _playerController;
    public GameController _gameController;
    private LootEnemy _lootEnemy;
    private LootBoss _lootBoss;

    public tagBullets tagbullet;

    private SpriteRenderer sprite;
    public Color corNormal;
    public Color corHit;

    private EnemyPath _enemypath;
    public float velocidade;
    private int indexAtual = 0;
    private bool chegouNoDestino = false;

    //Arma    
    public Transform armaBasica;
    public Transform[] armasAvancadas;

    private bool podeAtacar = true;
    //tipo de munição
    public int idBullet;

    public int velocidadeDoTiroBasico;
    public int velocidadeDoTiroAvancado;

    //barra de vida
    public Slider sliderLife;
    private int vidaMaxima = 200;
    private int vidaAtual;

   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gameController = FindFirstObjectByType<GameController>();
        _playerController = FindFirstObjectByType<PlayerController>();
        _enemypath = FindFirstObjectByType<EnemyPath>();
        sprite = GetComponent<SpriteRenderer>();
        _lootEnemy = GetComponent<LootEnemy>();
        _lootBoss = GetComponent<LootBoss>();
        StartCoroutine("AtaqueDoBoss");
        vidaAtual = vidaMaxima;
        sliderLife.maxValue = vidaMaxima;
        sliderLife.value = vidaAtual;
        sliderLife.fillRect.gameObject.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        MovimentacaoBoss();
    }


    public void MovimentacaoBoss()
    {
        if (_enemypath == null || _enemypath.checkPaths.Length == 0) return;

        Transform destino = _enemypath.checkPaths[indexAtual];

        transform.position = Vector3.MoveTowards(
            transform.position,                     //posição própria
            destino.position,  //posição do que ele ira seguir
            velocidade * Time.deltaTime);           // velocidade
        if (Vector3.Distance(transform.position, destino.position) < 0.01f && !chegouNoDestino)
        {
            chegouNoDestino = true;
            Invoke(nameof(ProximoPonto), 0.1f);
        }
    }
    void ProximoPonto()
    {
        chegouNoDestino = false;
        indexAtual = Random.Range(0, _enemypath.checkPaths.Length);
    }

    IEnumerator AtaqueDoBoss()
    {   
        while (true)
        {
            podeAtacar = false;
            yield return new WaitForSeconds(1f);
            AtaqueBasico();
            yield return new WaitForSeconds(3f);
            AtaqueAvancado();
            podeAtacar = true;
        }

    }


    public void AtaqueBasico()
    {
        if (_gameController.isAlivePlayer) 
        {
            armaBasica.right = _playerController.transform.position - transform.position;
            GameObject temp = Instantiate(_gameController.prefabBullets[idBullet], armaBasica.position, armaBasica.rotation);
            temp.transform.tag = _gameController.ApplyTag(tagbullet);
            Vector2 direction = (_playerController.transform.position - armaBasica.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            temp.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
            temp.GetComponent<Rigidbody2D>().linearVelocity = direction * velocidadeDoTiroBasico;
        }
    }

    public void AtaqueAvancado()
    {
        for (int i = 0; i < armasAvancadas.Length; i++)
        {
            if (_gameController.isAlivePlayer)
            {
                armasAvancadas[i].right = _playerController.transform.position - transform.position;
                GameObject temp = Instantiate(_gameController.prefabBullets[idBullet + 1], armasAvancadas[i].position, armasAvancadas[i].rotation);
                temp.transform.tag = _gameController.ApplyTag(tagbullet);
                Vector2 direction = (_playerController.transform.position - armasAvancadas[i].position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                temp.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
                temp.GetComponent<Rigidbody2D>().linearVelocity = direction * velocidadeDoTiroAvancado;
            }
            else
            {
                break;
            }
        }
    }

    

    public void DanoNaVida()
    {
        //Troca de cor momentaneamente
        StartCoroutine("AlteraCor");
        //diminuir a barra de vida
        vidaAtual -= _playerController.hit;
        vidaAtual = Mathf.Clamp(vidaAtual, 0, vidaMaxima);
        sliderLife.value = vidaAtual;
    }

    IEnumerator AlteraCor()
    {
        sprite.color = corHit;
        yield return new WaitForSeconds(0.1f);
        sprite.color = corNormal;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "PlayerShot":
                //vai levar hit e alterar de cor momentaneamente
                DanoNaVida();
                //explodir após levar o ultimo hit
                Destroy(collision.gameObject);
                if (vidaAtual <= 0)
                {
                    vidaAtual = 0;
                    print("Morreu");
                    sliderLife.fillRect.gameObject.SetActive(false);
                    GameObject temp = Instantiate(_gameController.PrefabExplosion, transform.position, transform.localRotation);
                    Destroy(this.gameObject);
                    Destroy(temp.gameObject, 0.4f);
                    //Dropar itens e muitos pontos
                    _lootBoss.SpawnLootBoss();
                    

                }
                
                break;
        }
    }

}
