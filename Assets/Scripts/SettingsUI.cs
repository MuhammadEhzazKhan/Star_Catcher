using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("Audio Settings UI")]
    public Toggle musicToggle;
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Start()
    {
        // Load saved settings
        bool musicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // Set initial UI state
        musicToggle.isOn = musicOn;
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;

        // Apply values immediately
        MusicManager.instance.ToggleMusic(musicOn);
        MusicManager.instance.SetVolume(musicVolume);
        AudioListener.volume = sfxVolume;

        // Add event listeners
        musicToggle.onValueChanged.AddListener(OnMusicToggle);
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    private void OnMusicToggle(bool isOn)
    {
        MusicManager.instance.ToggleMusic(isOn);
        PlayerPrefs.SetInt("MusicOn", isOn ? 1 : 0);
    }

    private void OnMusicVolumeChanged(float volume)
    {
        MusicManager.instance.SetVolume(volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    private void OnSFXVolumeChanged(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}
