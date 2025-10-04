using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField]
    float PitchRandomizerOffset;

    float startingPitch;

    [SerializeField]
    private AudioSource AudioSource;

    private KeyCode DebugKeyCode = KeyCode.P;

    [SerializeField]
    private AudioClip[] AudioClips;

    [SerializeField] 
    bool PlayOnStart;

    private float lastAudioTime = 0f; // Track the last known play time

    void Awake()
    {   
        AudioSource = GetComponent<AudioSource>();
        startingPitch = AudioSource.pitch;
        RandomizeSound();
    }

    void Start()
    {
        if(PlayOnStart)
        {
            PlaySound();
        }
    }

    void Update()
    {
        // Debug key to manually switch sound
        if (Input.GetKeyDown(DebugKeyCode))
        {
            RandomizeSound();
            AudioSource.Play();
        }

        // Check if the audio clip has completed a loop
        if (AudioSource.isPlaying && AudioSource.loop)
        {
            if (AudioSource.time < lastAudioTime) // When time resets to zero, it's a new loop
            {
                PlaySound();
            }
            lastAudioTime = AudioSource.time; // Update last known play time
        }
    }

    public void PlaySound()
    {
        RandomizeSound();
        AudioSource.Play();
    }

    void RandomizeSound()
    {
        AudioSource.clip = AudioClips[Random.Range(0, AudioClips.Length)];
        AudioSource.pitch = Mathf.Clamp(Random.Range(startingPitch - PitchRandomizerOffset, startingPitch +PitchRandomizerOffset), .1f, 3f);
    }
}

