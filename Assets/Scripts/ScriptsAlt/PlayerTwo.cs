using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerTwo : MonoBehaviour
{
    #region Variaveis

    // Projetil
    public GameObject projetilPrefab;
    public Transform playerArma;
    public float VelProjetil = 40f;
    public float distanciaFrames = 50f;
    public Vector3 direcao = Vector3.forward;
    public int DanoProjetil = 20; // Valor padrão para o PlayerTwo
    public float delayEntreDisparos = 0.2f;
    private bool podeAtirar = true;
    [SerializeField] private AudioClip[] Tiro;
    [SerializeField] private AudioClip[] Morte;
    public GameObject deathVFX;

    // Vida
    public Image hpBar;
    public int HpMax = 10;
    private int HPAtual;
    public int quantidadeCuraPorIntervalo = 1;
    public float tempoCura = 2f;
    private bool estaCurando = false;
    public HUDManager hudManager;
    public float tempoInvulnerabilidade = 2f;
    private bool estaInvulneravel = false;
    private bool isDead = false;

    private Vector3 checkpointPosition; // Adicionado para armazenar a posição do checkpoint

    #endregion

    void Start()
    {
        if (hudManager == null)
        {
            hudManager = Object.FindAnyObjectByType<HUDManager>();

            if (hudManager == null)
            {
                Debug.LogError("HUDManager não foi encontrado na cena. Verifique se o objeto HUD está presente.");
            }
            HPAtual = HpMax;
            UpdateHPBar();
            checkpointPosition = transform.position; // Define o checkpoint inicial como a posição inicial do jogador
        }
    }

    private void Update()
    {
        // Input diferente do Player 1 para atirar
        if (Input.GetMouseButton(0) && podeAtirar)
        {
            SFXManager.instance.PlaySoundFXClip(Tiro, transform, 0.3f);
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
        if (estaInvulneravel || isDead) return;

        HPAtual -= damage;
        if (HPAtual <= 0)
        {
            HPAtual = 0;
            UpdateHPBar();

            if (!isDead)
            {
                isDead = true;
                if (hudManager != null)
                {
                    hudManager.DecreaseLife();  // Atualiza a HUD e diminui as vidas do HUDManager
                    Instantiate(deathVFX, transform.position, Quaternion.identity);
                    if (hudManager.GetCurrentLives() > 0)
                    {
                        RespawnAtCheckpoint();  // Respawn do Player 2
                    }
                    else
                    {
                        SceneManager.LoadScene("cenaDerrota");  // Cena de Game Over
                    }
                }
                isDead = false;
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
        estaInvulneravel = true;
        yield return new WaitForSeconds(tempoInvulnerabilidade);
        estaInvulneravel = false;
    }

    public void RestoreHealthToFull()
    {
        HPAtual = HpMax;
        UpdateHPBar();
    }

    public void PlayerHeal(int amount)
    {
        HPAtual += amount;
        if (HPAtual > HpMax) HPAtual = HpMax;
        UpdateHPBar();
    }

    public void IniciarCuraContínua()
    {
        if (!estaCurando)
        {
            StartCoroutine(CurarAosPoucos());
        }
    }

    private IEnumerator CurarAosPoucos()
    {
        estaCurando = true;
        float curaTotal = 0f;

        while (curaTotal < 3f)
        {
            PlayerHeal(quantidadeCuraPorIntervalo);
            curaTotal += quantidadeCuraPorIntervalo;
            yield return new WaitForSeconds(tempoCura);
        }

        estaCurando = false;
    }

    private void UpdateHPBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = (float)HPAtual / (float)HpMax;
        }
        else
        {
            Debug.LogError("hpBar não está atribuído! Verifique a referência no Inspector.");
        }
    }

    public bool IsHealing()
    {
        return estaCurando;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            SFXManager.instance.PlaySoundFXClip(Morte, transform, 0.3f);
            EnemyDamage enemyDamage = other.GetComponent<EnemyDamage>();
            if (enemyDamage != null)
            {
                TakeDamage(enemyDamage.damage);
            }
        }
    }

    #endregion

    #region Checkpoint e Respawn

    // Método para definir a posição do checkpoint
    public void SetCheckpoint(Vector3 newCheckpointPosition)
    {
        checkpointPosition = newCheckpointPosition;
    }

    // Método para respawnar no checkpoint salvo
    public void RespawnAtCheckpoint()
    {
        transform.position = checkpointPosition; // Muda a posição do jogador para o checkpoint
        RestoreHealthToFull(); // Restaura a vida ao máximo
        isDead = false; // Marca o jogador como vivo
    }

    #endregion
}
