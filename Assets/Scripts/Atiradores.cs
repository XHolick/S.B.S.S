using System.Collections;
using UnityEngine;

public class Atirador : MonoBehaviour
{
    [Header("Configurações do Laser")]
    [SerializeField] private GameObject laserPrefab; // Prefab do laser
    [SerializeField] private float velocidadeMovimento = 1f; // Velocidade do movimento do atirador
    [SerializeField] private float offsetLaser = 1f; // Offset do laser em relação ao atirador

    [Header("Configuração do Atirador")]
    [SerializeField] private Transform atiradorParent; // Parent do atirador
    [SerializeField] private Vector3 posicaoInicial = new Vector3(0, 0, 0); // Posição inicial do atirador
    [SerializeField] private Transform player; // Referência ao jogador
    [SerializeField] private int danoAoTocarLaser = 10; // Dano ao tocar no laser

    [Header("Sistema de Movimento")]
    [SerializeField] private string controleDeMovimento = "1234"; // Padrão de movimento atual

    [System.Serializable]
    public class EstadoVidaBoss
    {
        [Header("Estado de Vida")]
        [Range(0, 100)] public float vidaPercentual; // Porcentagem de vida do boss
        public string novoPadraoMovimento; // Padrão de movimento para este estado
    }

    [SerializeField] private EstadoVidaBoss[] estadosDeVida; // Estados configuráveis de vida do Boss

    private GameObject laserAtivo; // Instância do laser
    private Transform atirador; // Transform do atirador
    private int comandoIndex = 0; // Índice atual do comando
    private bool isActionInProgress = false; // Controle de execução única por ação

    private void Start()
    {
        // Instancia o atirador na posição inicial
        atirador = Instantiate(new GameObject("Atirador"), posicaoInicial, Quaternion.identity, atiradorParent).transform;

        // Instancia o laser
        laserAtivo = Instantiate(laserPrefab, atirador.position + Vector3.up * offsetLaser, Quaternion.identity);
    }

    private void Update()
    {
        AtualizarRitmo();
    }

    private void AtualizarRitmo()
    {
        if (string.IsNullOrEmpty(controleDeMovimento) || isActionInProgress) return;

        // Executa o comando atual do padrão de movimento
        char comando = controleDeMovimento[comandoIndex % controleDeMovimento.Length];
        ExecutarComando(comando);
        comandoIndex++; // Avança para o próximo comando
    }

    private void ExecutarComando(char comando)
    {
        switch (comando)
        {
            case '1': // Avançar
                StartCoroutine(MoverAtirador(Vector3.forward * 10));
                break;
            case '2': // Recuar
                StartCoroutine(MoverAtirador(Vector3.back * 10));
                break;
            case '3': // Esquerda
                StartCoroutine(MoverAtirador(Vector3.left * 10));
                break;
            case '4': // Direita
                StartCoroutine(MoverAtirador(Vector3.right * 10));
                break;
        }
    }

    private IEnumerator MoverAtirador(Vector3 deslocamento)
    {
        isActionInProgress = true;

        Vector3 posicaoInicial = atirador.position;
        Vector3 posicaoFinal = posicaoInicial + deslocamento;

        float duracao = 1f / velocidadeMovimento; // Ajusta a duração pelo multiplicador de velocidade
        float tempo = 0;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;
            float t = tempo / duracao;

            atirador.position = Vector3.Lerp(posicaoInicial, posicaoFinal, t);

            // Atualiza a posição do laser
            laserAtivo.transform.position = atirador.position + Vector3.up * offsetLaser;

            yield return null;
        }

        atirador.position = posicaoFinal;
        laserAtivo.transform.position = atirador.position + Vector3.up * offsetLaser;

        isActionInProgress = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            // Aplica dano ao jogador
            PlayerGeral playerScript = player.GetComponent<PlayerGeral>();
            if (playerScript != null) playerScript.TakeDamage(danoAoTocarLaser);

            // Teleporta o jogador de volta para a origem
            player.position = Vector3.zero;
        }
    }
}
