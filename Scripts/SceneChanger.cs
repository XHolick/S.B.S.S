using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para carregar cenas

public class SceneChanger : MonoBehaviour
{
    // O nome da cena de destino (fase) que você deseja carregar
    public string nomeDaCenaDestino = "NovaFase"; // Altere para o nome da sua fase

    void Update()
    {
        // Verifica se a tecla 'Y' foi pressionada
        if (Input.GetKeyDown(KeyCode.Y))
        {
            // Carrega a cena de destino
            CarregarCena();
        }
    }

    void CarregarCena()
    {
        // Carrega a cena especificada
        SceneManager.LoadScene(nomeDaCenaDestino);
    }
}
