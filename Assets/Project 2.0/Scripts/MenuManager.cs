
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    [SerializeField] GameObject ButtonSoundEffect;
    [SerializeField] Slider sensitivitySlider;
    [SerializeField] Slider volumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject introTextPanel;
    [SerializeField] GameObject introText;
    [SerializeField] AudioSource MusicSource;

    [SerializeField] float scrollSpeed = .25f;
    [SerializeField] float startTextPosition;
    [SerializeField] float stopScrollAt;


    bool scrollText = false;


    void Awake()
    {

        GameManager.Instance.sensitivity = sensitivitySlider.value;

        volumeSlider.value = GameManager.Instance.masterVolume;
        AudioListener.volume = GameManager.Instance.masterVolume;

        musicVolumeSlider.value = GameManager.Instance.musicVolume;
        MusicSource.volume = GameManager.Instance.musicVolume;

        sensitivitySlider.value = GameManager.Instance.sensitivity;
        

        optionsMenu.SetActive(false);
        introTextPanel.SetActive(false);
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

    public void ProceedButton()
    {
        scrollText = true;
        introTextPanel.SetActive(true);
        introText.transform.position = (
                new Vector3(introText.transform.position.x, startTextPosition, introText.transform.position.z)
                );
    }

    public void StartGame()
    {
        StartCoroutine(WaitAndLoadScene("Arena"));
    }

    IEnumerator WaitAndLoadScene(string sceneName)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync(sceneName);
    }
    
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ToggleOptions();
        }

        if(scrollText)
        {
            introText.transform.position = (
                new Vector3(introText.transform.position.x, introText.transform.position.y + (scrollSpeed * Time.deltaTime), introText.transform.position.z)
                );

            if(introText.transform.position.y >= stopScrollAt)
            {
                scrollText = false;
            }
        }
    }

}
