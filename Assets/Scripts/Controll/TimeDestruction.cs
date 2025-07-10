using UnityEngine;

public class TimeDestruction : MonoBehaviour
{
    void Update()
    {
        AutoDestruir();
    }
    public void AutoDestruir()
    {
        Destroy(this.gameObject, 10f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Colidiu com: " + collision.gameObject.name + " | TAG: " + collision.tag);
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Colidiu com: " + collision.gameObject.name + " | TAG: " + collision.tag);
            Destroy(this.gameObject);
        }
    }
}
