using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SistemaDeSpawn : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string nome;
        public GameObject[] inimigos; // Prefabs dos inimigos
        public Vector3[] posicoesSpawn; // Posicionamento inicial dos inimigos
        public string[] acoesInimigos; // Sequência de ações para cada inimigo
        public float intervaloEntreSpawns = 1f; // Intervalo entre spawns de inimigos
    }

    public List<Wave> waves;
    public float intervaloEntreWaves = 5f;
    private int waveAtual = 0;
    public delegate void WavesFinalizadasHandler();
    public event WavesFinalizadasHandler OnWavesFinalizadas;

    public bool esperarTodosMorrerem = true;

    private List<GameObject> inimigosAtuais = new List<GameObject>();

    void Start()
    {
        StartCoroutine(GerenciarWaves());
    }

    public IEnumerator GerenciarWaves()
    {
        while (waveAtual < waves.Count)
        {
            Wave wave = waves[waveAtual];
            yield return StartCoroutine(SpawnWave(wave));

            if (esperarTodosMorrerem)
            {
                yield return new WaitUntil(() => TodosInimigosMortos());
            }
            else
            {
                yield return new WaitForSeconds(5f);
            }

            waveAtual++;
            yield return new WaitForSeconds(intervaloEntreWaves);
        }
        OnWavesFinalizadas?.Invoke();
        Debug.Log("Todas as waves foram concluídas.");
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        for (int i = 0; i < wave.inimigos.Length; i++)
        {
            GameObject inimigo = Instantiate(wave.inimigos[i], wave.posicoesSpawn[i], Quaternion.identity);
            inimigosAtuais.Add(inimigo);

            InimigosBase scriptInimigo = inimigo.GetComponent<InimigosBase>();
            if (scriptInimigo != null)
            {
                scriptInimigo.movimentoLoop = wave.acoesInimigos[i];
            }
            yield return new WaitForSeconds(wave.intervaloEntreSpawns);
        }
    }

    public void SetWaveAtual(int waveIndex)
    {
        waveAtual = waveIndex;
    }

    private bool TodosInimigosMortos()
    {
        inimigosAtuais.RemoveAll(inimigo => inimigo == null);
        return inimigosAtuais.Count == 0;
    }
}
