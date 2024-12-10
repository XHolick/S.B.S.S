using UnityEngine;
using UnityEngine.EventSystems;

public class OptionButton : MonoBehaviour
{
    public GameObject pauseMenu;    // Referência ao menu de pausa
    public GameObject optionsBar;   // Barra de opções que aparece na pausa
    private bool isPaused = false;

    void Start()
    {
        pauseMenu.SetActive(true);     // Garante que o menu de pausa esteja sempre ativo
        optionsBar.SetActive(false);   // A barra de opções começa desativada
    }

    // Método para pausar o jogo e exibir a barra de opções
    public void TogglePause()
    {
        if (!isPaused)
        {
            PauseGame();
        }
    }

    // Pausa o jogo e exibe a barra de opções
    void PauseGame()
    {
        Time.timeScale = 0;            // Pausa o tempo do jogo
        optionsBar.SetActive(true);    // Ativa a barra de opções
        isPaused = true;
    }

    // Retoma o jogo e esconde a barra de opções
    void ResumeGame()
    {
        Time.timeScale = 1;            // Retoma o tempo do jogo
        optionsBar.SetActive(false);   // Desativa a barra de opções
        isPaused = false;
    }

    void Update()
    {
        // Se o jogo está pausado e o jogador clica fora da barra de opções, retoma o jogo
        if (isPaused && Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUIElement(optionsBar))
            {
                ResumeGame();
            }
        }
    }

    // Verifica se o ponteiro está sobre a barra de opções ou seus elementos filhos
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
