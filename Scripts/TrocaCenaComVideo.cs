using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class TrocaCenaComVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Referência ao Video Player
    public string proximaCena; // Nome da próxima cena
    public GameObject objetoInterativo; // Objeto 3D interativo

    private bool videoIniciado = false;

    void Start()
    {
        // Verifica se o VideoPlayer foi configurado
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer não está configurado. Por favor, arraste o VideoPlayer para o script.");
            return;
        }

        // Configura o evento para quando o vídeo terminar
        videoPlayer.loopPointReached += TrocarCena;
    }

    void Update()
    {
        // Detecta clique no objeto 3D
        if (Input.GetMouseButtonDown(0)) // Botão esquerdo do mouse
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
            Debug.LogError("VideoPlayer não configurado. Não foi possível iniciar o vídeo.");
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
                Debug.LogError("O nome da próxima cena não foi configurado.");
            }
        }
    }
}
