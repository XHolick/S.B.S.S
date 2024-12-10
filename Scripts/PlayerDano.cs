using UnityEngine;

public class PlayerDano : MonoBehaviour
{
    public int DanoProjetil = 1; // Quantidade de dano que o projétil causa.

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto tem a tag "Enemy"
        if (other.CompareTag("Enemy"))
        {
            // Tenta obter o componente IDamageable
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
            {
                // Aplica o dano se o componente for encontrado
                damageable.TakeDamage(DanoProjetil);
                Debug.Log("Hit em " + other.gameObject.name); // Log para depuração
                Destroy(gameObject); // Destrói o projétil após o impacto
            }
        }
    }
}