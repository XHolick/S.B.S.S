using UnityEngine;
using System.Collections;

public class LaserDamage : MonoBehaviour
{
    public int damagePerSecond = 5;  // Quantidade de dano por segundo
    private bool isDamaging = false; // Flag para controlar se o dano est� sendo aplicado
    private bool isDead = false;     // Flag para verificar se o laser deve ser desativado

    private PlayerGeral playerGeralScript; // Refer�ncia ao script do jogador

    void Start()
    {
        // Encontra o jogador e obt�m o script PlayerGeral
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerGeralScript = player.GetComponent<PlayerGeral>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Quando o jogador entra no laser, inicia o dano cont�nuo
        if (other.CompareTag("Player") && !isDead)
        {
            isDamaging = true;
            StartCoroutine(ApplyContinuousDamage());
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Quando o jogador sai do laser, para o dano
        if (other.CompareTag("Player"))
        {
            isDamaging = false;
        }
    }

    private IEnumerator ApplyContinuousDamage()
    {
        // Aplica dano cont�nuo enquanto o jogador estiver dentro da �rea do laser
        while (isDamaging && !isDead)
        {
            playerGeralScript.TakeDamage(damagePerSecond);
            yield return new WaitForSeconds(1f); // Aplica dano a cada segundo
        }
    }

    public void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }
}
