using UnityEngine;

public class MenuOptions : MonoBehaviour
{

    [SerializeField]private GameObject menuCanvas;

    private bool menuAberto = false;
    private bool podeAbrirMenu = true;
    public GameObject MenuCanvas { get => menuCanvas; set => menuCanvas = value; }
    public bool MenuAberto { get => menuAberto; set => menuAberto = value; }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && podeAbrirMenu)
        {
            AlternarMenu();       
        }
    }

    public void AlternarMenu()
    {
        if (MenuCanvas == null) return;

        MenuAberto = !MenuAberto;
        MenuCanvas.SetActive(MenuAberto);

        if (MenuAberto == true)
        {         
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;

        }
    }

    public void BloquearMenu()
    {
        podeAbrirMenu = false;
        MenuAberto = false;

        if (menuCanvas != null)
        {
            menuCanvas.SetActive(false);
            MusicManager music = FindFirstObjectByType<MusicManager>();
            music.PararTodosOsFx();
            Time.timeScale = 1;
        }
    }

    public void LiberarMenu()
    {
        podeAbrirMenu = true;
    }
    
    public void FecharMenuOpcoesNaFase()
    {
        MenuCanvas.SetActive(false);
        Time.timeScale = 1;
    }
    public void DesbloquarMenu()
    {
        menuAberto = !MenuAberto;
        Time.timeScale = 1;
    }
}
