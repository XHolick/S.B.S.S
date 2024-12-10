using UnityEngine;
using UnityEngine.UI; // Para o Slider
using TMPro;         // Para o TextMeshPro

public class ControleVolumeComTMP : MonoBehaviour
{
    public Slider sliderVolume;          // Slider para ajustar o volume
    public TextMeshProUGUI textoVolume;  // TMP para exibir o volume

    void Start()
    {
        // Configura o slider para refletir o volume atual
        sliderVolume.value = AudioListener.volume;

        // Atualiza o texto inicial
        AtualizarTextoVolume();

        // Adiciona um ouvinte para atualizar o volume ao mover o slider
        sliderVolume.onValueChanged.AddListener(AlterarVolume);
    }

    void AlterarVolume(float valor)
    {
        // Define o volume global
        AudioListener.volume = valor;

        // Atualiza o texto com o novo valor
        AtualizarTextoVolume();
    }

    void AtualizarTextoVolume()
    {
        // Exibe o volume em porcentagem no TextMeshPro
        //textoVolume.text = "Volume: " + Mathf.RoundToInt(AudioListener.volume * 100) + "%";
    }
}
