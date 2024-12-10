using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Para troca de cenas

public class BotaoInteracao : MonoBehaviour
{
    [Header("Configurações do Botão")]
    public string mensagemHover = "Ir para a Cena";
    public string nomeCena = "NomeDaCena"; // Nome da cena a ser carregada
    public TextMeshProUGUI mensagemDisplay; // Campo para exibir mensagens na interface

    [Header("Configurações Visuais")]
    public Material materialOriginal;
    public Material materialBrilho;

    private Renderer rendererAtual;

    private void Start()
    {
        // Obtém o Renderer do botão e garante que o material original seja configurado
        rendererAtual = GetComponent<Renderer>();
        if (rendererAtual != null && materialOriginal == null)
        {
            materialOriginal = rendererAtual.material;
        }
    }

    private void OnMouseEnter()
    {
        // Altera o material para o de brilho e exibe a mensagem
        if (rendererAtual != null && materialBrilho != null)
        {
            rendererAtual.material = materialBrilho;
        }

        if (mensagemDisplay != null)
        {
            mensagemDisplay.text = mensagemHover;
        }
    }

    private void OnMouseExit()
    {
        // Restaura o material original e limpa a mensagem
        if (rendererAtual != null)
        {
            rendererAtual.material = materialOriginal;
        }

        if (mensagemDisplay != null)
        {
            mensagemDisplay.text = "";
        }
    }

    private void OnMouseDown()
    {
        // Carrega a cena configurada
        if (!string.IsNullOrEmpty(nomeCena))
        {
            SceneManager.LoadScene(nomeCena);
        }
    }
}
