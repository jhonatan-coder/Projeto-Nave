using UnityEngine;

public class FadeTransition : MonoBehaviour
{
    public Animator animator;
    private SceneController _startFly;

    private void Start()
    {
        _startFly = FindFirstObjectByType<SceneController>();
        
    }
    void Update()
    {
        ControllAnimator();
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
