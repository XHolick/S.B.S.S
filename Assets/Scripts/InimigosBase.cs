using System.Collections;
using UnityEngine;

public class InimigosBase : MonoBehaviour, IDamageable
{
    #region Variáveis

    // Variáveis de ações
    public string movimentoLoop;
    private int movementLength = 0;
    [SerializeField] private AudioClip[] morte;

    // Variáveis de vida
    [SerializeField] private int VidaMax = 5;
    private int VidaAtual;
    [SerializeField] private GameObject deathPrefab;

    // Projetil principal
    [SerializeField] private GameObject ModelEnemyProj;
    [SerializeField] private Transform enemyWeapon;
    public float VelEnemyProj = 40f;
    public float DistanciaEnemyProj = 50f;
    public Vector3 direcao = Vector3.forward;

    // Projetil alternativo
    [SerializeField] private Transform PowerUpMEsq;
    [SerializeField] private Transform PowerUpMDir;
    public float VelEnemyProjAlt = 10f;
    public float DistanciaEnemyProjAlt = 20f;
    [SerializeField] private GameObject ModelEnemyProjAlt;

    private bool isActionInProgress = false; // Controla se uma ação está em andamento
    private Vector3 velocity = Vector3.zero;
    public GerenciadorDeMortes gerenciadorDeMortes;

    #endregion

    #region Métodos Unity
    void Start()
    {
        VidaAtual = VidaMax;

        // Obtém referência ao sistema de grid de movimento
        GameObject movementP = GameObject.FindWithTag("Moviment");

    }

    void Update()
    {
        if (!isActionInProgress && movementLength < movimentoLoop.Length)
        {
            isActionInProgress = true;
            Debug.Log($"Executando ação: {movimentoLoop[movementLength]}");
            StartCoroutine(ExecutarAcao(movimentoLoop[movementLength]));
            movementLength++;
        }
        else if (movementLength >= movimentoLoop.Length)
        {
            movementLength = 0; // Reinicia o loop de ações
        }
    }


    #endregion

    #region Sistema de Vida
    public void TakeDamage(int dano)
    {
        VidaAtual -= dano;

        if (VidaAtual <= 0)
        {
            Die();
            SFXManager.instance.PlaySoundFXClip(morte, transform, 0.3f);
        }
    }

    public void Die(GameObject vfxPrefab = null)
    {
        // Se houver um VFX específico, instancie-o
        if (vfxPrefab != null)
        {
            Instantiate(vfxPrefab, transform.position, transform.rotation);
        }
        // Se houver um deathPrefab padrão, instancie-o
        else if (deathPrefab != null)
        {
            Instantiate(deathPrefab, transform.position, transform.rotation);
        }
        Destroy(gameObject);

        if (gerenciadorDeMortes != null)
        {
            gerenciadorDeMortes.IncrementarInimigosMortos();
        }
    }

    #endregion

    #region Comportamento do Inimigo


    private IEnumerator ExecutarAcao(char beatStage)
    {
        switch (beatStage)
        {
            case '0': // Parado
                yield return new WaitForSeconds(0.5f);
                break;
            case '1': // Frente
                yield return StartCoroutine(MoveToPosition(new Vector3(0, 0, -10)));
                break;
            case '2': // Trás
                yield return StartCoroutine(MoveToPosition(new Vector3(0, 0, 10)));
                break;
            case '3': // Esquerda
                yield return StartCoroutine(MoveToPosition(new Vector3(-10, 0, 0)));
                break;
            case '4': // Direita
                yield return StartCoroutine(MoveToPosition(new Vector3(10, 0, 0)));
                break;
            case '5': // Disparo simples
                Disparar();
                yield return new WaitForSeconds(0.5f);
                break;
            case '6': // Disparo alternativo
                DispararAlt();
                yield return new WaitForSeconds(0.5f);
                break;
            case '7': // Disparo combinado
                Disparar();
                DispararAlt();
                yield return new WaitForSeconds(0.5f);
                break;
            case 'L': // Loop infinito
                StartCoroutine(LoopInfinito());
                yield break; // Sai do fluxo atual
        }

        isActionInProgress = false; // Marca ação como concluída
        yield return null;
    }


    #endregion

    #region Controle de Ataques
    private IEnumerator LoopInfinito()
    {
        int loopStartIndex = movementLength; // Armazena o início do loop no movimento atual

        while (true) // Repetição infinita
        {
            for (int i = loopStartIndex; i < movimentoLoop.Length; i++)
            {
                yield return StartCoroutine(ExecutarAcao(movimentoLoop[i]));
            }
        }
    }

    private void Disparar()
    {
        GameObject temp = Instantiate(ModelEnemyProj, enemyWeapon.position, enemyWeapon.rotation);
        StartCoroutine(Mover(temp, direcao, DistanciaEnemyProj, VelEnemyProj));
    }

    private void DispararAlt()
    {
        GameObject projEsquerda = Instantiate(ModelEnemyProjAlt, PowerUpMEsq.position, PowerUpMEsq.rotation);
        GameObject projDireita = Instantiate(ModelEnemyProjAlt, PowerUpMDir.position, PowerUpMDir.rotation);

        StartCoroutine(Mover(projEsquerda, direcao, DistanciaEnemyProjAlt, VelEnemyProjAlt));
        StartCoroutine(Mover(projDireita, direcao, DistanciaEnemyProjAlt, VelEnemyProjAlt));
    }

    private IEnumerator Mover(GameObject proj, Vector3 direcao, float distancia, float velocidade)
    {
        Vector3 destino = proj.transform.position + direcao.normalized * distancia;

        while (Vector3.Distance(proj.transform.position, destino) > 0.01f)
        {
            proj.transform.position = Vector3.MoveTowards(proj.transform.position, destino, Time.deltaTime * velocidade);
            yield return null;
        }

        Destroy(proj);
    }
    private IEnumerator MoveToPosition(Vector3 deslocamento)
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + deslocamento;

        float journeyLength = Vector3.Distance(startPosition, targetPosition);
        float startTime = Time.time;

        // Usando MoveTowards para garantir que a movimentação ocorra corretamente
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            // Mover o inimigo na direção da posição de destino
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 10f);
            yield return null;
        }

        // Garante que o inimigo chegue exatamente na posição de destino
        transform.position = targetPosition;
    }


    #endregion
}