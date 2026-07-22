using DG.Tweening;
using UnityEngine;

public class CastleAttack : MonoBehaviour
{
    public static CastleAttack ins;
    void Awake() { ins = this; }

    public AudioSource gameMusic;
    public AudioSource soundEffectsAudioSource;

    public AudioClip clip;

    void Start()
    {
        gameMusic.volume = 0;
        gameMusic.DOFade(.2f, 10);

    }

    void Update()
    {
        
    }

    public void PlayBtnSound()
    {
        soundEffectsAudioSource.clip = clip;
        soundEffectsAudioSource.Play(); 
    }
}
