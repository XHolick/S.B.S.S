using UnityEngine;
using TMPro; // Para acessar o componente TextMeshProUGUI
using System.Collections; // Necessário para usar coroutines

public class ChangeTextColorTMP : MonoBehaviour
{
    public TextMeshProUGUI textoTMP; // Arraste e solte o componente TextMeshProUGUI no Inspetor
    public float tempoEspera = 1f; // Tempo de espera entre as mudanças de cor

    void Start()
    {
        // Inicia a coroutine que muda a cor a cada 'tempoEspera' segundos
        StartCoroutine(MudarCorComTempo());
    }

    // Coroutine que muda a cor com um intervalo de tempo
    IEnumerator MudarCorComTempo()
    {
        while (true)
        {
            // Alterna entre as cores
            AlterarCorTexto();

            // Aguarda pelo tempo especificado antes de mudar novamente
            yield return new WaitForSeconds(tempoEspera);
        }
    }

    void AlterarCorTexto()
    {
        // Verifica a cor atual do texto e alterna entre as opções
        if (textoTMP.color == Color.green)
        {
            textoTMP.color = Color.red; // Se for verde, muda para vermelho
        }
        else if (textoTMP.color == Color.red)
        {
            textoTMP.color = Color.blue; // Se for vermelho, muda para azul
        }
        else
        {
            textoTMP.color = Color.green; // Caso contrário, muda para verde
        }
    }
}
