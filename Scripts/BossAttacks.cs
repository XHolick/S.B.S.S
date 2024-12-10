using UnityEngine;
using System.Collections;

public class BossAttacks : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private Transform armaPrincipal;  // Local de disparo da arma principal
    [SerializeField] private Transform[] pontosLaterais;  // Pontos para disparos laterais (esquerda/direita)

    [Header("Prefabs")]
    [SerializeField] private GameObject projetilFracoPrefab; // Prefab de projétil fraco
    [SerializeField] private GameObject projetilAsaPrefab;   // Prefab de projétil da asa
    [SerializeField] private GameObject laserPrefab;         // Prefab do laser
    [SerializeField] private GameObject avisoPrefab;         // Prefab de aviso antes do disparo

    [Header("Ataque 1: Projetil Fraco")]
    [SerializeField] private float velProjetilFraco = 10f;  // Velocidade do projétil fraco
    [SerializeField] private float distanciaMaximaProjFraco = 20f;  // Distância máxima do projétil fraco

    [Header("Ataque 2: Projéteis das Asas")]
    [SerializeField] private float velProjetilAsa = 15f;  // Velocidade dos projéteis das asas
    [SerializeField] private float distanciaMaximaProjAsa = 25f;  // Distância máxima dos projéteis das asas
    [SerializeField] private float intervaloEntreGrupos = 0.5f;  // Intervalo entre grupos de disparo

    [Header("Laser Principal (Frontal)")]
    [SerializeField] private float duracaoLaser = 3f;  // Duração do laser principal
    [SerializeField] private float distanciaSpawnLaser = 10f;  // Distância de spawn do laser da arma
    [SerializeField] private float tempoAviso = 2f;  // Tempo de aviso antes do disparo

    [Header("Configurações de Escala do Laser")]
    [SerializeField] private Vector3 escalaInicialLaser = new Vector3(1f, 1f, 1f);  // Escala inicial do laser
    [SerializeField] private Vector3 escalaFinalLaser = new Vector3(2f, 1f, 1f);    // Escala final do laser (somente no eixo X)


    private bool isActionInProgress = false;

    #region Ataque 1: Projetil Fraco
    public void AtaqueProjetilFraco()
    {
        if (!isActionInProgress)
        {
            StartCoroutine(DispararProjetilFraco());
        }
    }

    private IEnumerator DispararProjetilFraco()
    {
        isActionInProgress = true;
        GameObject projetil = Instantiate(projetilFracoPrefab, armaPrincipal.position, Quaternion.identity);
        StartCoroutine(MoverProjetil(projetil, -Vector3.forward, velProjetilFraco, distanciaMaximaProjFraco)); // Mudança para -Vector3.forward
        yield return null;
        isActionInProgress = false;
    }

    #endregion

    #region Ataque 2: Projéteis das Asas
    public void AtaqueProjeteisAsa()
    {
        if (!isActionInProgress)
        {
            StartCoroutine(DispararGrupoProjeteisAsa());
        }
    }

    private IEnumerator DispararGrupoProjeteisAsa()
    {
        isActionInProgress = true;

        for (int i = 0; i < 2; i++)  // Dispara dois grupos de projéteis
        {
            foreach (Transform ponto in pontosLaterais)
            {
                GameObject proj = Instantiate(projetilAsaPrefab, ponto.position, Quaternion.identity);
                StartCoroutine(MoverProjetil(proj, -Vector3.forward, velProjetilAsa, distanciaMaximaProjAsa)); // Mudança para -Vector3.forward
            }
            yield return new WaitForSeconds(intervaloEntreGrupos);
        }

        isActionInProgress = false;
    }

    #endregion

    public void AtaqueLaserFrontal()
    {
        if (!isActionInProgress)
        {
            StartCoroutine(DispararLaserFrontal());
        }
    }

    private IEnumerator DispararLaserFrontal()
    {
        isActionInProgress = true;

        // Exibe o aviso antes do disparo
        GameObject aviso = Instantiate(avisoPrefab, armaPrincipal.position, Quaternion.identity);
        yield return new WaitForSeconds(tempoAviso);
        Destroy(aviso);

        // Instancia o laser na posição configurada à frente da arma
        Vector3 laserSpawnPos = armaPrincipal.position + armaPrincipal.forward * distanciaSpawnLaser;
        GameObject laser = Instantiate(laserPrefab, laserSpawnPos, Quaternion.identity);

        // Define a escala inicial do laser
        laser.transform.localScale = escalaInicialLaser;

        float tempoAtual = 0f;

        // Aumenta a escala do laser ao longo do tempo, mas apenas no eixo X
        while (tempoAtual < duracaoLaser)
        {
            // Altera a escala apenas no eixo X
            Vector3 novaEscala = laser.transform.localScale;
            novaEscala.x = Mathf.Lerp(escalaInicialLaser.x, escalaFinalLaser.x, tempoAtual / duracaoLaser);  // Cresce para o eixo X

            laser.transform.localScale = novaEscala;

            tempoAtual += Time.deltaTime;
            yield return null;
        }

        // Após a duração, o laser atinge a escala final no eixo X
        laser.transform.localScale = new Vector3(escalaFinalLaser.x, laser.transform.localScale.y, laser.transform.localScale.z);

        Destroy(laser);  // Destroi o laser após a duração
        isActionInProgress = false;
    }

    #region Movimento de Projetil
    // Usa Rigidbody.MovePosition para mover projéteis
    private IEnumerator MoverProjetil(GameObject projetil, Vector3 direcao, float velocidade, float distanciaMaxima)
    {
        Rigidbody rb = projetil.GetComponent<Rigidbody>();
        Vector3 posicaoInicial = projetil.transform.position;

        while (Vector3.Distance(projetil.transform.position, posicaoInicial) < distanciaMaxima)
        {
            // Move o projetil usando Rigidbody.MovePosition
            Vector3 novaPosicao = rb.position + direcao * velocidade * Time.deltaTime;
            rb.MovePosition(novaPosicao);

            yield return null;
        }

        Destroy(projetil);
    }
    #endregion
}