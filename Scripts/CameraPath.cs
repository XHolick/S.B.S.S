using UnityEngine;

public class CameraEllipsePath : MonoBehaviour
{
    public Transform pointA; // Primeiro ponto da elipse
    public Transform pointB; // Segundo ponto da elipse
    public float speed = 5f; // Velocidade de movimento
    public float height = 10f; // Altura no eixo Y
    public float fixedXRotation = 75f; // Ângulo fixo no eixo X
    public bool loop = true; // Controla se o movimento é contínuo

    private float angle = 0f; // Ângulo atual para calcular a posição ao longo da elipse
    private float semiMajorAxis; // Distância entre os pontos no eixo maior (X)
    private float semiMinorAxis; // Eixo menor (calculado como metade da distância em Z)
    private bool hasStarted = false; // Controla se o movimento já foi iniciado

    void Awake()
    {
        if (pointA == null || pointB == null)
        {
            Debug.LogError("Os pontos da elipse precisam ser definidos!");
            enabled = false;
            return;
        }

        // Calcular os eixos da elipse
        semiMajorAxis = Mathf.Abs(pointB.position.x - pointA.position.x) / 2f;
        semiMinorAxis = Mathf.Abs(pointB.position.z - pointA.position.z) / 2f;

        // Posicionar a câmera no início da trajetória
        transform.position = CalculatePositionOnEllipse(0f);

        // Ajustar a rotação inicial para que a câmera já comece na direção correta
        AdjustInitialRotation();
    }

    void Update()
    {
        // Atualizar o ângulo com base na velocidade
        angle += speed * Time.deltaTime;

        // Se loop estiver desabilitado, limitar o ângulo entre 0 e 360 graus
        if (!loop)
        {
            angle = Mathf.Clamp(angle, 0f, 360f);
        }

        // Calcular a nova posição da câmera na elipse
        Vector3 newPosition = CalculatePositionOnEllipse(angle);
        transform.position = new Vector3(newPosition.x, height, newPosition.z);

        // Calcular a rotação da câmera para olhar suavemente ao longo da trajetória
        Vector3 forwardPosition = CalculatePositionOnEllipse(angle + 1f); // Um pouco à frente
        Vector3 direction = forwardPosition - transform.position;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Vector3 eulerRotation = targetRotation.eulerAngles;
            eulerRotation.x = fixedXRotation; // Travar o eixo X
            transform.rotation = Quaternion.Euler(eulerRotation);
        }
    }

    // Ajusta a rotação inicial para que a câmera comece com a rotação correta
    private void AdjustInitialRotation()
    {
        Vector3 forwardPosition = CalculatePositionOnEllipse(angle + 1f); // Um pouco à frente
        Vector3 direction = forwardPosition - transform.position;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Vector3 eulerRotation = targetRotation.eulerAngles;
            eulerRotation.x = fixedXRotation; // Travar o eixo X
            eulerRotation.y = transform.rotation.eulerAngles.y; // Manter a rotação Y
            transform.rotation = Quaternion.Euler(eulerRotation);
        }
    }

    // Calcula a posição ao longo da elipse baseado no ângulo
    private Vector3 CalculatePositionOnEllipse(float angleInDegrees)
    {
        float angleInRadians = angleInDegrees * Mathf.Deg2Rad;
        float centerX = (pointA.position.x + pointB.position.x) / 2f; // Centro no eixo X
        float centerZ = (pointA.position.z + pointB.position.z) / 2f; // Centro no eixo Z

        // Fórmula paramétrica da elipse
        float x = centerX + semiMajorAxis * Mathf.Cos(angleInRadians);
        float z = centerZ + semiMinorAxis * Mathf.Sin(angleInRadians);

        return new Vector3(x, 0f, z);
    }

    // Desenhar a elipse no editor para facilitar o ajuste
    private void OnDrawGizmos()
    {
        if (pointA == null || pointB == null) return;

        Gizmos.color = Color.cyan;

        // Desenhar a elipse em várias etapas para visualizar o caminho
        int steps = 100;
        Vector3 previousPoint = CalculatePositionOnEllipse(0f);

        for (int i = 1; i <= steps; i++)
        {
            float angleStep = (360f / steps) * i;
            Vector3 nextPoint = CalculatePositionOnEllipse(angleStep);
            Gizmos.DrawLine(previousPoint, nextPoint);
            previousPoint = nextPoint;
        }

        // Marcar os pontos principais
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(pointA.position, 0.3f);
        Gizmos.DrawSphere(pointB.position, 0.3f);
    }
}
