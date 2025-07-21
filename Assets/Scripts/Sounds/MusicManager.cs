using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    [SerializeField]private AudioSource  audioSourceMusic;
    [SerializeField]private AudioSource  audioSourceFX;
    [SerializeField]private AudioSource  audioSourceFXColetaveis;
    [SerializeField]private AudioSource  audioSourceFXTiro;
    [SerializeField]private AudioSource  audioSourceFXExplosion;

    public AudioClip[]  musicas;
    public AudioClip musicaIntro;
    public AudioClip musicaGameOver;
    public AudioClip helicopteroDecolando;
    public AudioClip helicopteroVoando;
    public AudioClip FxTiro;
    public AudioClip FxExplosao;
    //musicas para os itens
    public AudioClip moeda;
    public AudioClip vida;
    public AudioClip bomba;

    private Coroutine musicaAtual;

    public Slider sliderVolumeMusica;
    public Slider sliderVolumeFX;
    public Slider sliderVolumeFXColetaveis;
    public Slider sliderVolumeFxTiro;
    public Slider sliderVolumeFxExplosion;

    private Coroutine helioCoroutine;
    private AudioClip ultimoClipTocado;

    public AudioSource AudioSourceMusic { get => audioSourceMusic; set => audioSourceMusic = value; }
    public AudioSource AudioSourceFX { get => audioSourceFX; set => audioSourceFX = value; }
    public AudioSource AudioSourceFXTiro { get => audioSourceFXTiro; set => audioSourceFXTiro = value; }
    public AudioSource AudioSourceFXExplosion { get => audioSourceFXExplosion; set => audioSourceFXExplosion = value; }
    public AudioSource AudioSourceFXColetaveis { get => audioSourceFXColetaveis; set => audioSourceFXColetaveis = value; }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        audioSourceFX.priority = 20;
        AudioSourceFXTiro.priority = 10;

        AudioSourceMusic.volume = 1f;
        AudioSourceFX.volume = 1f;
        AudioSourceFXTiro.volume = 1f;
        AudioSourceFXExplosion.volume = 1f;


        sliderVolumeMusica.value = PlayerPrefs.GetFloat("volMusic", AudioSourceMusic.volume);
        sliderVolumeFX.value = PlayerPrefs.GetFloat("volFX", AudioSourceFX.volume);
        sliderVolumeFXColetaveis.value = PlayerPrefs.GetFloat("volFX", audioSourceFX.volume);
        sliderVolumeFxTiro.value = PlayerPrefs.GetFloat("volTiroFX", AudioSourceFXTiro.volume);
        sliderVolumeFxExplosion.value = PlayerPrefs.GetFloat("volFXExplosion", AudioSourceFXExplosion.volume);

        //Adicionar listener para salvar sempre que o valor mudar
        sliderVolumeMusica.onValueChanged.AddListener(SalvarVolMusic);

        sliderVolumeFX.onValueChanged.AddListener(SalvarVolFX);
        sliderVolumeFXColetaveis.onValueChanged.AddListener(SalvarVolFX);

        sliderVolumeFxTiro.onValueChanged.AddListener(SalvarVolTiroFx);
        sliderVolumeFxExplosion.onValueChanged.AddListener(SalvarVolFxExplosion);
        
        ControleDeVolumeFx();
        ControleDeVolumeMusic();
        ControleDeVolumeFXTiro();
        ControleDeVolumeFXExplosion();

    }

    void SalvarVolMusic(float value)
    {
        PlayerPrefs.SetFloat("volMusic", value);
        PlayerPrefs.Save();
    }

    void SalvarVolFX(float value)
    {
        PlayerPrefs.SetFloat("volFX", value);
        PlayerPrefs.Save();
    }

    void SalvarVolTiroFx(float value)
    {
        PlayerPrefs.SetFloat("volTiroFX", value);
        PlayerPrefs.Save();
    }

    void SalvarVolFxExplosion(float value)
    {
        PlayerPrefs.SetFloat("volFXExplosion", value);
        PlayerPrefs.Save();
    }


    //Ira tocar a _musicManager dependendo de qual seja a cena
    public void TrocarMusicaCena(string nomeCena)
    {
        if (musicaAtual != null)
        {
            StopCoroutine(musicaAtual);
        }
        switch (nomeCena)
        {
            case "Menu-Inicial":
                musicaAtual = StartCoroutine(MusicaLoop(musicaIntro));
                break;
            case "Fase":
                musicaAtual = StartCoroutine(PlaylistMusicFase());
                break;
            case "GameOver":
                musicaAtual = StartCoroutine(MusicaLoop(musicaGameOver));
                AudioSourceMusic.volume = 0.3f;
                PararTodosOsFx();
                break;
        }
    }


    //COROUTINE PARA MUSICAS DO JOGO
    IEnumerator PlaylistMusicFase()
    {
        int index = 0;
        while (true) // loop infinito para tocar música
        {
            if (musicas.Length == 0) yield break; // se não houver música alguma ira sair.

            AudioSourceMusic.clip = musicas[Random.Range(index, musicas.Length)];
            AudioSourceMusic.Play();

            //vai esperar até o fim da música
            yield return new WaitForSeconds(AudioSourceMusic.clip.length);
            //troca para próxima música
            index++;
            if (index >= musicas.Length)
            {
                index = 0;
            }
        }

    }
    
    //Vai iniciar as musicas do menu inicial e do GameOver
    IEnumerator MusicaLoop(AudioClip clip)
    {
        while (true) 
        {
            AudioSourceMusic.clip = clip;
            AudioSourceMusic.Play();

            yield return new WaitForSeconds(clip.length);
        }
    }
    IEnumerator HelicopteroFXLoop(AudioClip clip)
    {
        while (true)
        {
            AudioSourceFX.clip = clip;
            AudioSourceFX.Play();
            AudioSourceFX.loop = true;

            yield return new WaitForSeconds(clip.length);
        }
    }
    public void HelicopteroDecolando()
    {
        if (ultimoClipTocado == helicopteroDecolando)
        {
            return;
        }

        if (helioCoroutine != null)
        {
            StopCoroutine(helioCoroutine);
        }

        ultimoClipTocado = helicopteroDecolando;

        helioCoroutine = StartCoroutine(HelicopteroFXLoop(helicopteroDecolando));
    }
    public void HelicopteroVoando()
    {
        if (ultimoClipTocado == helicopteroVoando)
        {
            return;
        }

        if (helioCoroutine != null)
        {
            StopCoroutine(helioCoroutine);
        }

        ultimoClipTocado = helicopteroVoando;

        helioCoroutine = StartCoroutine(HelicopteroFXLoop(helicopteroVoando));
    }
    //Caso acrescentar novos fx, utilizar essa função para ativa-los
    public void FXSomColetaveis(AudioClip clip)
    {
        AudioSourceFX.PlayOneShot(clip);
    }

    private void FXTiroHelicopteroPrincipal(AudioClip clip)
    {
        AudioSourceFXTiro.clip = clip;
        AudioSourceFXTiro.Play();
    }


    public void FxTiroHelicoptero()
    {
        FXTiroHelicopteroPrincipal(FxTiro);
        
    }
    public void FxExplosaoDeath()
    {
        FXSomExplosion(FxExplosao);

    }
    private void FXSomExplosion(AudioClip clip)
    {
        AudioSourceFXExplosion.clip = clip;
        AudioSourceFXExplosion.Play();
    }

    public void ControleDeVolumeMusic()
    {
        AudioSourceMusic.volume = sliderVolumeMusica.value;
    }
    public void ControleDeVolumeFXExplosion()
    {
        AudioSourceFXExplosion.volume = sliderVolumeFxExplosion.value;
    }
    public void ControleDeVolumeFXTiro()
    {
        AudioSourceFXTiro.volume = sliderVolumeFxTiro.value;
    }

    public void ControleDeVolumeFx()
    {
        AudioSourceFX.volume = sliderVolumeFX.value;
        AudioSourceFXColetaveis.volume = sliderVolumeFX.value;
    }

    public void PararTodosOsFx()
    {
        AudioSourceFX.Stop();
        AudioSourceFXExplosion.Stop();
        AudioSourceFXTiro.Stop();
        if (ultimoClipTocado == true)
        {
            StopCoroutine(helioCoroutine);

        }
    }
    public void LigarTodosOsFX()
    {
        AudioSourceFX.Play();
        AudioSourceFXExplosion.Play();
        AudioSourceFXTiro.Play();

        StartCoroutine("helioCoroutine");

    }

}
