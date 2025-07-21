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
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
}
