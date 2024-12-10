using UnityEngine;

public class MissileController : MonoBehaviour
{
    private float speed;
    private float maxDistance;
    private Vector3 startPosition;

    public void SetMissileProperties(float missileSpeed, float missileRange)
    {
        speed = missileSpeed;
        maxDistance = missileRange;
        startPosition = transform.position;
    }

    void Update()
    {
        // Move o míssil para frente na direção que ele foi disparado
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Verifica se o míssil percorreu a distância máxima e o destrói
        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject); // Destroi o míssil quando atinge a distância máxima
        }
    }
}
