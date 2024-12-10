using UnityEngine;
using UnityEngine.EventSystems;

public class OptionButton : MonoBehaviour
{
    public GameObject pauseMenu;    // Refer�ncia ao menu de pausa
    public GameObject optionsBar;   // Barra de op��es que aparece na pausa
    private bool isPaused = false;

    void Start()
    {
        pauseMenu.SetActive(true);     // Garante que o menu de pausa esteja sempre ativo
        optionsBar.SetActive(false);   // A barra de op��es come�a desativada
    }

    // M�todo para pausar o jogo e exibir a barra de op��es
    public void TogglePause()
    {
        if (!isPaused)
        {
            PauseGame();
        }
    }

    // Pausa o jogo e exibe a barra de op��es
    void PauseGame()
    {
        Time.timeScale = 0;            // Pausa o tempo do jogo
        optionsBar.SetActive(true);    // Ativa a barra de op��es
        isPaused = true;
    }

    // Retoma o jogo e esconde a barra de op��es
    void ResumeGame()
    {
        Time.timeScale = 1;            // Retoma o tempo do jogo
        optionsBar.SetActive(false);   // Desativa a barra de op��es
        isPaused = false;
    }

    void Update()
    {
        // Se o jogo est� pausado e o jogador clica fora da barra de op��es, retoma o jogo
        if (isPaused && Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUIElement(optionsBar))
            {
                ResumeGame();
            }
        }
    }

    // Verifica se o ponteiro est� sobre a barra de op��es ou seus elementos filhos
    private bool IsPointerOverUIElement(GameObject target)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject == target || result.gameObject.transform.IsChildOf(target.transform))
            {
                return true;
            }
        }

        return false;
    }
}
