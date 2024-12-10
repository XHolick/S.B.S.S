using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorDeCena : MonoBehaviour
{
    public SistemaDeSpawn sistemaDeSpawn; // Referência ao script SistemaDeSpawn
    public string nomeDaCenaParaCarregar; // Nome da cena que você deseja carregar

    void Start()
    {
        if (sistemaDeSpawn != null)
        {
            sistemaDeSpawn.OnWavesFinalizadas += TrocarDeCena;
        }
        else
        {
            Debug.LogError("SistemaDeSpawn não está atribuído no GerenciadorDeCena!");
        }
    }

    void TrocarDeCena()
    {
        Debug.Log("Todas as waves foram concluídas. Trocando de cena para: " + nomeDaCenaParaCarregar);
        SceneManager.LoadScene(nomeDaCenaParaCarregar);
    }
}
