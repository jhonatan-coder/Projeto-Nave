
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    public Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void TransicaoBasica()
    {
        anim.SetTrigger("FadeIn");
    }
}
