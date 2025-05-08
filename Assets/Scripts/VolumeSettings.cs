using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
        }
        
        if(PlayerPrefs.HasKey("SfxVolume"))
        {
            LoadSfx();
        }
        else
        {
            SetSfxVolume();
        }
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);

        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSfxVolume()
    {
        float volume = sfxSlider.value;
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);

        PlayerPrefs.SetFloat("SfxVolume", volume);
    }

    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
    }

    private void LoadSfx()
    {
        sfxSlider.value = PlayerPrefs.GetFloat("SfxVolume");
    }
}
