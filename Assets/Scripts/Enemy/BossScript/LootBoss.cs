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
            //vai escolher um loot aleat�rio
            GameObject lootEscolhido = armazemLootEnemy[Random.Range(0, armazemLootEnemy.Length)];

            //Vai criar uma posi��o aleatoria em volta do boss
            Vector3 posicaoSpawn = transform.position + (Vector3)Random.insideUnitCircle * raioDoSpawn;

            //vai instanciar o loot escolhido j� na posi��o definida
            GameObject loot = Instantiate(lootEscolhido, posicaoSpawn, Quaternion.identity);
            loot.transform.SetParent(GameObject.Find("Cenario").transform);
        }
    }
}
