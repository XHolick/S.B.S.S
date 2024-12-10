using UnityEngine;
using UnityEngine.SceneManagement;

public class TrocaCenaAoClicar : MonoBehaviour
{
    public string proximaCena; // Nome da pr�xima cena
    public GameObject objetoInterativo; // Objeto 3D interativo

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
                    TrocarCena();
                }
            }
        }
    }

    void TrocarCena()
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
