using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float timeDestroy;
    private void Start()
    {
        Destroy(this.gameObject, timeDestroy);
    }
}
