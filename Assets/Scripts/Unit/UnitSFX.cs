using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSFX : MonoBehaviour
{
    
    private AudioSource _audioSource;
    [SerializeField] List<AudioClip> _attackSounds;
    [SerializeField] List<AudioClip> _deathSounds;

    // Start is called before the first frame update
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }

    public void PlayShootSound()
    {
        _audioSource.PlayOneShot(_attackSounds[Random.Range(0, _attackSounds.Count)]);
    }
    
    public void PlayDeathSound()
    {
        _audioSource.PlayOneShot(_deathSounds[Random.Range(0, _deathSounds.Count)]);
    }
}
