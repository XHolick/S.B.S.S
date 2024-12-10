using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SistemaDeSpawnIndependente : MonoBehaviour
{
    [Header("Refer�ncia ao Sistema de Spawn")]
    public SistemaDeSpawn sistemaDeSpawn; // Refer�ncia ao script de spawn original

    [Header("Configura��es de Checkpoint")]
    public GameObject playerPrefab; // Refer�ncia ao prefab do jogador 1
    public GameObject player2Prefab; // Refer�ncia ao prefab do jogador 2

    private int waveCheckpoint = 0; // �ltima wave salva como checkpoint

    private void Start()
    {
        if (sistemaDeSpawn != null)
        {
            StartCoroutine(sistemaDeSpawn.GerenciarWaves());
        }
    }

    // Salva o �ndice da wave atual como checkpoint
    public void SaveWaveCheckpoint(int waveIndex)
    {
        waveCheckpoint = waveIndex;
        Debug.Log("Checkpoint salvo na wave: " + waveCheckpoint);
    }

    // Reinicia os jogadores a partir do checkpoint
    public void RestartPlayersFromCheckpoint()
    {
        // Reinicia o jogador 1
        if (playerPrefab != null)
        {
            PlayerGeral player1 = playerPrefab.GetComponent<PlayerGeral>();
            if (player1 != null)
            {
                player1.RespawnAtCheckpoint(); // Reinicia o player 1 corretamente
            }
        }

        // Reinicia o jogador 2 (se existir)
        if (player2Prefab != null)
        {
            PlayerTwo player2 = player2Prefab.GetComponent<PlayerTwo>();
            if (player2 != null)
            {
                player2.RespawnAtCheckpoint(); // Reinicia o player 2 corretamente
            }
        }

        // Reinicia a wave no checkpoint salvo
        if (sistemaDeSpawn != null)
        {
            sistemaDeSpawn.SetWaveAtual(waveCheckpoint);
        }
    }
}
