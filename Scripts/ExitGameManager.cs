using UnityEngine;

public class ExitGameManager : MonoBehaviour
{
    public GameObject exitPanel; // Arraste o painel de saída no Inspector

    void Update()
    {
        // Verifica se a tecla ESC foi pressionada
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Ativa o painel de saída se ele não estiver ativo
            if (exitPanel != null)
            {
                exitPanel.SetActive(true);
                PauseGame(); // Pausa o jogo quando o painel de saída aparece
            }
        }
    }

    // Método chamado pelo botão "Sim" para sair do jogo
    public void ExitGame()
    {
        Application.Quit();
        print("Saiu");
    }

    // Método chamado pelo botão "Não" para fechar o painel
    public void CancelExit()
    {
        if (exitPanel != null)
        {
            exitPanel.SetActive(false);
            ResumeGame(); // Retoma o jogo quando o painel de saída é fechado
        }
    }

    // Pausa o jogo (desativa a movimentação e outras interações)
    private void PauseGame()
    {
        Time.timeScale = 0; // Pausa o tempo no jogo
    }

    // Retoma o jogo (ativa novamente as interações)
    private void ResumeGame()
    {
        Time.timeScale = 1; // Retorna o tempo ao normal
    }
}
