using UnityEngine;

public class SoltarObjeto : MonoBehaviour
{
    // Refer�ncia ao objeto que queremos soltar quando este script for destru�do
    public GameObject objetoParaSoltar;

    // Esse m�todo � chamado automaticamente quando o GameObject ou script � destru�do
    void OnDestroy()
    {
        // Verifica se o objeto para soltar ainda existe
        if (objetoParaSoltar != null)
        {
            // Desanexa o objeto
            objetoParaSoltar.transform.SetParent(null);
            Debug.Log("Objeto foi solto ao destruir o script.");
        }
    }
}
