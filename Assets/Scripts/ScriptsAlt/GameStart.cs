using UnityEngine;
using TMPro;  // Importa o TextMeshPro
using System.Collections;

public class GameStartManager : MonoBehaviour
{
    public TextMeshProUGUI countdownText;  // Referência ao TMP Text para mostrar a contagem
    public GameObject countdownPanel;  // Painel que contém o texto da contagem
    public float countdownDuration = 3f;  // Duração da contagem em segundos

    private bool gamePaused = true;  // Indica se o jogo está pausado
    private bool countdownStarted = false;  // Flag para impedir múltiplas contagens

    void Start()
    {
        // Pausar o jogo ao iniciar a fase
        PauseGame();

        // Exibir a mensagem inicial
        countdownText.text = "Press any key to start!";
        countdownPanel.SetActive(true);  // Ativar o painel de contagem
    }

    void Update()
    {
        // Quando o jogador pressionar uma tecla, começar a contagem
        if (gamePaused && !countdownStarted && Input.anyKeyDown)
        {
            StartCoroutine(StartCountdown());
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f;  // Pausar o tempo do jogo
        gamePaused = true;
    }

    void ResumeGame()
    {
        Time.timeScale = 1f;  // Retomar o tempo normal do jogo
        gamePaused = false;
    }

    IEnumerator StartCountdown()
    {
        countdownStarted = true;  // Impede que o coroutine seja iniciado novamente
        countdownText.text = "";  // Limpar o texto de "Press any key"

        float remainingTime = countdownDuration;

        // Contagem regressiva
        while (remainingTime > 0)
        {
            countdownText.text = remainingTime.ToString("0");  // Mostrar a contagem inteira
            yield return new WaitForSecondsRealtime(1f);  // Espera 1 segundo no tempo real (não no tempo do jogo)
            remainingTime--;
        }

        // Exibir "Start" por 2 segundos
        countdownText.text = "Start!";
        yield return new WaitForSecondsRealtime(2f);  // Aguarda 2 segundos com o texto "Start!" visível

        // Esconder o painel e desativar o texto
        countdownPanel.SetActive(false);  // Esconde o painel de contagem
        countdownText.text = "";  // Limpar o texto, caso queira que ele fique vazio
        countdownText.gameObject.SetActive(false);  // Desativa o objeto de texto para garantir que o texto seja desativado
        ResumeGame();  // Retomar o jogo
    }
}
