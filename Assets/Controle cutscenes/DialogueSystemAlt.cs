using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueSystemAlt : MonoBehaviour
{
    [Header("Dialogue Settings")]
    [SerializeField] private DialogueData dialogueData;  // Dados do di�logo
    [SerializeField] private float startDelay = 3.0f;    // Delay antes de come�ar o di�logo
    [SerializeField] private string nextSceneName;       // Nome da pr�xima cena ap�s o di�logo

    [Header("Dependencies")]
    [SerializeField] private DialogueUI dialogueUI;      // Interface do usu�rio do di�logo
    [SerializeField] private TypeTextAnimation typeText; // Sistema de anima��o de texto digitado

    [Header("Image Animations")]
    [SerializeField] private Image image1;               // Primeira imagem que aparece durante o di�logo
    [SerializeField] private int showImage1AtLine = 2;   // Linha em que a primeira imagem deve aparecer
    [SerializeField] private Image image2;               // Segunda imagem que aparece durante o di�logo
    [SerializeField] private int showImage2AtLine = 5;   // Linha em que a segunda imagem deve aparecer

    [Header("Final Animation or Image")]
    [SerializeField] private GameObject finalImage;      // Imagem final a ser ativada ap�s o di�logo

    private int currentLineIndex = 0;

    private enum State { Disabled, Waiting, Typing }
    private State currentState = State.Disabled;

    private void Start()
    {
        if (dialogueData == null)
        {
            Debug.LogError("DialogueData is not assigned!");
            return;
        }

        if (dialogueUI == null || typeText == null)
        {
            Debug.LogError("Missing required components: DialogueUI or TypeTextAnimation.");
            return;
        }

        // Certifique-se de que o UI e as imagens/anima��es estejam desativados no in�cio
        dialogueUI.Disable();
        if (image1 != null) image1.gameObject.SetActive(false);
        if (image2 != null) image2.gameObject.SetActive(false);
        if (finalImage != null) finalImage.SetActive(false);

        typeText.TypeFinished += OnTypingComplete;
        StartCoroutine(StartDialogueAfterDelay());
    }

    private IEnumerator StartDialogueAfterDelay()
    {
        yield return new WaitForSeconds(startDelay);
        StartDialogue();
    }

    private void Update()
    {
        if (currentState == State.Disabled) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (currentState == State.Typing)
            {
                typeText.Skip(); // Pula para o final da linha atual
                currentState = State.Waiting;
            }
            else if (currentState == State.Waiting)
            {
                ShowNextLine();
            }
        }
    }

    private void StartDialogue()
    {
        dialogueUI.Enable();
        currentLineIndex = 0;
        ShowNextLine();
    }

    private void ShowNextLine()
    {
        if (currentLineIndex >= dialogueData.talkScript.Count)
        {
            EndDialogue();
            return;
        }

        var currentLine = dialogueData.talkScript[currentLineIndex].text;
        typeText.SetFullText(currentLine);
        typeText.StartTyping();
        currentState = State.Typing;

        // Exibe a primeira imagem a partir da linha especificada
        if (currentLineIndex >= showImage1AtLine && image1 != null)
        {
            image1.gameObject.SetActive(true); // Ativa a primeira imagem
        }

        // Exibe a segunda imagem a partir da linha especificada
        if (currentLineIndex >= showImage2AtLine && image2 != null)
        {
            image2.gameObject.SetActive(true); // Ativa a segunda imagem
        }

        currentLineIndex++;
    }

    private void OnTypingComplete()
    {
        currentState = State.Waiting;
    }

    private void EndDialogue()
    {
        dialogueUI.Disable();
        currentState = State.Disabled;

        // Desativa as imagens ap�s o fim do di�logo
        if (image1 != null)
        {
            image1.gameObject.SetActive(false);
        }
        if (image2 != null)
        {
            image2.gameObject.SetActive(false);
        }

        // Ativa a imagem ou anima��o final
        if (finalImage != null)
        {
            finalImage.SetActive(true);
        }

        // Troca para a pr�xima cena, se especificado
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
