using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class TrocaCenaComVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Refer�ncia ao Video Player
    public string proximaCena; // Nome da pr�xima cena
    public GameObject objetoInterativo; // Objeto 3D interativo

    private bool videoIniciado = false;

    void Start()
    {
        // Verifica se o VideoPlayer foi configurado
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer n�o est� configurado. Por favor, arraste o VideoPlayer para o script.");
            return;
        }

        // Configura o evento para quando o v�deo terminar
        videoPlayer.loopPointReached += TrocarCena;
    }

    void Update()
    {
        // Detecta clique no objeto 3D
        if (Input.GetMouseButtonDown(0)) // Bot�o esquerdo do mouse
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == objetoInterativo)
                {
                    IniciarVideo();
                }
            }
        }
    }

    public void IniciarVideo()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Play();
            videoIniciado = true;
        }
        else
        {
            Debug.LogError("VideoPlayer n�o configurado. N�o foi poss�vel iniciar o v�deo.");
        }
    }

    void TrocarCena(VideoPlayer vp)
    {
        if (videoIniciado)
        {
            if (!string.IsNullOrEmpty(proximaCena))
            {
                SceneManager.LoadScene(proximaCena);
            }
            else
            {
                Debug.LogError("O nome da pr�xima cena n�o foi configurado.");
            }
        }
    }
}
