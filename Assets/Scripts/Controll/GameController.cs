using System.Collections;
using TMPro;
using UnityEngine;

public enum tagBullets
{
    player,
    inimigo
}

public enum gameState
{
    intro,
    gameplay
}

public enum estadoHelicoptero
{
    decolando,
    voando,
    pronto
}

public class GameController : MonoBehaviour
{
    public PlayerController _playerController;

    public gameState currentState;

    private estadoHelicoptero estadoAtualHelicopeto = estadoHelicoptero.decolando;

    
    public GameObject playerPrefab;
    public int extraLifes;

    public Transform spawnPlayer;
    public float delaySpawnPlayer;

    public float timeInvunerability;

    [Header("Limites de movimento")]
    public Transform limiteSuperior;
    public Transform limiteInferior;
    public Transform limiteEsquerdo;
    public Transform limiteDireito;

    public Transform cenario;
    public Transform posFinalFase;
    public float velocidadeFase;

    [Header("Prefabs")]
    public GameObject[] prefabBullets;
    public GameObject PrefabExplosion;

    public bool isAlivePlayer;

    [Header("Config. Intro Fase")]

    public float tamanhoInicialNave;
    public float tamanhoOriginalNave;

    public float velocidadeDecolagemMax;
    private float velocidadeAtual;

    public Transform posicaoInicialNave;
    public Transform posicaoDecolagemAviao;
    public Transform posicaoDecolagemHelicoptero;
    public Transform posicaoFinalMovimentoHelicoptero;

    public LayerMask layerMask;

    private bool isDecolar;
    [Header("Cor da Fumaça")]
    public Color corInicialFumaca;
    public Color corFinalFumaca;

    [Header("Config. HUD")]
    public TMP_Text txtVidasExtras;
    public TMP_Text txtMoedasExtras;
    public TMP_Text txtBombasExtras;

    public TMP_Text txtPontuacao;

    //Variaveis de score
    public int score;

    //controle para habilitar inimigos aereos
    public GameObject ativarInimigo;
    [Header("Spawn dos Aviões Humanos")]
    public GameObject[] prefabEnemys;
    public Transform[] localSpawnEnemys;
    [Header("Spawn dos Aviões Alienigenas")]
    public GameObject[] prefabEnemysAliens;
    public Transform[] localSpawnEnemysAliens;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        txtPontuacao.text = "0";
        txtVidasExtras.text = extraLifes.ToString();
        txtMoedasExtras.text = "0";
        txtBombasExtras.text = "0";

        currentState = gameState.intro;
        StartCoroutine("IntroFase");
        StartCoroutine("AtivarNavesInimigas");

