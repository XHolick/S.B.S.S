using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConfigWindow : MonoBehaviour
{
    public GameObject configWindow; // A janela de configuração (o painel)
    public Button openButton; // O botão que abre a janela
    private bool isDragging = false;
    private Vector2 dragOffset;

    private Vector3 originalPosition;
    private float elasticSpeed = 10f; // Velocidade do movimento elástico
    private float elasticStrength = 0.5f; // Força do movimento elástico

    void Start()
    {
        // Desabilitar a janela de configuração inicialmente
        configWindow.SetActive(false);

        // Configurar o botão para abrir a janela
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

    // Método para abrir a janela de configuração
    void OpenConfigWindow()
    {
        configWindow.SetActive(true);

        // Coloca a janela no topo da UI
        configWindow.transform.SetAsLastSibling();

        // Aplica o movimento elástico
        StartCoroutine(ElasticMove(configWindow.transform));
    }

    // Método para mover a janela de maneira fluida (elástica)
    IEnumerator ElasticMove(Transform panelTransform)
    {
        Vector3 targetPosition = Input.mousePosition;
        Vector3 currentPos = panelTransform.position;

        float time = 0f;

        // Movimento elástico
        while (time < 1f)
        {
            time += Time.deltaTime * elasticSpeed;

            // Calcular a suavização (efeito elástico)
            float smoothTime = Mathf.Sin(time * Mathf.PI * elasticStrength);
            panelTransform.position = Vector3.Lerp(currentPos, targetPosition, smoothTime);

            yield return null;
        }

        // Garantir que a posição final seja a posição do mouse
        panelTransform.position = targetPosition;
    }

    // Inicia o arraste da janela quando o botão do mouse é pressionado
    public void OnBeginDrag()
    {
        isDragging = true;
        RectTransform panelRect = configWindow.GetComponent<RectTransform>();
        dragOffset = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0) - panelRect.position;
    }

    // Interrompe o arraste da janela quando o botão do mouse é solto
    public void OnEndDrag()
    {
        isDragging = false;
    }
}
