using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public float planetRadius = 15.5f;
    public int numberOfTrees = 7;
    public GameObject treePrefab;

    void Start()
    {
        // Удаляем старые деревья со сцены (если они есть)
        GameObject[] existingTrees = GameObject.FindGameObjectsWithTag("Tree");
        foreach (GameObject tree in existingTrees)
        {
            Destroy(tree);
        }

        // Создаем новые деревья
        for (int i = 0; i < numberOfTrees; i++)
        {
            // Случайное направление на сфере
            Vector3 randomDirection = Random.onUnitSphere;
            Vector3 spawnPosition = randomDirection * planetRadius;

            if (treePrefab != null)
            {
                // Создаем дерево
                GameObject newTree = Instantiate(treePrefab, spawnPosition, Quaternion.identity);

                // Поворачиваем его "вверх" от центра шара
                newTree.transform.up = randomDirection;
            }
        }
    }
}
