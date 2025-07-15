using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemiesSides : MonoBehaviour
{

    public Transform[] positionSpawns;

    private List<int> posicoesOcupadas = new List<int>();

    public GameObject prefabEnemies;

    public int maxInimigos = 2;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine("SpawnEnemiesSide");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator SpawnEnemiesSide()
    {
        if (posicoesOcupadas.Count >= positionSpawns.Length)
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine("SpawnEnemiesSide");
            yield break;
        }
        int position;

        do
        {
            position = Random.Range(0, positionSpawns.Length);
        } while (posicoesOcupadas.Contains(position));

        posicoesOcupadas.Add(position);

        GameObject temp = Instantiate(prefabEnemies, positionSpawns[position].position, positionSpawns[position].rotation);
        if (temp.transform.position.x < 0)
        /*{
            temp.transform.rotation = Quaternion.Euler(0,0, 90);
            temp.GetComponent<IAEnemy>().grausCurva = Random.Range(0, 90);

        }
        else if(temp.transform.position.x > 0)
        {
            temp.transform.rotation = Quaternion.Euler(0, 0, -90);
            temp.GetComponent<IAEnemy>().grausCurva = Random.Range(0, -180);

        }*/
        StartCoroutine(LiberarPosicao(position, 5f));
        
        yield return new WaitForSeconds(5f);

        StartCoroutine(SpawnEnemiesSide());
    }

    IEnumerator LiberarPosicao(int index, float delay)
    {
        yield return new WaitForSeconds(delay);
        posicoesOcupadas.Remove(index);
    }
}
