using UnityEngine;

public class LootEnemy : MonoBehaviour
{
    public GameObject[] lootRandom;

    public void SpawnLoot()
    {
        int indexItem = 0;
        int rand = Random.Range(0, 100);
        if (rand < 50)
        {
            rand = Random.Range(0, 100);
            if (rand > 85)
            {
                indexItem = 2; //caixa bomba
            }
            else if (rand > 50)
            {
                indexItem = 1; //caixa vida
            }
            else
            {
                indexItem = 0; // caixa moeda
            }

            GameObject temp = Instantiate(lootRandom[indexItem], transform.position, Quaternion.identity);
            temp.transform.SetParent(GameObject.Find("Cenario").transform);
        }

    }
}
