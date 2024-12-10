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
        // Move o m�ssil para frente na dire��o que ele foi disparado
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Verifica se o m�ssil percorreu a dist�ncia m�xima e o destr�i
        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject); // Destroi o m�ssil quando atinge a dist�ncia m�xima
        }
    }
}
