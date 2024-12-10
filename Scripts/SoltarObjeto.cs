using UnityEngine;

public class SoltarObjeto : MonoBehaviour
{
    // Referência ao objeto que queremos soltar quando este script for destruído
    public GameObject objetoParaSoltar;

    // Esse método é chamado automaticamente quando o GameObject ou script é destruído
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
