  using UnityEngine;

public class ObjetoDeCura : MonoBehaviour
{
    public int curaAmount = 5; // Quantidade de vida restaurada
    public float tempoDestruicao = 1f; // Tempo até a destruição do objeto após a cura
    private bool curado = false; // Flag para evitar cura múltipla


    // Quando um jogador entra em contato com o objeto de cura
    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto que colidiu tem a tag "Player"
        if (other.CompareTag("Player") && !curado)
        {
            // Tenta obter o script PlayerGeral do jogador
            PlayerGeral playerGeral = other.GetComponent<PlayerGeral>();
            print(playerGeral);
            // Se o script PlayerGeral foi encontrado, aplica a cura
            if (playerGeral != null)
            {
                playerGeral.PlayerHeal(curaAmount); // Cura o jogador
                curado = true; // Marca como curado para não curar mais de uma vez
                print("cura");
                // Inicia a destruição do objeto de cura após o tempo definido
                Destroy(gameObject, tempoDestruicao);
            }
            else
            {
                Debug.LogWarning("Script PlayerGeral não encontrado no jogador!");
            }
        }
    }
}
