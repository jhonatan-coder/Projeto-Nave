using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    [SerializeField]private AudioSource  audioSourceMusic;
    [SerializeField]private AudioSource  audioSourceFX;
    [Header("Musicas do jogo")]
    public AudioClip[]  musicas;
    public AudioClip musicaIntro;
    public AudioClip musicaGameOver;
    [Header("Sons de FX em Geral")]
    public AudioClip helicopteroDecolando;
    public AudioClip helicopteroVoando;
    public AudioClip FxTiro;
    public AudioClip FxExplosao;
    
    public AudioClip moeda;
    public AudioClip vida;
    public AudioClip bomba;

    private Coroutine musicaAtual;

    public Slider sliderVolumeMusica;
    public Slider sliderVolumeFX;

    private Coroutine helioCoroutine;
    private AudioClip ultimoClipTocado;

    public AudioSource AudioSourceMusic { get => audioSourceMusic; set => audioSourceMusic = value; }
    public AudioSource AudioSourceFX { get => audioSourceFX; set => audioSourceFX = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        AudioSourceMusic.volume = 1f;
        AudioSourceFX.volume = 1f;

        sliderVolumeMusica.value = PlayerPrefs.GetFloat("volMusic", AudioSourceMusic.volume);
        sliderVolumeFX.value = PlayerPrefs.GetFloat("volFX", AudioSourceFX.volume);

        //Adicionar listener para salvar sempre que o valor mudar
        sliderVolumeMusica.onValueChanged.AddListener(SalvarVolMusic);

        sliderVolumeFX.onValueChanged.AddListener(SalvarVolFX);
        
        ControleDeVolumeFx();
        ControleDeVolumeMusic();

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
                TocarMusica(musicaIntro);
                break;
            case "Fase":
                musicaAtual = StartCoroutine(PlaylistMusicFase());
                break;
            case "GameOver":
                TocarMusica(musicaGameOver);
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
    public void FXGeral(AudioClip clip)
    {
        AudioSourceFX.PlayOneShot(clip);
    }
    public void TocarMusica(AudioClip clip, bool loop = false)
    {
        while (true)
        {
            AudioSourceMusic.clip = clip;
            AudioSourceMusic.loop = loop;
            AudioSourceMusic.Play();
        }
    }
    IEnumerator HelicopteroFXLoop(AudioClip clip, bool loop = true)
    {
        while (true)
        {
            AudioSourceFX.clip = clip;
            AudioSourceFX.Play();
            AudioSourceFX.loop = loop;

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
    //Controla sons FX Geral
    public void ControleDeVolumeMusic()
    {
        AudioSourceMusic.volume = sliderVolumeMusica.value;
    }

    public void ControleDeVolumeFx()
    {
        AudioSourceFX.volume = sliderVolumeFX.value;
    }

    public void PararTodosOsFx()
    {
        AudioSourceFX.Stop();

        if (ultimoClipTocado == true)
        {
            StopCoroutine(helioCoroutine);

        }
    }
    public void LigarTodosOsFX()
    {
        AudioSourceFX.Play();

        StartCoroutine("helioCoroutine");

    }

}
