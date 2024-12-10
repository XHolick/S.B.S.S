using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    // Refer�ncia ao prefab do jogador (pode ser arrastado no Inspector)
    public GameObject playerPrefab;

    // Refer�ncia ao painel da tela de Game Over
    public GameObject gameOverTela;

    void Start()
    {
        // Certifique-se de que a tela de Game Over est� desativada no in�cio
        gameOverTela.SetActive(false);
    }

    void Update()
    {
        // Verifica se o playerPrefab foi destru�do (se o jogador morreu)
        if (playerPrefab == null)
        {
            // Se o jogador morreu, ativa a tela de Game Over
            gameOverTela.SetActive(true);
        }
    }
}
