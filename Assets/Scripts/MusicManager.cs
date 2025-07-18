using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    public AudioSource musicSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep music across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        float volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        musicSource.volume = volume;

        bool musicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        musicSource.mute = !musicOn;

        musicSource.Play();
    }

    public void SetVolume(float value)
    {
        musicSource.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void ToggleMusic(bool isOn)
    {
        musicSource.mute = !isOn;
        PlayerPrefs.SetInt("MusicOn", isOn ? 1 : 0);
    }
}
