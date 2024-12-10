using UnityEngine;
using System.Collections;

public class EnemyDamage : MonoBehaviour
{
    public int damage = 1;  // Dano que o inimigo causa ao jogador
    private bool isDead = false;  // Flag para verificar se o inimigo morreu

    private PlayerGeral playerGeralScript;

    private Rigidbody rb;  // Referência ao Rigidbody do projétil
    private float stopThreshold = 0.1f;  // Valor mínimo da velocidade para considerar "em movimento"

    private bool canCheckMovement = false;  // Flag para saber se pode verificar o movimento

    void Start()
    {
        // Inicializa o PlayerGeral para o inimigo
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerGeralScript = player.GetComponent<PlayerGeral>();
        }

        // Obtém o Rigidbody do projétil (assumindo que o inimigo tem um projétil associado)
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogWarning("O projétil precisa ter um Rigidbody para monitorar o movimento.");
        }

        // Chama a função para instanciar o projétil e aguardar 2 segundos
        StartCoroutine(InitializeProjectileMovement());
    }

    // Função que aguarda 2 segundos para começar a monitorar o movimento do projétil
    private IEnumerator InitializeProjectileMovement()
    {
        yield return new WaitForSeconds(2f);  // Espera 2 segundos antes de permitir a verificação do movimento
        canCheckMovement = true;  // Agora podemos verificar o movimento do projétil
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isDead)
        {
            // Causa dano ao jogador
            playerGeralScript.TakeDamage(damage);
        }

        // Lógica de colisão do projétil
        Destroy(gameObject);  // Destruir o projétil ao colidir com algo
    }

    void FixedUpdate()
    {
        if (canCheckMovement && rb != null)
        {
            // Verifica se a velocidade do projétil é baixa o suficiente para ser considerado parado
            if (rb.linearVelocity.magnitude < stopThreshold)
            {
                // Se estiver parado, destrua o projétil
                Destroy(gameObject);
            }
        }
    }

    public void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }
}
