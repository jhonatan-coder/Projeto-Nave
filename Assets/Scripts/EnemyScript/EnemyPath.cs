using System.Collections;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    
    public Transform[] checkPaths;

    //public Transform enemyNav;

    public float speedEnemy;
    public float delayStop;//controla o tempo que vai de um ponto ao outro

    private int idCheckPoints;
    private bool moviment;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine("InitMoviment");
    }

    // Update is called once per frame
    void Update()
    {
        /*if (moviment)
        {
            enemyNav.position = Vector3.MoveTowards(enemyNav.position, checkPaths[idCheckPoints].position, speedEnemy * Time.deltaTime);

            if (enemyNav.position == checkPaths[idCheckPoints].position)
            {
                moviment = false;
                StartCoroutine("InitMoviment");
            }
        }*/
    }

   /* public void EnemyRoute()
    {

        int rand = Random.Range(0, checkPaths.Length);
        
    }*/

    IEnumerator InitMoviment()
    {
        idCheckPoints = Random.Range(0, checkPaths.Length);//fara o inimigo ir a um local de forma imprevisível

        /*if (idCheckPoints >= checkPaths.Length)
        {
            idCheckPoints = 0;
        }*/

        yield return new WaitForSeconds(delayStop);

        moviment = true;
    }
    
}
