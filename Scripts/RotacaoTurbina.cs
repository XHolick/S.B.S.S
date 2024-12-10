using UnityEngine;

public class RotacaoTurbina : MonoBehaviour
{
    public float velocidadeRotacaoAtual = 0f;    // V.inicial
    public float velocidadeMaxima = 2000f;       // V.max
    public float aceleracao = 50f;               // aumento por seg
    private bool girar = false;
    private AudioSource somMotor;

    void Start()
    {
     
        somMotor = GetComponent<AudioSource>();
    }

    // iniciar a rotaçao
    public void IniciarRotacao()
    {
        girar = true;

        // Toca o som do motor ao iniciar a rotação
        if (somMotor != null && !somMotor.isPlaying)
        {
 
        }
    }

    void Update()
    {
        if (girar)
        {
            // Aumenta a velocidade gradualmente até o limite
            if (velocidadeRotacaoAtual < velocidadeMaxima)
            {
                velocidadeRotacaoAtual += aceleracao * Time.deltaTime;
            }

            // Aplica a rotação no eixo correto
            transform.Rotate(Vector3.up * velocidadeRotacaoAtual * Time.deltaTime);
        }
    }
}
