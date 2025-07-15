using System.Collections;
using UnityEngine;
public enum DirecaoInicial
{
    Cima,
    Baixo,
    Direita,
    Esquerda
}
public class IAEnemy : MonoBehaviour
{
    private PlayerController _playerController;

    private GameController _gameController;

    private LootEnemy _instanceDeathEnemy;

    //variavel para vleocidade da curva
    public float velocidadeMovimento;
  
    //variavel para o ponto onde vai iniciar a curva(eixo Y)
    public float pontoInicialCurva;
    //verificar se já está no ponto da curva
    private bool isCurva;
    //variavel que indica quantos graus sera essa curva
    public float grausCurva;
    //variavel para definir a direção que será girado o inimigo
    public float incrementar;
    //vai incrementar a curva, saber o quanto foi curvado
    private float incrementado;
    //eixo que sera girado
    private float rotacaoZ;

    //controle da armaBasica inimigo
    public Transform arma;
    public float velocidadeTiro;
    public int idBullet;
    public tagBullets tagBullet;

    public float delayTiro;

    public DirecaoInicial direcaoMovimento;

    Vector3 direcao;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerController = FindFirstObjectByType<PlayerController>();
        _gameController = FindFirstObjectByType<GameController>();
        rotacaoZ = transform.eulerAngles.z;
        _instanceDeathEnemy = GetComponent<LootEnemy>();
        
    }

    // Update is called once per frame
    void Update()
    {
        DirectionControll();

        if (Input.GetButtonDown("Jump"))
        {
            Atirar();
        }
    }
    public void Atirar()
    {
        GameObject temp = Instantiate(_gameController.prefabBullets[idBullet], arma.position, transform.localRotation);
        temp.transform.tag = _gameController.ApplyTag(tagBullet);
        temp.transform.rotation = Quaternion.Euler(0,0, transform.localRotation.z);

        temp.GetComponent<Rigidbody2D>().linearVelocity = transform.up * velocidadeTiro * -1;
    }

    public void DirectionControll()
    {
        switch (direcaoMovimento)
        {
            case DirecaoInicial.Cima:
                direcao = Vector3.up;
                if (transform.position.y >= pontoInicialCurva && isCurva == false)
                {
                    isCurva = true;
                }
                break;
            case DirecaoInicial.Baixo:
                direcao = Vector3.down;
                if (transform.position.y <= pontoInicialCurva && isCurva == false)
                {
                    isCurva = true;
                }
                break;
            case DirecaoInicial.Esquerda:
                direcao = Vector3.left;
                if (transform.position.x <= pontoInicialCurva && isCurva == false)
                {
                    isCurva = true;
                }
                break;
            case DirecaoInicial.Direita:
                direcao = Vector3.right;
                if (transform.position.x >= pontoInicialCurva && isCurva == false)
                {
                    isCurva = true;
                }
                break;
        }

        if (isCurva && incrementado < grausCurva)
        {
            rotacaoZ += incrementar;
            transform.rotation = Quaternion.Euler(0,0, rotacaoZ);
            
            if (incrementar < 0)
            {
                incrementado += (incrementar * -1);
            }
            else
            {
                incrementado += incrementar;
            }
        }

        transform.Translate(Vector3.down * velocidadeMovimento * Time.deltaTime);
    }

    private void OnBecameVisible()
    {
        StartCoroutine("TiroComDelay");
    }

    IEnumerator TiroComDelay()
    {
        yield return new WaitForSeconds(delayTiro);
        Atirar();
        StartCoroutine("TiroComDelay");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "PlayerShot":
                GameObject temp = Instantiate(_gameController.PrefabExplosion, transform.position, _gameController.PrefabExplosion.transform.localRotation);
                Destroy(this.gameObject);
                Destroy(collision.gameObject);
                _instanceDeathEnemy.SpawnLoot();
                break;
            case "Player":
                Destroy(this.gameObject);
                _gameController.HitPlayer();
                break;
        }
    }
    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
