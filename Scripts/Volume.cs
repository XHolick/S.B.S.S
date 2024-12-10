using TMPro;
using UnityEngine;
using UnityEngine.UI; // Necessário para o componente Slider

public class ControleVolume : MonoBehaviour
{
    public Slider sliderVolume;      // Slider para controlar o volume
    public TextMeshProUGUI textoVolume; // Texto para mostrar o valor atual do volume

    private void Start()
    {
        // Configura o slider para refletir o volume atual do AudioListener
        sliderVolume.value = AudioListener.volume;

        // Atualiza o texto com o valor inicial do volume
        AtualizarTextoVolume();

        // Adiciona um ouvinte ao evento "OnValueChanged" do Slider
        sliderVolume.onValueChanged.AddListener(AlterarVolume);
    }

    // Função chamada quando o valor do slider é alterado
    public void AlterarVolume(float valor)
    {
        // Ajusta o volume do AudioListener com o valor do slider
        AudioListener.volume = valor;

        // Atualiza o texto com o novo valor do volume
        AtualizarTextoVolume();
    }

    // Atualiza o texto mostrando o volume atual em porcentagem
    private void AtualizarTextoVolume()
    {
        // Exibe o volume atual como uma porcentagem
        textoVolume.text = Mathf.RoundToInt(AudioListener.volume * 100) + "%";
    }
}
