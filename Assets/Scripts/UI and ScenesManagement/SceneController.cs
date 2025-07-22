using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private static FadeTransition _fadeTransition;
    private MenuOptions _menuOptions;

    string cenaAtual;

    public string CenaAtual { get => cenaAtual; set => cenaAtual = value; }

    

    void Start()
    {
        //por estar utilizando o LoadSceneMode, não nescessita especificar qual o nome da cena, será pego automaticamente

        _fadeTransition = FindFirstObjectByType<FadeTransition>();
        _menuOptions = FindFirstObjectByType<MenuOptions>();
        //Toda vez que uma cena for carregada chame.
        SceneManager.sceneLoaded += OnCenaCarregada;
        CenaAtual = SceneManager.GetActiveScene().name;
        MusicManager _musicManager = FindFirstObjectByType<MusicManager>();
        if (_musicManager != null)
        {
            _musicManager.TrocarMusicaCena(CenaAtual);
        }
        if (CenaAtual == "Menu-Inicial")
        {
            _musicManager.PararTodosOsFx();
            _menuOptions.DesativarBotoes();
            StartCoroutine(DesligaAnimacaoFade());
        }
    }
    

    //Função para carregar uma fase nova ao clicar nos botôes continua, reiniciar ou iniciar.
    public void StartGame(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }
    //vai pegar o nome da cena que estiver carregando
    void OnCenaCarregada(Scene cena, LoadSceneMode modo)
    {
        //vai carregar a _musicManager da cena especificada
        MusicManager _musicManager = FindFirstObjectByType<MusicManager>();


        if (_musicManager != null)
        {
            _musicManager.TrocarMusicaCena(cena.name);
        }

        MenuOptions menu = FindFirstObjectByType<MenuOptions>();
        if (menu != null)
        {
            if (cena.name == "CenaCreditosFinais")
            {
                menu.BloquearMenu();
            }
            else
            {
                menu.LiberarMenu();
            }
        }

        if (cena.name == "Menu-Inicial")
        {
            
            _menuOptions.DesativarBotoes();
            _musicManager.PararTodosOsFx();
        }
        else
        {
            
            menu.LiberarMenu();
        }
        if (cena.name == "Fase01")
        {
            StartCoroutine(DesligaSons(_musicManager));
        }
        
        

    }

    //Remove o evento para evitar chamadas após este objeto ser destruído
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnCenaCarregada;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    //Desliga o som rapidamente ao entrar na Fase
    IEnumerator DesligaSons(MusicManager musicManager)
    {
        musicManager.PararTodosOsFx();
        yield return new WaitForSeconds(0.1f);
        musicManager.LigarTodosOsFX();
    }

    IEnumerator DesligaAnimacaoFade()
    {
        if (_fadeTransition != null)
        {
            _fadeTransition.enabled = false;
        }
        yield return new WaitForEndOfFrame();
    }

    IEnumerator LigaAnimacaoFade()
    {
        if (_fadeTransition != null)
        {
            _fadeTransition.enabled = true;
        }
        yield return new WaitForEndOfFrame();
    }

    public void FecharMenuOpcoes()
    {
        MenuOptions menu = FindFirstObjectByType<MenuOptions>();
        menu.FecharMenuOpcoesNaFase();
        menu.MenuAberto = false;        
    }
    
    public void CenaFinal()
    {
        StartCoroutine(ComecaFaseFinal());
    }

    IEnumerator ComecaFaseFinal()
    {
        yield return new WaitForSeconds(5);
        StartGame("CenaCreditosFinais");
    }
}
