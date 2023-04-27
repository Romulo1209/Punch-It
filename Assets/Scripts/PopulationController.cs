using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopulationController : MonoBehaviour
{
    [SerializeField] private int maxPopulation;
    [SerializeField] private List<GameObject> charactersSpawned; 

    [SerializeField] private GameObject[] enemiesPrefabs;
    [SerializeField] private Transform[] points;
    [SerializeField] private Transform charactersFather;

    private void Start() {
        StartCoroutine(GeneratePopulation());
    }

    IEnumerator GeneratePopulation()
    {
        charactersSpawned.RemoveAll(s => s == null);
        if(charactersSpawned.Count < maxPopulation)
            charactersSpawned.Add(SpawnCharacter());

        yield return new WaitForSeconds(Random.Range(0, 0.5f));
        StartCoroutine(GeneratePopulation());
    }

    public GameObject SpawnCharacter()
    {
        int randomId = Random.Range(0, GameController.Instance.ActualLevel + 1);
        if (randomId >= enemiesPrefabs.Length)
            randomId = enemiesPrefabs.Length - 1;

        Vector3 randomPos = new Vector3(Random.Range(points[0].position.x, points[1].position.x), points[0].position.y, Random.Range(points[0].position.z, points[1].position.z));
        GameObject randomEnemy = enemiesPrefabs[randomId];

        GameObject Enemy = Instantiate(randomEnemy, randomPos, Quaternion.identity);
        Enemy.transform.parent = charactersFather;
        return Enemy;
    }
}
