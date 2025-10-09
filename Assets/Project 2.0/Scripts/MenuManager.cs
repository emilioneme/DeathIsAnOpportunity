
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    [SerializeField] GameObject ButtonSoundEffect;
    [SerializeField] Slider sensitivitySlider;
    [SerializeField] Slider volumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] AudioSource MusicSource;


    void Awake()
    {

        GameManager.Instance.sensitivity = sensitivitySlider.value;

        volumeSlider.value = GameManager.Instance.masterVolume;
        AudioListener.volume = GameManager.Instance.masterVolume;

        musicVolumeSlider.value = GameManager.Instance.musicVolume;
        MusicSource.volume = GameManager.Instance.musicVolume;

        sensitivitySlider.value = GameManager.Instance.sensitivity;
        

        optionsMenu.SetActive(false);
    }
    public void SetVolume()
    {
        GameManager.Instance.masterVolume = volumeSlider.value;
        AudioListener.volume = GameManager.Instance.masterVolume;
    }

    public void SetMusicVolume()
    {
        GameManager.Instance.musicVolume = musicVolumeSlider.value;
        MusicSource.volume = GameManager.Instance.musicVolume;
    }

    public void SetSensitivity()
    {
        GameManager.Instance.sensitivity = sensitivitySlider.value;
    }


    public void ToggleOptions()
    {
        optionsMenu.SetActive(!optionsMenu.activeInHierarchy);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ToggleOptions();
            Debug.Log("Escape key was released.");
        }
    }

    public void PlayButtonSound()
    {
        Destroy(Instantiate(ButtonSoundEffect), 1f);
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
