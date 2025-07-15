using UnityEngine;

public class FadeTransition : MonoBehaviour
{
    public Animator animator;
    private StartFly _startFly;

    private void Start()
    {
        _startFly = FindFirstObjectByType<StartFly>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            ControllAnimator();
            
        }
    }

    public void ControllAnimator()
    {
        animator.SetTrigger("FadeOut");
    }

    public void FadeComplet()
    {
        _startFly.StartGame("Menu-Inicial");

    }
}