        StartCoroutine("SpawnEnemys");
        StartCoroutine("SpawnAliens");
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlivePlayer == true)
        {
            LimitMovimentPlayer();
        }

        ControleHelicoptero();
        ControleAviao();
        
        
    }

    private void LateUpdate()
    {
        if (currentState == gameState.gameplay)
        {
            cenario.position = Vector3.MoveTowards(cenario.position, new Vector3(cenario.position.x, posFinalFase.position.y, 0), velocidadeFase * Time.deltaTime);
        }
        //vai movimentar a fase
    }

    public void ControleHelicoptero()
    {
        //CONDIÇÃO CASO O JOGADOR SELECIONE UM HELICOPTERO
        //Só funciona com helicoptero
        if (_playerController != null && _playerController.gameObject != null &&
            _playerController.gameObject.layer == 6 && isDecolar == true && currentState == gameState.intro)
        {
            switch (estadoAtualHelicopeto)
            {
                //caso o estado seja decolando
                case estadoHelicoptero.decolando:
                    //vai mover até o ponto de decolagem
                    StartCoroutine("Subir");
                    _playerController.transform.position = Vector3.MoveTowards(_playerController.transform.position,
                    posicaoDecolagemHelicoptero.position, velocidadeAtual * Time.deltaTime);
                    //caso tenha chego no ponto de decolagem, mudara o estado para voando
                    _playerController.playerSombra.SetActive(true);

                    if (Vector3.Distance(_playerController.transform.position, posicaoDecolagemHelicoptero.position) < 0.01f)
                    {
                        print("Helicoptero - Subir pouco");
                        estadoAtualHelicopeto = estadoHelicoptero.voando;
                    }
                    break;
                //caso estado seja voando
                case estadoHelicoptero.voando:
                    //movera o helicoptero até o segundo ponto
                    _playerController.transform.position = Vector3.MoveTowards(_playerController.transform.position,
                        posicaoFinalMovimentoHelicoptero.position, velocidadeAtual * Time.deltaTime);
                    //chegando no segundo ponto, ele entrara no estado pronto onde o jogo poderá começar
                    if (Vector3.Distance(_playerController.transform.position, posicaoFinalMovimentoHelicoptero.position) <= 0.001f)
                    {
                        print("Começou a voar");
                        currentState = gameState.gameplay;
                    }
                    break;
            }

        }
    }
    public void ControleAviao()
    {
        //CONDIÇÃO CASO O JOGADOR SELECIONE UMA NAVE
        //Só funciona com nave
        if (_playerController != null && _playerController.gameObject != null &&
            _playerController.gameObject.layer == 7 && isDecolar == true && currentState == gameState.intro)
        {
            _playerController.transform.position = Vector3.MoveTowards(_playerController.transform.position, posicaoDecolagemAviao.position, velocidadeAtual * Time.deltaTime);
            if (Vector3.Distance(_playerController.transform.position, posicaoDecolagemAviao.position) < 0.01f)
            {
                _playerController.playerSombra.SetActive(true);
                StartCoroutine("Subir");
                print("Avião - Subir muito");
                currentState = gameState.gameplay;

            }
        }
    }

    void LimitMovimentPlayer()
    {
        float clampedX = Mathf.Clamp(_playerController.transform.position.x, limiteEsquerdo.position.x, limiteDireito.position.x);
        float clampedY = Mathf.Clamp(_playerController.transform.position.y, limiteInferior.position.y, limiteSuperior.position.y);

        _playerController.transform.position = new Vector3(clampedX, clampedY, 0);
    }
    public string ApplyTag(tagBullets tag)
    {
        string retorno = null;

        switch (tag)
        {
            case tagBullets.player:
                retorno = "PlayerShot";
                break;
            case tagBullets.inimigo:
                retorno = "EnemyShot";
                break;
        }
        return retorno;
    }

    //função ao tomar tiro inimigo
    public void HitPlayer()
    {
        isAlivePlayer = false;
        if (_playerController != null)
        {
            GameObject temp = Instantiate(PrefabExplosion,_playerController.transform.position, PrefabExplosion.transform.localRotation);
            Destroy(_playerController.gameObject);
            _playerController = null;
        }
        extraLifes--;
        if (extraLifes >= 0)
        {
            StartCoroutine("InstantiatePlayer");
        }
        else
        {
            print("GAME OVER");
        }

        if (extraLifes < 0) {extraLifes = 0; }
        txtVidasExtras.text = extraLifes.ToString();
        
    }

    /*public void PositionSpawnEnemys()
    {

    }*/

    IEnumerator InstantiatePlayer()
    {
        yield return new WaitForSeconds(delaySpawnPlayer);
        if (playerPrefab == null)
        {
            Debug.LogError("PlayerPrefab não esta atribuido");
            yield break; 
        }
        
        GameObject temp = Instantiate(playerPrefab, spawnPlayer.position, spawnPlayer.localRotation);

        _playerController = temp.GetComponent<PlayerController>();
        yield return new WaitForEndOfFrame();
        if (_playerController != null)
        {
            _playerController.StartCoroutine("Invencible");
            isAlivePlayer = true;
        }
        else
        {
            Debug.LogError("Não foi possível encontrar o PlayerController no Prefab instanciado!");
        }
    }
    
    IEnumerator IntroFase()
    {
        _playerController.fumacaSr.color = corInicialFumaca;
        _playerController.transform.localScale = new Vector3(tamanhoInicialNave, tamanhoInicialNave, tamanhoInicialNave);
        _playerController.transform.position = posicaoInicialNave.position;

        yield return new WaitForEndOfFrame();
        isDecolar = true;

        print("Autorizado");
        //a cada 0.2 segundos a velocidade ira aumentar
        for (velocidadeAtual = 0; velocidadeAtual < velocidadeDecolagemMax; velocidadeAtual += 0.1f)
        {
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator Subir()
    {

        float duracao = 1f;
        float tempo = 0.1f;

        Vector3 escalaInicial = Vector3.one * tamanhoInicialNave;
        Vector3 escalaFinal = Vector3.one * tamanhoOriginalNave;

        while(tempo < duracao)
        {
            _playerController.transform.localScale = Vector3.Lerp(escalaInicial, escalaFinal, tempo / duracao);
            _playerController.fumacaSr.color = Color.Lerp(_playerController.fumacaSr.color, corFinalFumaca, 0.05f);

            if (Mathf.Approximately(_playerController.transform.localScale.x, tamanhoOriginalNave))
            {
                break;
            }
            tempo += Time.deltaTime;

            yield return null;
        }
        _playerController.transform.localScale = escalaFinal;
        print("Altura Maxima");
    }

    IEnumerator AtivarNavesInimigas()
    {
        yield return new WaitForSeconds(15);
        ativarInimigo.SetActive(true);
    }
    IEnumerator SpawnEnemys()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            int enemysAleatorios = Random.Range(0, prefabEnemys.Length);
            int positionEnemy = Random.Range(0, localSpawnEnemys.Length);
            GameObject temp1 = Instantiate(prefabEnemys[enemysAleatorios], localSpawnEnemys[positionEnemy].position, localSpawnEnemys[positionEnemy].localRotation);
            GameObject temp2 = Instantiate(prefabEnemys[enemysAleatorios], localSpawnEnemys[positionEnemy].position, localSpawnEnemys[positionEnemy].localRotation);
            print("Inimigo spawnado");
        }

    }

    IEnumerator SpawnAliens()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            int enemysAleatorios = Random.Range(0, prefabEnemysAliens.Length);
            int positionEnemy = Random.Range(0, localSpawnEnemys.Length);
            GameObject temp1 = Instantiate(prefabEnemysAliens[enemysAleatorios], localSpawnEnemysAliens[positionEnemy].position, localSpawnEnemysAliens[positionEnemy].localRotation);
            GameObject temp2 = Instantiate(prefabEnemysAliens[enemysAleatorios], localSpawnEnemysAliens[positionEnemy].position, localSpawnEnemysAliens[positionEnemy].localRotation);
            print("Inimigo spawnado");
        }
    }

    public void ScoreGame(int pontos)
    {
        score += pontos;
        txtPontuacao.text = score.ToString();
    }
}
