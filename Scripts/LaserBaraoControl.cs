using UnityEngine;

public class LaserController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public float laserLength = 10f;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);  // Início do laser
        lineRenderer.SetPosition(1, transform.position + transform.forward * laserLength);  // Final do laser
    }

    private void Update()
    {
        // Se precisar de mais lógica para atualizar o laser, adicione aqui
    }
}
