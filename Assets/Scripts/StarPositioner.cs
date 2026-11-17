using UnityEngine;

public class StarPositioner : MonoBehaviour
{
    public float planetRadius = 15.8f;
    public int numberOfStars = 18; // Сколько звезд создать

    void Start()
    {
        // На всякий случай удаляем все старые звезды со сцены
        GameObject[] existingStars = GameObject.FindGameObjectsWithTag("Star");
        foreach (GameObject star in existingStars)
        {
            Destroy(star);
        }

        // Создаем новые звезды
        for (int i = 0; i < numberOfStars; i++)
        {
            // 1. Случайное направление на сфере
            Vector3 randomDirection = Random.onUnitSphere;
            Vector3 spawnPosition = randomDirection * planetRadius;

            // 2. Создаем сферу
            GameObject newStar = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            newStar.name = "GeneratedStar";

            // 3. СРАЗУ СТАВИМ ТЕГ И ТРИГГЕР В КОДЕ (чтобы не забыть)
            newStar.tag = "Star";
            SphereCollider col = newStar.GetComponent<SphereCollider>();
            col.isTrigger = true;

            // 4. Настраиваем внешний вид
            newStar.transform.position = spawnPosition;
            newStar.transform.localScale = new Vector3(1f, 1f, 1f);

            // Делаем желтый материал
            Renderer rend = newStar.GetComponent<Renderer>();
            rend.material = new Material(Shader.Find("Standard"));
            rend.material.color = Color.yellow;
        }
    }
}
