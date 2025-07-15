using UnityEngine;

public class MenuOptions : MonoBehaviour
{

    [SerializeField]private GameObject menuCanvas;

    private bool menuAberto = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            AlternarMenu();
        }
    }

    public void AlternarMenu()
    {
        if (menuCanvas == null) return;

        menuAberto = !menuAberto;
        menuCanvas.SetActive(menuAberto);
        if (menuAberto == true)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        /*Cursor.visible = menuAberto;
        Cursor.lockState = menuAberto ? CursorLockMode.None : CursorLockMode.Locked;*/
    }

    

}
