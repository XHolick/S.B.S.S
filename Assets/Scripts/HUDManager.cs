using UnityEngine;
using TMPro; // Necess�rio para TextMeshPro
using UnityEngine.UI; // Necess�rio para modificar a cor da HUD
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour
{
    [Header("Configura��es do Contador de Vidas")]
    public TMP_Text lifeCounterText; // Texto do contador de vidas na HUD
    public int maxRetries = 3; // M�ximo de reinicializa��es
    private int currentLives; // Vidas restantes
    public SistemaDeSpawn sistemaDeSpawn;

    [Header("Configura��es da HUD")]
    public TMP_Text shieldCounterText; // Contador do escudo
    public TMP_Text missileCounterText; // Contador de m�sseis
    public Color defaultColor; // Cor padr�o da HUD
    public Color shieldActiveColor; // Cor para escudo ativo
    public Color missileActiveColor; // Cor para disparos extras ativos
    public Image[] hudImages; // Elementos da HUD a serem coloridos

    [Header("Configura��es de Cena")]
    public string restartSceneName = "GameOver"; // Nome da cena de reinicializa��o
    public SistemaDeSpawnIndependente sistemaDeSpawnIndependente;
    public GameObject playerPrefab; // Refer�ncia ao prefab do jogador 1
    public GameObject player2Prefab; // Refer�ncia ao prefab do jogador 2
    public int totalVidas; // Total de vidas compartilhadas entre os dois jogadores

    private void Start()
    {
        if (sistemaDeSpawnIndependente == null)
        {
            sistemaDeSpawnIndependente = Object.FindAnyObjectByType<SistemaDeSpawnIndependente>();
        }

        // Conectar evento ao final das waves do Sistema de Spawn
        if (sistemaDeSpawn != null)
        {
            sistemaDeSpawn.OnWavesFinalizadas += FinalWaves; // Ao final das waves, chamar a fun��o FinalWaves
        }

        totalVidas = maxRetries; // Inicia o total de vidas compartilhadas
        UpdateLifeCounter(); // Atualiza a HUD no in�cio
        ResetHUDColors(); // Reseta as cores da HUD no in�cio
    }

    public void UpdateShieldCounter(bool isActive, float timer)
    {
        if (shieldCounterText != null)
            shieldCounterText.text = isActive ? Mathf.Ceil(timer).ToString() : "E";
    }

    public void UpdateMissileCounter(bool isActive, float timer)
    {
        if (missileCounterText != null)
            missileCounterText.text = isActive ? Mathf.Ceil(timer).ToString() : "Q";
    }

    public void DecreaseLife()
    {
        if (totalVidas > 0)
        {
            totalVidas--; // Diminui a vida compartilhada
            UpdateLifeCounter(); // Atualiza o contador na HUD

            // Verifica se os jogadores ainda t�m vidas restantes
            if (totalVidas > 0)
            {
                // Chama a fun��o para reiniciar os jogadores a partir do checkpoint
                RestartPlayersFromCheckpoint();
            }
            else
            {
                Die(); // Caso n�o tenha mais vidas, vai para o Game Over
            }
        }
    }

    public void UpdateLifeCounter()
    {
        if (lifeCounterText != null)
            lifeCounterText.text = totalVidas.ToString();
    }

    public int GetCurrentLives()
    {
        return currentLives;
    }

    public void Die()
    {
        SceneManager.LoadScene(restartSceneName); // Reinicia para a cena de game over ou rein�cio
    }

    public void SetHUDColor(Color color)
    {
        foreach (var element in hudImages)
        {
            if (element != null)
                element.color = color;
        }
    }

    public void ResetHUDColors()
    {
        SetHUDColor(defaultColor); // Reseta para a cor padr�o
    }

    public void RestartPlayersFromCheckpoint()
    {
        if (sistemaDeSpawnIndependente != null)
        {
            sistemaDeSpawnIndependente.RestartPlayersFromCheckpoint(); // Chama o sistema de spawn independente para reiniciar
        }
    }

    private void FinalWaves()
    {
        Debug.Log("Todas as waves foram conclu�das. O jogo pode proceder para a pr�xima fase ou concluir.");
        // Aqui voc� pode adicionar a l�gica do que deve acontecer ap�s a finaliza��o das waves
    }
}
