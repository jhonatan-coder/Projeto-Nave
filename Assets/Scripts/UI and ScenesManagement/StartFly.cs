using UnityEngine;
using UnityEngine.SceneManagement;

public class StartFly : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

    }
    void Start()
    {
        //por estar utilizando o LoadSceneMode, n�o nescessita especificar qual o nome da cena, ser� pego automaticamente
        SceneManager.sceneLoaded += OnCenaCarregada;
        ControllMusic music = FindFirstObjectByType<ControllMusic>();
        if (music != null)
        {
            music.TrocarMusicaCena(SceneManager.GetActiveScene().name);
        }
    }

    void Update()
    {
        
    }

    //Fun��o para carregar uma fase nova ao clicar nos bot�es continua, reiniciar ou iniciar.
    public void StartGame(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }

    //vai pegar o nome da cena que estiver carregando
    void OnCenaCarregada(Scene cena, LoadSceneMode modo)
    {
        //vai carregar a musica da cena especificada
        ControllMusic musica = FindFirstObjectByType<ControllMusic>();
        if (musica != null)
        {
            musica.TrocarMusicaCena(cena.name);
        }
    }


    
}
