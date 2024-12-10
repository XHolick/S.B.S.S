using UnityEngine;

public class CloseGameOnESC : MonoBehaviour
{
    void Update()
    {
        // Verifica se a tecla ESC foi pressionada
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Fecha o jogo se estiver compilado (não funciona no editor da Unity)
            Application.Quit();

            // Log para confirmar no editor
            Debug.Log("Jogo fechado");
        }
    }
}
