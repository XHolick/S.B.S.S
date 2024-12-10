using UnityEngine;

public class BotaoStart : MonoBehaviour
{
    public RotacaoTurbina turbina; 

    void OnMouseDown()
    {
        // ativa a turbina quando clica
        turbina.IniciarRotacao();
    }
}
