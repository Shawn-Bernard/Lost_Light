using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private float musicVolume;

    [SerializeField] private float sfxVolume;

    [Header("Sound Clips")]
    public AudioClip MusicClip;
    public AudioClip shootClip;
    public AudioClip pickupClip;
    public AudioClip hitClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        PlayMusic();
    }

    private void PlaySFX(AudioClip clip, Vector3 position, float volume)
    {
        AudioSource.PlayClipAtPoint(clip, position, volume);
    }

    public void PlayMusic()
    {
        audioSource.clip = MusicClip;
        audioSource.volume = musicVolume;
        audioSource.Play();
    }

    public void PlayShoot(Vector3 soundPosition)
    {
        PlaySFX(shootClip, soundPosition, sfxVolume);
    }

    public void PlayPickup(Vector3 soundPosition)
    {
        PlaySFX(pickupClip, soundPosition, sfxVolume);
    }
    public void PlayHit(Vector3 soundPosition)
    {
        PlaySFX(hitClip, soundPosition, sfxVolume);
    }
}
