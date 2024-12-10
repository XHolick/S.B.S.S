using UnityEngine;

public class GerenciadorDeMortes : MonoBehaviour
{
    public int inimigosParaCura = 10; // Quantidade de inimigos mortos antes de dropar o item de cura
    private int inimigosMortos = 0; // Contador de inimigos mortos
    public SistemaDeSpawn sistemaDeSpawn; // Referência ao script de spawn original

    public GameObject prefabCura; // O prefab do objeto de cura
    public Transform posicaoCura; // A posição onde o objeto de cura será spawnado

    // Método que é chamado quando um inimigo é morto
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

    // Método para dropar o objeto de cura
    private void DroparObjetoDeCura()
    {
        if (prefabCura != null && posicaoCura != null)
        {
            Instantiate(prefabCura, posicaoCura.position, posicaoCura.rotation);
            Debug.Log("Objeto de cura foi spawnado!");
        }
        else
        {
            Debug.LogError("Prefab de cura ou posição não atribuída no Inspector.");
        }
    }
}
