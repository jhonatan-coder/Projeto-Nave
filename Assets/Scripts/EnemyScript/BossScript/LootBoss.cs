using UnityEngine;

public class LootBoss : MonoBehaviour
{
    //loot boss
    public GameObject[] armazemLootEnemy;
    private int quantidadeDeLoots = 5;
    public float raioDoSpawn = 3f;

    public void SpawnLootBoss()
    {
        for (int i = 0; i < quantidadeDeLoots; i++)
        {
            //vai escolher um loot aleatório
            GameObject lootEscolhido = armazemLootEnemy[Random.Range(0, armazemLootEnemy.Length)];

            //Vai criar uma posição aleatoria em volta do boss
            Vector3 posicaoSpawn = transform.position + (Vector3)Random.insideUnitCircle * raioDoSpawn;

            //vai instanciar o loot escolhido já na posição definida
            GameObject loot = Instantiate(lootEscolhido, posicaoSpawn, Quaternion.identity);
            loot.transform.SetParent(GameObject.Find("Cenario").transform);
        }
    }
}
