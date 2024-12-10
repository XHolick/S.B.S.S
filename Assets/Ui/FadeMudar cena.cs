using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeAndChangeScene : MonoBehaviour
{
    [Header("Configurações do Fade")]
    [SerializeField] private float fadeDuration = 2f; // Duração do fade-in
    [SerializeField] private string sceneToLoad = "NomeDaCena"; // Nome da cena a ser carregada

    private Image fadeImage; // Referência para o componente Image

    private void Start()
    {
        // Pega o componente Image anexado a este GameObject
        fadeImage = GetComponent<Image>();

        if (fadeImage != null)
        {
            // Começa com a imagem totalmente transparente
            Color initialColor = fadeImage.color;
            initialColor.a = 0f;
            fadeImage.color = initialColor;

            // Inicia o fade-in
            StartCoroutine(FadeInAndChangeScene());
        }
        else
        {
            Debug.LogError("Nenhuma Image foi encontrada neste GameObject.");
        }
    }

    private IEnumerator FadeInAndChangeScene()
    {
        float elapsedTime = 0f;
        Color currentColor = fadeImage.color;

        // Fade-in até a imagem ficar completamente opaca
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            currentColor.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = currentColor;
            yield return null;
        }

        // Espera 2 segundos após o fade-in estar completo
        yield return new WaitForSeconds(2f);

        // Carrega a nova cena
        SceneManager.LoadScene(sceneToLoad);
    }
}
