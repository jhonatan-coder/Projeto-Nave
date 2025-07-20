using UnityEngine;
using TMPro;
using System.Collections;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class GameOver : MonoBehaviour
{
    public float delayType;
    public TMP_Text texto;

    //public string[] frases;
    public string frase;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        StartCoroutine(TypeLetter());
    }


    IEnumerator TypeLetter()
    {
        texto.text = "";
        for (int letra = 0; letra < frase.Length; letra++)
        {

            //escreve as frases
            texto.text += frase[letra];
            yield return new WaitForSeconds(delayType);

        }
    }
    
}
