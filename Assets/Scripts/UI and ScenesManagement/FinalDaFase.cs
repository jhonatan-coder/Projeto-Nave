using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class FinalDaFase : MonoBehaviour
{
    public TMP_Text msgFinal;
    public GameObject btnMenu;

    public string[] frases;
    public float delayMsg;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (msgFinal == null)
        {
            msgFinal = FindFirstObjectByType<TextMeshProUGUI>();
        }

        StartCoroutine(AdicionarMensagemFinal());
    }

    IEnumerator AdicionarMensagemFinal()
    {
        msgFinal.text = "";
        for (int letra = 0; letra < frases.Length; letra++)
        {
            msgFinal.text = "";
            for (int idF = 0; idF < frases[letra].Length; idF++)
            {
                msgFinal.text += frases[letra][idF];
                yield return new WaitForSeconds(delayMsg);
            }
            yield return new WaitForSeconds(delayMsg+0.2f);
        }
        btnMenu.SetActive(true);

    }

}
