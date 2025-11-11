using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    private struct KeyToAudioClip
    {
        public string key;
        public AudioClip clip;
    }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<KeyToAudioClip> _audioClips;

    private Dictionary<string, AudioClip> audioClips = new();

    private void Awake()
    {
        foreach (var clip in _audioClips)
        {
            if (!audioClips.TryAdd(clip.key, clip.clip))
            {
                Debug.LogWarning("Duplicate clip key found in Audio Clip List");
                _audioClips.Remove(clip);
            }
        }
    }

    private void Start()
    {
        GameManager.Instance.AudioManager = this;
    }

    public bool TryPlayAudio(string key)
    {
        if (!audioClips.TryGetValue(key, out var clip)) return false;
        audioSource.PlayOneShot(clip);
        return true;
    }

    public bool TryChangeMusic(string key, bool isLooping)
    {
        if (!audioClips.TryGetValue(key, out var clip)) return false;
        audioSource.clip = clip;
        audioSource.loop = isLooping;
        audioSource.Play();
        return true;
    }
}