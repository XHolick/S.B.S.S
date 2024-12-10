using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Configura��es de Movimento")]
    public float movementSpeed = 8f;  // Velocidade de movimenta��o
    private Rigidbody rb;
    private Animator animator;

    // Vari�veis para armazenar as teclas de controle
    [Header("Teclas de Controle")]
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;

    [Header("Configura��es de Rota��o")]
    public float rotationSpeed = 100f; // Velocidade de rota��o
    public float maxRotation = 45f; // Rota��o m�xima configur�vel (Slider entre 0 e 360)
    private float currentRotation = 0f; // Estado da rota��o atual
    private float targetRotation = 0f;  // Rota��o alvo

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

        // Movimento para tr�s
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
            // Se n�o houver movimento lateral, voltar a rota��o ao zero
            targetRotation = 0f;
        }

        // Vetor de dire��o normalizado e multiplicado pela velocidade e deltaTime
        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ).normalized * movementSpeed * Time.deltaTime;

        // Move o jogador diretamente sem rota��o
        rb.MovePosition(rb.position + moveDirection);
    }

    void RotatePlayer()
    {
        // Gradualmente ajustar a rota��o atual para o valor alvo
        currentRotation = Mathf.Lerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Aplicar a rota��o ao jogador no eixo Y
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

        // Atualizando o par�metro de velocidade no Animator para que a anima��o reaja ao movimento
        animator.SetFloat("MoveX", moveX);
        animator.SetFloat("MoveZ", moveZ);
    }
}
