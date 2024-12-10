using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerGeral : MonoBehaviour
{
    #region Variaveis 
    // Projetil
    public GameObject projetilPrefab;
    public Transform playerArma;
    public float VelProjetil = 40f;
    public float distanciaFrames = 50f;
    public Vector3 direcao = Vector3.forward;
    public int DanoProjetil;
    public float delayEntreDisparos = 0.2f; // Delay em segundos entre os disparos
    private bool podeAtirar = true;
    [SerializeField] private AudioClip[] TiroPlayer;
    [SerializeField] private AudioClip[] Dano;
    public GameObject deathVFX;


    // Vida
    public Image hpBar;  // A imagem que representa a barra de HP
    public int HpMax = 10;  // HP m�ximo
    [SerializeField]private int HPAtual;  // HP atual
    public int quantidadeCuraPorIntervalo = 1;  // Quantidade de cura por intervalo
    public float tempoCura = 2f;  // Dura��o entre as curas (em segundos)
    private bool estaCurando = false;  // Evitar m�ltiplas corrotinas de cura
    public HUDManager hudManager;  // Refer�ncia ao HUDManager para controle de vidas
    public SistemaDeSpawnIndependente sistemaDeSpawnIndependente;  // Para rein�cio a partir de checkpoints
    public float tempoInvulnerabilidade = 2f;  // Tempo de invulnerabilidade ap�s tomar dano
    private bool estaInvulneravel = false;     // Indica se o jogador est� invulner�vel
    private bool isDead = false;  // Indica se o jogador est� morto
    private Vector3 startPosition;  // Posi��o inicial ou checkpoint do jogador
    private Quaternion startRotation;  // Rota��o inicial do jogador
    #endregion

    void Start()
    {
        HPAtual = HpMax;
        UpdateHPBar();  // Atualiza a barra de HP ao iniciar
        startPosition = transform.position;  // Define a posi��o inicial como checkpoint
        startRotation = transform.rotation;  // Salva a rota��o inicial

        if (hudManager == null)
        {
            hudManager = Object.FindAnyObjectByType<HUDManager>();

        }

        if (sistemaDeSpawnIndependente == null)
        {
            sistemaDeSpawnIndependente = Object.FindAnyObjectByType<SistemaDeSpawnIndependente>();
        }
    }
    

    private void Update()
    {
        if (Input.GetKey(KeyCode.J) && podeAtirar)
        {
            SFXManager.instance.PlaySoundFXClip(TiroPlayer, transform, 0.3f);
            StartCoroutine(DispararComDelay());
        }
    }

    #region Projetil
    private IEnumerator DispararComDelay()
    {
        podeAtirar = false;
        Disparar();
        yield return new WaitForSeconds(delayEntreDisparos);
        podeAtirar = true;
    }

    private void Disparar()
    {
        GameObject temp = Instantiate(projetilPrefab, playerArma.position, playerArma.rotation);
        StartCoroutine(Mover(temp));
    }

    private IEnumerator Mover(GameObject proj)
    {
        Vector3 destino = proj.transform.position + direcao.normalized * distanciaFrames;
        while (Vector3.Distance(proj.transform.position, destino) > 0.01f)
        {
            proj.transform.position = Vector3.MoveTowards(proj.transform.position, destino, Time.deltaTime * VelProjetil);
            yield return null;
        }
        Destroy(proj);
    }
    #endregion

    #region Heal
    public void SetInvulnerable(bool value)
    {
        estaInvulneravel = value;
    }

    public void TakeDamage(int damage)
    {
        if (estaInvulneravel || isDead) return; // Ignora se invulner�vel ou j� morto

        HPAtual -= damage;
        if (HPAtual <= 0)
        {
            HPAtual = 0;
            UpdateHPBar();

            if (!isDead) // Verifica se j� n�o est� morto
            {
                isDead = true; // Marca como morto para evitar m�ltiplas chamadas

                if (hudManager != null)
                {
                    hudManager.DecreaseLife();
                    Instantiate(deathVFX, transform.position, Quaternion.identity);
                    if (hudManager.GetCurrentLives() > 0)
                    {
                        // Reinicia o jogador a partir do checkpoint salvo
                        if (sistemaDeSpawnIndependente != null)
                        {
                            sistemaDeSpawnIndependente.RestartPlayersFromCheckpoint();
                        }
                        RestoreHealthToFull(); // Recupera o HP ap�s o respawn
                    }
                    else
                    {
                        // Se n�o houver mais vidas, carrega a cena de derrota
                        //SceneManager.LoadScene("cenaDerrotaBH");
                    }
                }

                isDead = false; // Reseta a flag ap�s lidar com a morte
            }
        }
        else
        {
            UpdateHPBar();
            StartCoroutine(AtivarInvulnerabilidade());
        }
    }

    private IEnumerator AtivarInvulnerabilidade()
    {
        estaInvulneravel = true; // Ativa o estado de invulnerabilidade
        yield return new WaitForSeconds(tempoInvulnerabilidade); // Aguarda o tempo definido
        estaInvulneravel = false; // Desativa o estado de invulnerabilidade
    }

    public void RestoreHealthToFull()
    {
        HPAtual = HpMax; // Recupera o HP ao m�ximo
        UpdateHPBar();   // Atualiza a barra de vida na UI
    }


    private void UpdateHPBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = (float)HPAtual / (float)HpMax;  // Atualiza a barra de HP
        }
        else
        {
            Debug.LogError("hpBar n�o est� atribu�do! Verifique a refer�ncia no Inspector.");
        }
    }

    #endregion

    #region Respawn no Checkpoint
    public void RespawnAtCheckpoint()
    {
        transform.position = startPosition;  // Teleporta o jogador de volta para o checkpoint
        transform.rotation = startRotation;  // Reseta a rota��o
        RestoreHealthToFull();  // Recupera o HP ao m�ximo
    }

    #endregion
    #region Heal
    public void IniciarCuraCont�nua()
    {
        if (!estaCurando) // Previne m�ltiplas corrotinas de cura
        {
            StartCoroutine(CurarCont�nuo());
        }
    }

    private IEnumerator CurarCont�nuo()
    {
        estaCurando = true;
        while (HPAtual < HpMax) // Continua a curar enquanto o HP n�o estiver cheio
        {
            PlayerHeal(quantidadeCuraPorIntervalo); // Cura o jogador
            yield return new WaitForSeconds(tempoCura); // Espera o tempo entre curas
        }
        estaCurando = false; // Finaliza a cura cont�nua quando o HP atingir o m�ximo
    }

    public void PlayerHeal(int amount)
    {
        HPAtual += amount;
        if (HPAtual > HpMax) {HPAtual = HpMax;} // Garante que o HP n�o ultrapasse o m�ximo
        UpdateHPBar(); // Atualiza a barra de vida do jogador
    }
    #endregion


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyDamage enemyDamage = other.GetComponent<EnemyDamage>();
            if (enemyDamage != null)
            {
                SFXManager.instance.PlaySoundFXClip(Dano, transform, 0.3f);
                TakeDamage(enemyDamage.damage); // Aplica dano ao jogador
            }
        }
    }
}
