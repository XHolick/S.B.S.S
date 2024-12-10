using UnityEngine;
using TMPro;

public class InteracaoBotao : MonoBehaviour
{
    public string textoMensagem; 
    public TextMeshProUGUI telaStart1;
    public TextMeshProUGUI telaStart2;
    
    private Material materialOriginal;
    public Material materialBrilho;

    private new Renderer renderer;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        materialOriginal = renderer.material;
    }

    private void OnMouseEnter()
    {
        // troca p. material de brilho
        renderer.material = materialBrilho;

        // Exibir a mensagem nas duas telas
        telaStart1.text = textoMensagem;
        telaStart2.text = textoMensagem;
    }

    private void OnMouseExit()
    {
        // volta p. original
        renderer.material = materialOriginal;

        // retira as msgs
        telaStart1.text = "";
        telaStart2.text = "";
    }
}
