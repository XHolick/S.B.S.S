using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    // Referência ao prefab do jogador (pode ser arrastado no Inspector)
    public GameObject playerPrefab;

    // Referência ao painel da tela de Game Over
    public GameObject gameOverTela;

    void Start()
    {
        // Certifique-se de que a tela de Game Over está desativada no início
        gameOverTela.SetActive(false);
    }

    void Update()
    {
        // Verifica se o playerPrefab foi destruído (se o jogador morreu)
        if (playerPrefab == null)
        {
            // Se o jogador morreu, ativa a tela de Game Over
            gameOverTela.SetActive(true);
        }
    }
}
