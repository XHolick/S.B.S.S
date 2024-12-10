using UnityEngine;
using System.Collections;

public class EnemyDamage : MonoBehaviour
{
    public int damage = 1;  // Dano que o inimigo causa ao jogador
    private bool isDead = false;  // Flag para verificar se o inimigo morreu

    private PlayerGeral playerGeralScript;

    private Rigidbody rb;  // Refer�ncia ao Rigidbody do proj�til
    private float stopThreshold = 0.1f;  // Valor m�nimo da velocidade para considerar "em movimento"

    private bool canCheckMovement = false;  // Flag para saber se pode verificar o movimento

    void Start()
    {
        // Inicializa o PlayerGeral para o inimigo
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerGeralScript = player.GetComponent<PlayerGeral>();
        }

        // Obt�m o Rigidbody do proj�til (assumindo que o inimigo tem um proj�til associado)
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogWarning("O proj�til precisa ter um Rigidbody para monitorar o movimento.");
        }

        // Chama a fun��o para instanciar o proj�til e aguardar 2 segundos
        StartCoroutine(InitializeProjectileMovement());
    }

    // Fun��o que aguarda 2 segundos para come�ar a monitorar o movimento do proj�til
    private IEnumerator InitializeProjectileMovement()
    {
        yield return new WaitForSeconds(2f);  // Espera 2 segundos antes de permitir a verifica��o do movimento
        canCheckMovement = true;  // Agora podemos verificar o movimento do proj�til
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isDead)
        {
            // Causa dano ao jogador
            playerGeralScript.TakeDamage(damage);
        }

        // L�gica de colis�o do proj�til
        Destroy(gameObject);  // Destruir o proj�til ao colidir com algo
    }

    void FixedUpdate()
    {
        if (canCheckMovement && rb != null)
        {
            // Verifica se a velocidade do proj�til � baixa o suficiente para ser considerado parado
            if (rb.linearVelocity.magnitude < stopThreshold)
            {
                // Se estiver parado, destrua o proj�til
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
