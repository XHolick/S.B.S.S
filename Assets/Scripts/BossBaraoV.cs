using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class BossBaraoV : MonoBehaviour, IDamageable
{
    #region Vari�veis
    [Header("Atributos de Vida")]
    public int VidaMax;
    public int VidaAtual = 300;
    [SerializeField] private Slider barraDeVida;
    private bool[] thresholdsReached = new bool[4]; // Controla quais estados de vida foram ativados

    [Header("Comportamento do Boss")]
    private Vector3 posicaoInicial;

    [System.Serializable]
    public class BossState
    {
        [Header("Porcentagem de Vida")]
        [Range(0, 100)] public float vidaPercentual; // Porcentagem de vida

        [Header("Controle de Movimento")]
        public string controleDeMovimento; // Sequ�ncia de comandos
    }

    [SerializeField] private BossState[] estadosDeVida = new BossState[4]; // Array de estados de vida (100%, 60%, 40%, 20%)
    private string controleDeMovimento = ""; // Controle atual de movimento
    private int comandoIndex = 0; // �ndice para controlar os comandos a serem executados
    private bool isActionInProgress = false; // Para evitar sobreposi��o de a��es

    [Header("Refer�ncia de Ataques")]
    public BossAttacks bossAttacks; // Refer�ncia ao script de ataques do Boss
    #endregion

    #region M�todos Unity
    private void Start()
    {
        VidaAtual = VidaMax;

        if (barraDeVida != null)
        {
            barraDeVida.maxValue = VidaMax;
            barraDeVida.value = VidaAtual;
        }

        posicaoInicial = transform.position;

        // Inicializa o comportamento para 100% de vida
        TrocarPadrao(0);
        thresholdsReached[0] = true;

        // Inicia o loop de a��es do boss
        StartCoroutine(BossActionLoop());
    }
    #endregion

    #region Sistema de Vida
    public void TakeDamage(int dano)
    {
        VidaAtual -= dano;
        AtualizarBarraDeVida();

        if (VidaAtual <= 0)
        {
            Die();
        }
        else
        {
            AtualizarEstadoDeVida();
        }
    }

    private void AtualizarBarraDeVida()
    {
        if (barraDeVida != null)
        {
            barraDeVida.value = VidaAtual;
        }
    }

    private void AtualizarEstadoDeVida()
    {
        float porcentagem = (float)VidaAtual / VidaMax * 100;

        for (int i = 0; i < estadosDeVida.Length; i++)
        {
            if (porcentagem <= estadosDeVida[i].vidaPercentual && !thresholdsReached[i])
            {
                thresholdsReached[i] = true;
                TrocarPadrao(i);
                break;
            }
        }
    }

    private void TrocarPadrao(int index)
    {
        controleDeMovimento = estadosDeVida[index].controleDeMovimento;
        Debug.Log($"Boss atingiu {estadosDeVida[index].vidaPercentual}% da vida. Mudando comportamento!");
        comandoIndex = 0; // Reinicia o �ndice dos comandos quando troca de padr�o
    }

    private void Die()
    {
        SceneManager.LoadScene("DI�LOGO 4 - final");
        Debug.Log("Boss derrotado! Vit�ria!");
    }
    #endregion

    #region Comportamento do Boss
    private IEnumerator BossActionLoop()
    {
        while (VidaAtual > 0)
        {
            if (!string.IsNullOrEmpty(controleDeMovimento) && !isActionInProgress)
            {
                char comando = controleDeMovimento[comandoIndex % controleDeMovimento.Length];
                yield return StartCoroutine(ExecutarComando(comando));
                comandoIndex++; // Avan�a para o pr�ximo comando ap�s completar a a��o
            }
            else
            {
                yield return null; // Aguarda caso n�o haja controle de movimento configurado
            }
        }
    }

    private IEnumerator ExecutarComando(char comando)
    {
        isActionInProgress = true;

        switch (comando)
        {
            case '0':
                print("Parado");
                yield return new WaitForSeconds(1f);
                break;
            case '1':
                yield return StartCoroutine(MoveToPosition(new Vector3(0, 0, -10)));
                print("Avan�ar");
                break;
            case '2':
                yield return StartCoroutine(MoveToPosition(new Vector3(0, 0, 10)));
                print("Recuar");
                break;
            case '3':
                yield return StartCoroutine(MoveToPosition(new Vector3(-10, 0, 0)));
                print("Esquerda");
                break;
            case '4':
                yield return StartCoroutine(MoveToPosition(new Vector3(10, 0, 0)));
                print("Direita");
                break;
            case '5':
                bossAttacks.AtaqueProjetilFraco();
                print("Ataque Fraco");
                yield return new WaitForSeconds(1f);
                break;
            case '6':
                bossAttacks.AtaqueProjeteisAsa();
                print("Ataque M�dio");
                yield return new WaitForSeconds(1f);
                break;
            case '8':
                bossAttacks.AtaqueLaserFrontal();
                print("Ataque Laser");
                yield return new WaitForSeconds(1f);
                break;
            default:
                Debug.LogWarning($"Comando {comando} n�o reconhecido!");
                break;
        }

        isActionInProgress = false;
    }

    private IEnumerator MoveToPosition(Vector3 deslocamento)
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + deslocamento;

        float journeyLength = Vector3.Distance(startPosition, targetPosition);
        float startTime = Time.time;

        while (Vector3.Distance(transform.position, targetPosition) > 0.3f)
        {
            float distanceCovered = (Time.time - startTime) * 30;
            float fractionOfJourney = distanceCovered / journeyLength;

            transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);
            yield return null;
        }

        transform.position = targetPosition;
        posicaoInicial = targetPosition;
    }
    #endregion
}
