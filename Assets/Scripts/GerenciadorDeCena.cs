using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorDeCena : MonoBehaviour
{
    public SistemaDeSpawn sistemaDeSpawn; // Refer�ncia ao script SistemaDeSpawn
    public string nomeDaCenaParaCarregar; // Nome da cena que voc� deseja carregar

    void Start()
    {
        if (sistemaDeSpawn != null)
        {
            sistemaDeSpawn.OnWavesFinalizadas += TrocarDeCena;
        }
        else
        {
            Debug.LogError("SistemaDeSpawn n�o est� atribu�do no GerenciadorDeCena!");
        }
    }

    void TrocarDeCena()
    {
        Debug.Log("Todas as waves foram conclu�das. Trocando de cena para: " + nomeDaCenaParaCarregar);
        SceneManager.LoadScene(nomeDaCenaParaCarregar);
    }
}
