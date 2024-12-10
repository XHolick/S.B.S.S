using System.Collections;
using UnityEngine;

public class GerenciadorDeAtaque : MonoBehaviour
{
    [Header("Configura��es do Ataque")]
    public GameObject prefabAtaque; // Prefab do ataque
    public float velocidade = 10f; // Velocidade do movimento
    public float distanciaMaxima = 20f; // Dist�ncia m�xima antes de ser destru�do
    public float saltoDistancia = 2f; // Dist�ncia de cada salto
    public int dano = 10; // Dano causado ao player

    [Header("Pontos de Origem")]
    public Transform[] pontosDeOrigem; // Pontos de origem do ataque (�ndices configur�veis)

    private bool acaoPendente = false; // Se h� uma a��o pendente
    private int indiceAtual = -1; // �ndice atual (come�a inativo)

    void Update()
    {
        // Verifica se h� uma a��o pendente e executa
        if (acaoPendente && indiceAtual >= 0)
        {
            Disparar(indiceAtual);
            indiceAtual = -1; // Reseta o �ndice ap�s a execu��o
            acaoPendente = false; // Desativa a a��o pendente
        }
    }

    // Configura o �ndice e marca que h� uma a��o pendente
    public void SetIndice(int novoIndice)
    {
        if (novoIndice >= 0 && novoIndice < pontosDeOrigem.Length)
        {
            indiceAtual = novoIndice;
            acaoPendente = true;
        }
        else
        {
            Debug.LogWarning("�ndice inv�lido ou fora dos limites!");
        }
    }

    // Dispara o ataque a partir de um �ndice
    private void Disparar(int indice)
    {
        // Verifica se o �ndice � v�lido
        if (pontosDeOrigem == null || pontosDeOrigem.Length == 0 || indice < 0 || indice >= pontosDeOrigem.Length)
        {
            Debug.LogWarning("�ndice de origem inv�lido ou pontos n�o configurados!");
            return;
        }

        // Instancia o prefab na posi��o e rota��o do ponto de origem
        Transform pontoOrigem = pontosDeOrigem[indice];
        GameObject ataque = Instantiate(prefabAtaque, pontoOrigem.position, pontoOrigem.rotation);

        // Inicia o movimento do ataque
        StartCoroutine(MoverAtaque(ataque));
    }

    // Movimento do ataque com saltos e destrui��o no final
    private IEnumerator MoverAtaque(GameObject ataque)
    {
        float distanciaPercorrida = 0f; // Controle da dist�ncia percorrida

        while (distanciaPercorrida < distanciaMaxima)
        {
            // Calcula o pr�ximo ponto de teletransporte
            Vector3 proximaPosicao = ataque.transform.position + ataque.transform.forward * saltoDistancia;

            // Move instantaneamente (teletransporte)
            ataque.transform.position = proximaPosicao;

            // Atualiza a dist�ncia percorrida
            distanciaPercorrida += saltoDistancia;

            // Checa colis�o com o player
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

            // Espera um intervalo antes de realizar o pr�ximo salto
            yield return new WaitForSeconds(1f / velocidade);
        }

        // Destroi o ataque ap�s atingir a dist�ncia m�xima
        Destroy(ataque);
    }
}
