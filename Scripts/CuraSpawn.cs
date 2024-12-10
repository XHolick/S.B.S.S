using UnityEngine;

public class GerenciadorDeMortes : MonoBehaviour
{
    public int inimigosParaCura = 10; // Quantidade de inimigos mortos antes de dropar o item de cura
    private int inimigosMortos = 0; // Contador de inimigos mortos
    public SistemaDeSpawn sistemaDeSpawn; // Refer�ncia ao script de spawn original

    public GameObject prefabCura; // O prefab do objeto de cura
    public Transform posicaoCura; // A posi��o onde o objeto de cura ser� spawnado

    // M�todo que � chamado quando um inimigo � morto
    public void IncrementarInimigosMortos()
    {

        if (sistemaDeSpawn != null)
        {
            StartCoroutine(sistemaDeSpawn.GerenciarWaves());
        }



        if (inimigosMortos >= inimigosParaCura)
        {
            DroparObjetoDeCura(); // Dropar o objeto de cura
            inimigosMortos = 0; // Reseta o contador
        }
    }

    // M�todo para dropar o objeto de cura
    private void DroparObjetoDeCura()
    {
        if (prefabCura != null && posicaoCura != null)
        {
            Instantiate(prefabCura, posicaoCura.position, posicaoCura.rotation);
            Debug.Log("Objeto de cura foi spawnado!");
        }
        else
        {
            Debug.LogError("Prefab de cura ou posi��o n�o atribu�da no Inspector.");
        }
    }
}
