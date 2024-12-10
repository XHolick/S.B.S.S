using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConfigWindow : MonoBehaviour
{
    public GameObject configWindow; // A janela de configura��o (o painel)
    public Button openButton; // O bot�o que abre a janela
    private bool isDragging = false;
    private Vector2 dragOffset;

    private Vector3 originalPosition;
    private float elasticSpeed = 10f; // Velocidade do movimento el�stico
    private float elasticStrength = 0.5f; // For�a do movimento el�stico

    void Start()
    {
        // Desabilitar a janela de configura��o inicialmente
        configWindow.SetActive(false);

        // Configurar o bot�o para abrir a janela
        openButton.onClick.AddListener(OpenConfigWindow);
    }

    void Update()
    {
        // Verifica se estamos arrastando a janela
        if (isDragging)
        {
            Vector2 mousePosition = Input.mousePosition;
            configWindow.transform.position = mousePosition - dragOffset;
        }
    }

    // M�todo para abrir a janela de configura��o
    void OpenConfigWindow()
    {
        configWindow.SetActive(true);

        // Coloca a janela no topo da UI
        configWindow.transform.SetAsLastSibling();

        // Aplica o movimento el�stico
        StartCoroutine(ElasticMove(configWindow.transform));
    }

    // M�todo para mover a janela de maneira fluida (el�stica)
    IEnumerator ElasticMove(Transform panelTransform)
    {
        Vector3 targetPosition = Input.mousePosition;
        Vector3 currentPos = panelTransform.position;

        float time = 0f;

        // Movimento el�stico
        while (time < 1f)
        {
            time += Time.deltaTime * elasticSpeed;

            // Calcular a suaviza��o (efeito el�stico)
            float smoothTime = Mathf.Sin(time * Mathf.PI * elasticStrength);
            panelTransform.position = Vector3.Lerp(currentPos, targetPosition, smoothTime);

            yield return null;
        }

        // Garantir que a posi��o final seja a posi��o do mouse
        panelTransform.position = targetPosition;
    }

    // Inicia o arraste da janela quando o bot�o do mouse � pressionado
    public void OnBeginDrag()
    {
        isDragging = true;
        RectTransform panelRect = configWindow.GetComponent<RectTransform>();
        dragOffset = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0) - panelRect.position;
    }

    // Interrompe o arraste da janela quando o bot�o do mouse � solto
    public void OnEndDrag()
    {
        isDragging = false;
    }
}
