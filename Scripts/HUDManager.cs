using UnityEngine;
using TMPro; // Necessário para TextMeshPro
using UnityEngine.UI; // Necessário para modificar a cor da HUD
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour
{
    [Header("Configurações do Contador de Vidas")]
    public TMP_Text lifeCounterText; // Texto do contador de vidas na HUD
    public int maxRetries = 3; // Máximo de reinicializações
    private int currentLives; // Vidas restantes
    public SistemaDeSpawn sistemaDeSpawn;

    [Header("Configurações da HUD")]
    public TMP_Text shieldCounterText; // Contador do escudo
    public TMP_Text missileCounterText; // Contador de mísseis
    public Color defaultColor; // Cor padrão da HUD
    public Color shieldActiveColor; // Cor para escudo ativo
    public Color missileActiveColor; // Cor para disparos extras ativos
    public Image[] hudImages; // Elementos da HUD a serem coloridos

    [Header("Configurações de Cena")]
    public string restartSceneName = "GameOver"; // Nome da cena de reinicialização
    public SistemaDeSpawnIndependente sistemaDeSpawnIndependente;
    public GameObject playerPrefab; // Referência ao prefab do jogador 1
    public GameObject player2Prefab; // Referência ao prefab do jogador 2
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
            sistemaDeSpawn.OnWavesFinalizadas += FinalWaves; // Ao final das waves, chamar a função FinalWaves
        }

        totalVidas = maxRetries; // Inicia o total de vidas compartilhadas
        UpdateLifeCounter(); // Atualiza a HUD no início
        ResetHUDColors(); // Reseta as cores da HUD no início
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

            // Verifica se os jogadores ainda têm vidas restantes
            if (totalVidas > 0)
            {
                // Chama a função para reiniciar os jogadores a partir do checkpoint
                RestartPlayersFromCheckpoint();
            }
            else
            {
                Die(); // Caso não tenha mais vidas, vai para o Game Over
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
        SceneManager.LoadScene(restartSceneName); // Reinicia para a cena de game over ou reinício
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
        SetHUDColor(defaultColor); // Reseta para a cor padrão
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
        Debug.Log("Todas as waves foram concluídas. O jogo pode proceder para a próxima fase ou concluir.");
        // Aqui você pode adicionar a lógica do que deve acontecer após a finalização das waves
    }
}
