using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControllMusic : MonoBehaviour
{
    private AudioSource  audioSource;
    public AudioClip[]  musicas;
    public AudioClip musicaIntro;
    public AudioClip musicaGameOver;

    private Coroutine musicaAtual;

    public Slider sliderVolumeMusica;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        audioSource = GetComponent<AudioSource>();
        
    }
    private void Start()
    {
        audioSource.volume = 0.5f;
        sliderVolumeMusica.value = audioSource.volume;
    }
    //Ira tocar a musica dependendo de qual seja a cena
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
                musicaAtual = StartCoroutine(PlaylistFase());
                break;
            case "GameOver":
                musicaAtual = StartCoroutine(MusicaLoop(musicaGameOver));
                break;
        }
    }



    IEnumerator PlaylistFase()
    {
        int index = 0;
        while (true) // loop infinito para tocar música
        {
            if (musicas.Length == 0) yield break; // se não houver música alguma ira sair.

            audioSource.clip = musicas[index];
            audioSource.Play();

            //vai esperar até o fim da música
            yield return new WaitForSeconds(audioSource.clip.length);
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
            audioSource.clip = clip;
            audioSource.Play();

            yield return new WaitForSeconds(clip.length);
        }
    }

   public void ControleDeVolume()
    {
        audioSource.volume = sliderVolumeMusica.value;
    }
}
