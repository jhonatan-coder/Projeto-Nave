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
    private SceneController _controleDeCena;
    private MusicManager _musicManager;

    public gameState currentState;

    private estadoHelicoptero estadoAtualHelicopeto = estadoHelicoptero.decolando;

    
    public GameObject playerPrefab;
    public int extraLifes;
    public int scorePlus;
    public int scorePlusMomentaneo;
    public int lifePlus;
    public int shotPlus;

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
    private bool isProcessingDeath;

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
    public TMP_Text txtshotsPlus;

    public TMP_Text txtPontuacao;

    //Variaveis de score
    public int score;
    //variavel que armazena o valor de cada inimigo para o score
    public int pontosInimigos;

    [Header("Spawn dos Aviões Humanos")]
    public GameObject prefabEnemies;
    public Transform[] localSpawnEnemies;
    [Header("Spawn dos Aviões Alienigenas")]
    public GameObject prefabEnemiesAliens;
    public Transform[] localSpawnEnemiesAliens;
    //controle para habilitar inimigos aereos
    public GameObject ativarInimigo;

    [Header("Spawn do Boss")]
    public GameObject ativarBossFinal;
    public Transform pontoSpawnBoss;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _controleDeCena = FindFirstObjectByType<SceneController>();
        _musicManager = FindFirstObjectByType<MusicManager>();
        _playerController = FindAnyObjectByType<PlayerController>();
        scorePlus = 1;
        shotPlus = 0;
        lifePlus = 0;

        txtPontuacao.text = "0";
        txtVidasExtras.text = extraLifes.ToString();
        txtMoedasExtras.text = "1";
        txtshotsPlus.text = "0";

        currentState = gameState.intro;
        StartCoroutine(IntroFase());
        StartCoroutine(AtivarNavesInimigas());

        StartCoroutine(SpawnHumansEnemies());
        StartCoroutine(SpawnEnemiesAliens());
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
        AtivarBoss();
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
                    //AQUI VAI O SOM DO HELICOPTERO DECOLANDO
                    _playerController.transform.position = Vector3.MoveTowards(_playerController.transform.position,
                    posicaoDecolagemHelicoptero.position, velocidadeAtual * Time.deltaTime);
                    //caso tenha chego no ponto de decolagem, mudara o estado para voando
                    _playerController.playerSombra.SetActive(true);

                    if (Vector3.Distance(_playerController.transform.position, posicaoDecolagemHelicoptero.position) < 0.01f)
                    {
                        estadoAtualHelicopeto = estadoHelicoptero.voando;                       
                    }
                    _musicManager.HelicopteroDecolando();
                    break;
                //caso estado seja voando
                case estadoHelicoptero.voando:
                    //movera o helicoptero até o segundo ponto
                    _playerController.transform.position = Vector3.MoveTowards(_playerController.transform.position,
                        posicaoFinalMovimentoHelicoptero.position, velocidadeAtual * Time.deltaTime);
                    //chegando no segundo ponto, ele entrara no estado pronto onde o jogo poderá começar
                    if (Vector3.Distance(_playerController.transform.position, posicaoFinalMovimentoHelicoptero.position) <= 0.001f)
                    {
                        currentState = gameState.gameplay;
                    }
                    //AQUI VAI O SOM DO HELICOPTERO VOANDO RAPIDO
                    _musicManager.HelicopteroVoando();
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
                currentState = gameState.gameplay;

            }
        }
    }

    //limita a movimentação do player em todos os angulos
    void LimitMovimentPlayer()
    {
        float clampedX = Mathf.Clamp(_playerController.transform.position.x, limiteEsquerdo.position.x, limiteDireito.position.x);
        float clampedY = Mathf.Clamp(_playerController.transform.position.y, limiteInferior.position.y, limiteSuperior.position.y);

        _playerController.transform.position = new Vector3(clampedX, clampedY, 0);
    }
    //define a tag que os tiros irão receber para causar dano
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

    //Controla o hit e a morte do jogador
    public void HitPlayer()
    {
        if (!isAlivePlayer || isProcessingDeath) return;
        isProcessingDeath = true;
        isAlivePlayer = false;
        if (_playerController != null)
        {
            GameObject temp = Instantiate(PrefabExplosion,_playerController.transform.position, PrefabExplosion.transform.localRotation);
            Destroy(_playerController.gameObject);
            _playerController = null;
        }
        _musicManager.FXGeral(_musicManager.FxExplosao);
        extraLifes--;
        if (extraLifes >= 0)
        {
            StartCoroutine(InstantiatePlayer());
        }
        else
        {
            //carrega cena de gameover

            _controleDeCena.StartGame("GameOver");
        }

        if (extraLifes < 0) {extraLifes = 0; }
        txtVidasExtras.text = extraLifes.ToString();
        
    }

    //vai ativar o boss assim que chegar na distância mínima
    public void AtivarBoss()
    {
        if (isAlivePlayer)
        {
            if (Vector3.Distance(_playerController.transform.position, pontoSpawnBoss.position) <= 15f)
            {
                ativarBossFinal.SetActive(true);
            }
        }
        
    }

    //Vai instanciar o player após ser morto(enquanto tiver vidas)
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
        isAlivePlayer = true;
        if (_playerController != null)
        {
            _playerController.StartCoroutine("Invencible");
        }
        else
        {
            Debug.LogError("Não foi possível encontrar o PlayerController no Prefab instanciado!");
        }
        isProcessingDeath = false;
    }
    
    //controla os efetios da cinematica no inicio
    IEnumerator IntroFase()
    {
        if (_playerController == null)
        {
            Debug.LogError("PlayerController está NULL durante IntroFase. Verifique se o jogador foi instanciado antes.");
            yield break;
        }
        _playerController.fumacaSr.color = corInicialFumaca;
        _playerController.transform.localScale = new Vector3(tamanhoInicialNave, tamanhoInicialNave, tamanhoInicialNave);
        _playerController.transform.position = posicaoInicialNave.position;

        yield return new WaitForEndOfFrame();
        isDecolar = true;

        //a cada 0.2 segundos a velocidade ira aumentar
        for (velocidadeAtual = 0; velocidadeAtual < velocidadeDecolagemMax; velocidadeAtual += 0.1f)
        {
            yield return new WaitForSeconds(0.2f);
        }
    }
    //Controle da subida e doslocamento do GameObject
    IEnumerator Subir()
    {

        float duracao = 1f;
        float tempo = 0.1f;

        Vector3 escalaInicial = Vector3.one * tamanhoInicialNave;
        Vector3 escalaFinal = Vector3.one * tamanhoOriginalNave;
            while (tempo < duracao)
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
    }

    //ativa as naves inimigas após um tempo
    IEnumerator AtivarNavesInimigas()
    {
        yield return new WaitForSeconds(40);
        ativarInimigo.SetActive(true);
    }

    //spawna os inimigos avião comum após determinado tempo
    IEnumerator SpawnHumansEnemies()
    {
        //tempo que sera spawnado inicialmente as naves humanas inimigas
        float contagemInicial = 40f;
        float contagemFinal = 50f;
        while (true)
        {
            //escolhe randomicamente o tempo entre 40 e 50 segundos
            float time = Random.Range(contagemInicial, contagemFinal);
            yield return new WaitForSeconds(10f);
            int positionEnemy1 = Random.Range(0, localSpawnEnemies.Length);
            int positionEnemy2;
            //Para não ter dois inimigos na mesma posição, utilizo o do while para ter uma posição diferente
            do
            {
                positionEnemy2 = Random.Range(0, localSpawnEnemies.Length);
            } while (positionEnemy2 == positionEnemy1);
            //instanciar inimigos em tempos diferentes
            GameObject temp1 = Instantiate(prefabEnemies, localSpawnEnemies[positionEnemy1].position, localSpawnEnemies[positionEnemy1].rotation);
            yield return new WaitForSeconds(time);
            GameObject temp2 = Instantiate(prefabEnemies, localSpawnEnemies[positionEnemy2].position, localSpawnEnemies[positionEnemy2].rotation);
          
            //a cada vez que é spawnado, o tempo total do próximo é reduzido
            contagemInicial -= 15f;
            contagemFinal -= 20f;
            
            //chegando em um limite de 10 a 20 segundos
            if (contagemInicial < 10 && contagemFinal < 20)
            {
                contagemInicial = 10f;
                contagemFinal = 20f;
            }
        }

    }

    //spawna as naves do tipo Alien
    IEnumerator SpawnEnemiesAliens()
    {
        while (true)
        {
            float time = Random.Range(90, 120);
            yield return new WaitForSeconds(time);
            int positionEnemiesAliens1 = Random.Range(0, localSpawnEnemiesAliens.Length);
            int positionEnemiesAliens2;
            do
            {
                positionEnemiesAliens2 = Random.Range(0, localSpawnEnemiesAliens.Length);
            } while (positionEnemiesAliens2 == positionEnemiesAliens1);

            GameObject temp1 = Instantiate(prefabEnemiesAliens, localSpawnEnemiesAliens[positionEnemiesAliens1].position, localSpawnEnemiesAliens[positionEnemiesAliens1].rotation);
            GameObject temp2 = Instantiate(prefabEnemiesAliens, localSpawnEnemiesAliens[positionEnemiesAliens2].position, localSpawnEnemiesAliens[positionEnemiesAliens2].rotation);

        }
    }


    public void ScoreGame()
    {
        
        pontosInimigos *= scorePlus;
        score += pontosInimigos;
        txtPontuacao.text = score.ToString();
    }

    public void LifePlus()
    {
        lifePlus++;
        extraLifes += lifePlus;
        if (extraLifes >= 3)
        {
            extraLifes = 3;
        }
        txtVidasExtras.text = extraLifes.ToString();
    }

    public void ScorePlus()
    {
        scorePlus++;
        txtMoedasExtras.text = scorePlus.ToString();
    }

    public void ShotPlus()
    {
        shotPlus++;
        txtshotsPlus.text = shotPlus.ToString();
    }

    public void ScoreGame(int pontos)
    {
        shotPlus++;
        txtshotsPlus.text = shotPlus.ToString();

    }

}
