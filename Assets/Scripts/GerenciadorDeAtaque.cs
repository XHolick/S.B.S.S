using System.Collections;
using UnityEngine;

public class GerenciadorDeAtaque : MonoBehaviour
{
    [Header("Configurações do Ataque")]
    public GameObject prefabAtaque; // Prefab do ataque
    public float velocidade = 10f; // Velocidade do movimento
    public float distanciaMaxima = 20f; // Distância máxima antes de ser destruído
    public float saltoDistancia = 2f; // Distância de cada salto
    public int dano = 10; // Dano causado ao player

    [Header("Pontos de Origem")]
    public Transform[] pontosDeOrigem; // Pontos de origem do ataque (índices configuráveis)

    private bool acaoPendente = false; // Se há uma ação pendente
    private int indiceAtual = -1; // Índice atual (começa inativo)

    void Update()
    {
        // Verifica se há uma ação pendente e executa
        if (acaoPendente && indiceAtual >= 0)
        {
            Disparar(indiceAtual);
            indiceAtual = -1; // Reseta o índice após a execução
            acaoPendente = false; // Desativa a ação pendente
        }
    }

    // Configura o índice e marca que há uma ação pendente
    public void SetIndice(int novoIndice)
    {
        if (novoIndice >= 0 && novoIndice < pontosDeOrigem.Length)
        {
            indiceAtual = novoIndice;
            acaoPendente = true;
        }
        else
        {
            Debug.LogWarning("Índice inválido ou fora dos limites!");
        }
    }

    // Dispara o ataque a partir de um índice
    private void Disparar(int indice)
    {
        // Verifica se o índice é válido
        if (pontosDeOrigem == null || pontosDeOrigem.Length == 0 || indice < 0 || indice >= pontosDeOrigem.Length)
        {
            Debug.LogWarning("Índice de origem inválido ou pontos não configurados!");
            return;
        }

        // Instancia o prefab na posição e rotação do ponto de origem
        Transform pontoOrigem = pontosDeOrigem[indice];
        GameObject ataque = Instantiate(prefabAtaque, pontoOrigem.position, pontoOrigem.rotation);

        // Inicia o movimento do ataque
        StartCoroutine(MoverAtaque(ataque));
    }

    // Movimento do ataque com saltos e destruição no final
    private IEnumerator MoverAtaque(GameObject ataque)
    {
        float distanciaPercorrida = 0f; // Controle da distância percorrida

        while (distanciaPercorrida < distanciaMaxima)
        {
            // Calcula o próximo ponto de teletransporte
            Vector3 proximaPosicao = ataque.transform.position + ataque.transform.forward * saltoDistancia;

            // Move instantaneamente (teletransporte)
            ataque.transform.position = proximaPosicao;

            // Atualiza a distância percorrida
            distanciaPercorrida += saltoDistancia;

            // Checa colisão com o player
            RaycastHit hit;
            if (Physics.Raycast(ataque.transform.position, ataque.transform.forward, out hit, saltoDistancia))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    PlayerGeral player = hit.collider.GetComponent<PlayerGeral>();
                    if (player != null)
                    {
                        player.TakeDamage(dano); // Aplica dano ao player
                        Debug.Log("Player atingido pelo ataque!");
                    }
                }
            }

            // Espera um intervalo antes de realizar o próximo salto
            yield return new WaitForSeconds(1f / velocidade);
        }

        // Destroi o ataque após atingir a distância máxima
        Destroy(ataque);
    }
}
