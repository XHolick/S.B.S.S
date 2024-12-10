using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerTwoPrefab;  // Prefab do Player 2
    public Transform playerTwoSpawn;    // Ponto de spawn do Player 2
    private GameObject playerTwoInstance;  // Instância do Player 2
    private bool playerTwoJoined = false;
    [SerializeField] private AudioClip[] p2;
    [SerializeField] private AudioClip[] p3;


    public PlayerGeral playerOneScript;  // Referência ao script do Player 1
    public float danoReducao = 0.5f;     // Redução de dano de 50%

    void Update()
    {
        // Entrada do Player 2
        if (!playerTwoJoined && Input.GetKeyDown(KeyCode.P))  // Player 2 entra ao pressionar 'P'
        {
            SFXManager.instance.PlaySoundFXClip(p2, transform, 0.3f);
            EntrarPlayerDois();
        }

        // Saída do Player 2
        if (playerTwoJoined && Input.GetKeyDown(KeyCode.O))  // Player 2 sai ao pressionar 'O'
        {
            SFXManager.instance.PlaySoundFXClip(p3, transform, 0.3f);
            SairPlayerDois();
        }
    }

    private void EntrarPlayerDois()
    {
        playerTwoInstance = Instantiate(playerTwoPrefab, playerTwoSpawn.position, playerTwoSpawn.rotation);
        playerTwoJoined = true;

        // Reduz o dano do Player 1 e Player 2 pela metade
        if (playerOneScript != null)
        {
            playerOneScript.DanoProjetil = Mathf.FloorToInt(playerOneScript.DanoProjetil * danoReducao);  // Player 1
        }

        PlayerTwo playerTwoScript = playerTwoInstance.GetComponent<PlayerTwo>();
        if (playerTwoScript != null)
        {
            playerTwoScript.DanoProjetil = Mathf.FloorToInt(playerTwoScript.DanoProjetil * danoReducao);  // Player 2
        }
    }

    private void SairPlayerDois()
    {
        if (playerTwoInstance != null)
        {
            Destroy(playerTwoInstance);  // Remove o Player 2
            playerTwoJoined = false;     // Atualiza o estado para indicar que o Player 2 saiu
        }

        // O dano reduzido do Player 1 permanece, mesmo se o Player 2 sair
    }
}
