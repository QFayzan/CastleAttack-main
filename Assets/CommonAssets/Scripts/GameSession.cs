using UnityEngine;

public class GameSession : MonoBehaviour
{
    public static GameSession ins;
    void Awake() { if (ins == null) { ins = this; } }


    public AudioClip btnSound;
    public AudioSource soundEffectsAudioSource;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        
    }

    public void PlayBtnSound()
    {
        soundEffectsAudioSource.clip = btnSound;
        soundEffectsAudioSource.Play();
    }

    public void PlaySound(AudioClip audioClip)
    {
        soundEffectsAudioSource.clip = audioClip;
        soundEffectsAudioSource.Play();
    }
}
