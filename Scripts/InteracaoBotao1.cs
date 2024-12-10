using UnityEngine;
using TMPro;

public class InteracaoBotao1 : MonoBehaviour
{
    public string textoMensagem;
    public TextMeshProUGUI telaC1;
    public TextMeshProUGUI telaC2; 
    
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
        // troca p material de brilho
        renderer.material = materialBrilho;

        // msg duas telas
        telaC1.text = textoMensagem;
        telaC2.text = textoMensagem;
    }

    private void OnMouseExit()
    {
        // volta p. material original
        renderer.material = materialOriginal;

        // remove as msgs
        telaC1.text = "";
        telaC2.text = "";
    }
}
