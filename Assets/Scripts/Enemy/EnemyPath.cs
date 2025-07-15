using System.Collections;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    public Transform[] checkPaths;
    public float speedEnemy;
    public float delayStop;

    private int idCheckPoint;
    private bool movimentando;

    void Start()
    {
        StartCoroutine(InitMoviment());
    }

    void Update()
    {
        if (movimentando)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                checkPaths[idCheckPoint].position,
                speedEnemy * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, checkPaths[idCheckPoint].position) < 0.01f)
            {
                movimentando = false;
                StartCoroutine(InitMoviment());
            }
        }
    }

    IEnumerator InitMoviment()
    {
        yield return new WaitForSeconds(delayStop);
        idCheckPoint = Random.Range(0, checkPaths.Length);
        movimentando = true;
    }
}