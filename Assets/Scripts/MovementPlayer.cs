using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float movementSpeed = 8f;  // Velocidade de movimentação
    private Rigidbody rb;
    private Animator animator;

    // Variáveis para armazenar as teclas de controle
    [Header("Teclas de Controle")]
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;

    [Header("Configurações de Rotação")]
    public float rotationSpeed = 100f; // Velocidade de rotação
    public float maxRotation = 45f; // Rotação máxima configurável (Slider entre 0 e 360)
    private float currentRotation = 0f; // Estado da rotação atual
    private float targetRotation = 0f;  // Rotação alvo

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        MovePlayer();
        RotatePlayer();
        AnimatePlayer();
    }

    void MovePlayer()
    {
        float moveX = 0f;
        float moveZ = 0f;

        // Movimento para frente
        if (Input.GetKey(upKey))
        {
            moveZ = 1f;
        }

        // Movimento para trás
        if (Input.GetKey(downKey))
        {
            moveZ = -1f;
        }

        // Movimento para a esquerda
        if (Input.GetKey(leftKey))
        {
            moveX = -1f;
            targetRotation = -maxRotation; // Rota para a esquerda (negativo)
        }

        // Movimento para a direita
        else if (Input.GetKey(rightKey))
        {
            moveX = 1f;
            targetRotation = maxRotation; // Rota para a direita (positivo)
        }
        else
        {
            // Se não houver movimento lateral, voltar a rotação ao zero
            targetRotation = 0f;
        }

        // Vetor de direção normalizado e multiplicado pela velocidade e deltaTime
        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ).normalized * movementSpeed * Time.deltaTime;

        // Move o jogador diretamente sem rotação
        rb.MovePosition(rb.position + moveDirection);
    }

    void RotatePlayer()
    {
        // Gradualmente ajustar a rotação atual para o valor alvo
        currentRotation = Mathf.Lerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Aplicar a rotação ao jogador no eixo Y
        transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);
    }

    void AnimatePlayer()
    {
        float moveX = 0f;
        float moveZ = 0f;

        // Pegando os inputs novamente (poderia ser otimizado)
        if (Input.GetKey(upKey))
        {
            moveZ = 1f;
        }

        if (Input.GetKey(downKey))
        {
            moveZ = -1f;
        }

        if (Input.GetKey(leftKey))
        {
            moveX = -1f;
        }

        if (Input.GetKey(rightKey))
        {
            moveX = 1f;
        }

        // Atualizando o parâmetro de velocidade no Animator para que a animação reaja ao movimento
        animator.SetFloat("MoveX", moveX);
        animator.SetFloat("MoveZ", moveZ);
    }
}
