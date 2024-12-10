using UnityEngine;
using System.Collections;

public class HeliAttacks : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private Transform armaPrincipal;
    [SerializeField] private Transform[] pontosLaterais;

    [Header("Prefabs")]
    [SerializeField] private GameObject projetilFracoPrefab;
    [SerializeField] private GameObject projetilAsaPrefab;
    [SerializeField] private GameObject projetilExplosaoPrefab;

    [Header("Ataque 1: Projetil Fraco")]
    [SerializeField] private float velProjetilFraco = 10f;
    [SerializeField] private float distanciaMaximaProjFraco = 20f;

    [Header("Ataque 2: Projéteis das Asas")]
    [SerializeField] private float velProjetilAsa = 15f;
    [SerializeField] private float distanciaMaximaProjAsa = 25f;
    [SerializeField] private float intervaloEntreGrupos = 0.5f;

    [Header("Ataque 3: Míssil Explosivo")]
    [SerializeField] private float velMissil = 12f;
    [SerializeField] private float distanciaMaximaMissil = 30f;
    [SerializeField] private Transform player;
    [SerializeField] private float explosaoDelay = 0.1f;
    [SerializeField] private AudioClip[] a1;
    [SerializeField] private AudioClip[] a2;

    // Estados individuais para cada ataque
    private bool isProjetilFracoInProgress = false;
    private bool isProjeteisAsaInProgress = false;
    private bool isMissilExplosivoInProgress = false;

    #region Ataque 1: Projetil Fraco
    public void AtaqueProjetilFraco()
    {
        if (!isProjetilFracoInProgress)
        {
            SFXManager.instance.PlaySoundFXClip(a1, transform, 0.3f);
            StartCoroutine(DispararProjetilFraco());
        }
    }

    private IEnumerator DispararProjetilFraco()
    {
        isProjetilFracoInProgress = true;
        GameObject projetil = Instantiate(projetilFracoPrefab, armaPrincipal.position, Quaternion.identity);
        StartCoroutine(MoverProjetil(projetil, -Vector3.forward, velProjetilFraco, distanciaMaximaProjFraco));
        yield return null;
        isProjetilFracoInProgress = false;
    }
    #endregion

    #region Ataque 2: Projéteis das Asas
    public void AtaqueProjeteisAsa()
    {
        if (!isProjeteisAsaInProgress)
        {
            SFXManager.instance.PlaySoundFXClip(a2, transform, 0.3f);
            StartCoroutine(DispararGrupoProjeteisAsa());
        }
    }

    private IEnumerator DispararGrupoProjeteisAsa()
    {
        isProjeteisAsaInProgress = true;

        for (int i = 0; i < 2; i++) // Dispara dois grupos de projéteis
        {
            foreach (Transform ponto in pontosLaterais)
            {
                GameObject proj = Instantiate(projetilAsaPrefab, ponto.position, Quaternion.identity);
                StartCoroutine(MoverProjetil(proj, -Vector3.forward, velProjetilAsa, distanciaMaximaProjAsa));
            }
            yield return new WaitForSeconds(intervaloEntreGrupos);
        }

        isProjeteisAsaInProgress = false;
    }
    #endregion

    #region Ataque 3: Míssil Explosivo
    public void AtaqueMissilExplosivo()
    {
        if (!isMissilExplosivoInProgress)
        {
            StartCoroutine(DispararMissilExplosivo());
        }
    }

    private IEnumerator DispararMissilExplosivo()
    {
        isMissilExplosivoInProgress = true;
        GameObject missil = Instantiate(projetilFracoPrefab, armaPrincipal.position, Quaternion.identity);
        StartCoroutine(MoverMissilExplosivo(missil, velMissil, distanciaMaximaMissil));
        yield return null;
        isMissilExplosivoInProgress = false;
    }

    private IEnumerator MoverMissilExplosivo(GameObject missil, float velocidade, float distanciaMaxima)
    {
        Vector3 direcao = -Vector3.forward;
        float distanciaPercorrida = 0f;

        while (distanciaPercorrida < distanciaMaxima)
        {
            float deslocamento = velocidade * Time.deltaTime;
            missil.transform.Translate(direcao * deslocamento, Space.World);
            distanciaPercorrida += deslocamento;

            // Verifica se o míssil está alinhado com o player no eixo Z
            if (Mathf.Abs(missil.transform.position.z - player.position.z) < 0.1f)
            {
                DispararProjeteisLaterais(missil.transform.position);
                Destroy(missil);
                yield break;
            }

            yield return null;
        }

        Destroy(missil);
    }

    private void DispararProjeteisLaterais(Vector3 posicao)
    {
        // Vetores para os lados (esquerda e direita)
        Vector3[] direcoes = { Vector3.left, Vector3.right };

        foreach (Vector3 direcao in direcoes)
        {
            GameObject projExplosao = Instantiate(projetilExplosaoPrefab, posicao, Quaternion.identity);
            StartCoroutine(MoverProjetil(projExplosao, direcao, velProjetilFraco, distanciaMaximaProjFraco));
        }
    }


    #endregion

    #region Mover Projetil Genérico
    private IEnumerator MoverProjetil(GameObject projetil, Vector3 direcao, float velocidade, float distanciaMaxima)
    {
        float distanciaPercorrida = 0f;

        while (distanciaPercorrida < distanciaMaxima)
        {
            float deslocamento = velocidade * Time.deltaTime;
            projetil.transform.Translate(direcao * deslocamento, Space.World);
            distanciaPercorrida += deslocamento;
            yield return null;
        }

        Destroy(projetil);
    }
    #endregion
}
