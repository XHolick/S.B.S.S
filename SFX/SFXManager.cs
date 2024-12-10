using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Coloca todos os efeitos sonoros organizados numa pasta só pra ter certeza q ta certinho.
//Esse script vc vai colocar num empty (chamei o meu de SFXManager)
//Cria um outro empty como prefab. Ele vai ter uma audiosource (DESABILITA O PLAY ON AWAKE). Conecta o prefab no soundFXObject pelo inspector
//Pra chamar a audioSource, tu vai criar no objeto que tu quer (inimigo, player, etc.) duas coisas:
// [SerializeField] private AudioClip[] nomeDoArray; <--- isso serve pra tu colocar todos os sfxs de um tipo específico q tu queira, ele vai tocar um aleatorio entre eles;
// SFXManager.instance.PlaySoundFXClip(nomeDoArray, transform, 0.3f); <--- isso vai puxar pra executar o efeito sonoro.


public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }   
    }

    public void PlaySoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume) 
    {
        int rand = Random.Range(0, audioClip.Length);
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip[rand];
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }
}
