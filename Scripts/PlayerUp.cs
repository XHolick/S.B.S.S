using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUp : MonoBehaviour
{
    #region Vari�veis de Player
    public Transform player; // Refer�ncia ao jogador
    [SerializeField] private AudioClip[] Escudo;
    [SerializeField] private AudioClip[] MissilPlayer;
    #endregion

    #region Variantes de Power-ups
    public GameObject shieldPrefab; // Prefab do escudo
    public GameObject missilePrefab; // Prefab do m�ssil
    public Transform missileSpawnPoint1; // Ponto de disparo do primeiro m�ssil
    public Transform missileSpawnPoint2; // Ponto de disparo do segundo m�ssil

    // Escudo
    public float shieldActiveTime = 5f; // Tempo que o escudo fica ativo
    public float shieldCooldown = 5f; // Tempo de recarga do escudo
    private bool shieldActive = false;
    private float shieldCooldownTimer = 0f;
    private GameObject activeShield;
    private float extraShieldTime = 0f; // Tempo extra ap�s colis�o com inimigo

    // M�sseis
    public float powerUpAttackDuration = 10f; // Tempo que o ataque extra pode ficar ativo
    public float missileCooldown = 5f; // Tempo de recarga dos m�sseis
    public float missileInterval = 2f; // Intervalo entre disparos
    public float missileSpeed = 10f; // Velocidade dos m�sseis
    public float missileRange = 100f; // Dist�ncia m�xima dos m�sseis
    private bool missileActive = false;
    private float missileCooldownTimer = 0f;

    public PlayerGeral playerGeral; // Refer�ncia ao script do jogador
    // HUD
    public HUDManager hudManager;
    #endregion

    void Start()
    {
        hudManager = Object.FindFirstObjectByType<HUDManager>();
    }

    void Update()
    {
        // Verificar o cooldown dos power-ups
        HandleCooldowns();

        // Ativar o escudo (se poss�vel)
        if (Input.GetKeyDown(KeyCode.E) && shieldCooldownTimer <= 0f && !shieldActive && !missileActive)
        {
            SFXManager.instance.PlaySoundFXClip(Escudo, transform, 0.3f);
            ActivateShield();
        }

        // Ativar m�sseis (se poss�vel)
        if (Input.GetKeyDown(KeyCode.Q) && missileCooldownTimer <= 0f && !missileActive && !shieldActive)
        {
            SFXManager.instance.PlaySoundFXClip(MissilPlayer, transform, 0.3f);
            StartCoroutine(ActivateMissileRoutine());
        }
    }

    #region Escudo
    private void ActivateShield()
{
    shieldActive = true;
    hudManager.SetHUDColor(hudManager.shieldActiveColor);
    hudManager.UpdateShieldCounter(shieldActive, shieldActiveTime);

    // Ativa a invulnerabilidade no script do jogador
    if (playerGeral != null)
    {
        playerGeral.SetInvulnerable(true);
    }

    // Instancia o escudo
    if (shieldPrefab != null && player != null)
    {
        activeShield = Instantiate(shieldPrefab, player.position, Quaternion.identity, player);
    }

    StartCoroutine(ShieldDurationCoroutine());
}

private IEnumerator ShieldDurationCoroutine()
{
    float timer = 0f;

    // Enquanto o tempo de escudo n�o expirar, conte o tempo
    while (timer < shieldActiveTime + extraShieldTime)
    {
        timer += Time.deltaTime;
        hudManager.UpdateShieldCounter(shieldActive, shieldActiveTime + extraShieldTime - timer);
        yield return null;
    }

    // Destr�i o escudo ap�s o tempo final
    if (activeShield != null)
    {
        Destroy(activeShield);
    }

    shieldActive = false;
    hudManager.ResetHUDColors();

    // Desativa a invulnerabilidade no script do jogador
    if (playerGeral != null)
    {
        playerGeral.SetInvulnerable(false);
    }

    // Inicia o cooldown do escudo e soma ao tempo extra
    shieldCooldownTimer = shieldCooldown + extraShieldTime;
    extraShieldTime = 0f; // Resetar o tempo extra ap�s o uso
}

    // M�todo chamado quando o escudo sofre uma colis�o com inimigo
    private void OnCollisionEnter(Collision collision)
    {
        // Verifica se o objeto colidido � um inimigo
        if (collision.gameObject.CompareTag("Enemy") && shieldActive)
        {
            // Adiciona tempo extra ao escudo
            extraShieldTime = 3f; // Adiciona 3 segundos ao escudo ap�s a colis�o (ajuste conforme necess�rio)
            Debug.Log("Escudo atingido! Tempo extra: " + extraShieldTime + " segundos.");
        }
    }
    #endregion

    #region M�sseis
    private IEnumerator ActivateMissileRoutine()
    {
        missileActive = true;
        float timer = 0f;

        hudManager.SetHUDColor(hudManager.missileActiveColor);
        hudManager.UpdateMissileCounter(missileActive, powerUpAttackDuration);

        // Durante a dura��o, dispara m�sseis periodicamente
        while (timer < powerUpAttackDuration)
        {
            SpawnMissiles();
            yield return new WaitForSeconds(missileInterval);
            timer += missileInterval;

            float remainingTime = powerUpAttackDuration - timer;
            hudManager.UpdateMissileCounter(missileActive, remainingTime);
        }

        missileActive = false;
        hudManager.ResetHUDColors();

        // Iniciar o cooldown dos m�sseis
        missileCooldownTimer = missileCooldown;
    }

    private void SpawnMissiles()
    {
        if (missilePrefab != null)
        {
            GameObject missile1 = Instantiate(missilePrefab, missileSpawnPoint1.position, missileSpawnPoint1.rotation);
            GameObject missile2 = Instantiate(missilePrefab, missileSpawnPoint2.position, missileSpawnPoint2.rotation);

            // Configurar os m�sseis
            MissileController missileController1 = missile1.GetComponent<MissileController>();
            MissileController missileController2 = missile2.GetComponent<MissileController>();

            if (missileController1 != null)
            {
                missileController1.SetMissileProperties(missileSpeed, missileRange);
            }

            if (missileController2 != null)
            {
                missileController2.SetMissileProperties(missileSpeed, missileRange);
            }
        }
    }
    #endregion

    #region Cooldowns
    private void HandleCooldowns()
    {
        // Cooldown do escudo
        if (shieldCooldownTimer > 0f)
        {
            shieldCooldownTimer -= Time.deltaTime;
            hudManager.UpdateShieldCounter(false, shieldCooldownTimer);
        }

        // Cooldown dos m�sseis
        if (missileCooldownTimer > 0f)
        {
            missileCooldownTimer -= Time.deltaTime;
            hudManager.UpdateMissileCounter(false, missileCooldownTimer);
        }
    }
    #endregion
}
