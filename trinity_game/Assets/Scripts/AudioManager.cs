using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// Trinity 1.0
    /// Ses yöneticisi fonksiyonlarý
    /// </summary>

    public static AudioManager Instance { get; set; }

    [Header("--- Audio Source ---")]
    [SerializeField] AudioSource musicSource;       // Müzik kontrolcüsü
    [SerializeField] AudioSource sfxSource;         // Efekt kontrolcüsü

    [Header("--- Audio Mixer ---")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("--- Audio Clip ---")]
    public AudioClip backgroundMusic;
    public AudioClip auBombExplode;
    public AudioClip auPowerUpEat;
    public AudioClip auPowerUpExplode;
    public AudioClip auPlayerExplode;
    public AudioClip auPlayerJump;
    public AudioClip auPlayerHi;
    public AudioClip auEnemyExplode;
    public AudioClip auBoxExplode;
    public AudioClip auWallRockHit;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        LoadMusicVolume();
        LoadSfxVolume();
    }

    // Oyun açýldýðýnda yapýlacaklar
    private void Start()
    {
        LoadMusicVolume();
        LoadSfxVolume();
        musicSource.clip = backgroundMusic;
        musicSource.Play();
        //musicSource.vol
    }
    
    // Herhangi bir efekti bir sefer çal
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
        //sfxSource.Play();
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        musicSource.volume = volume;
        //audioMixer.SetFloat("music",volume);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    private void LoadMusicVolume()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
            SetMusicVolume();
            Debug.Log("musicVolume: " + musicSlider.value + " " + PlayerPrefs.GetInt("musicVolume").ToString());
        }
        else
        {
            musicSlider.value = 1;
            //PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
            SetMusicVolume();
            Debug.Log("musicVolume: " + musicSlider.value + " " + PlayerPrefs.GetInt("musicVolume").ToString());

        }
        if (PlayerPrefs.HasKey("musicMute"))
        {
            musicSource.mute = Convert.ToBoolean(PlayerPrefs.GetInt("musicMute"));
            Debug.Log("musicMute: " + PlayerPrefs.GetInt("musicMute").ToString());
        }
        else
        {
            musicSource.mute = false;
            PlayerPrefs.SetInt("musicMute", Convert.ToInt16(musicSource.mute));
            Debug.Log("musicMute: " + PlayerPrefs.GetInt("musicMute").ToString());
        }
    }
    public void TogleMusic()
    {
        musicSource.mute = !musicSource.mute;
        PlayerPrefs.SetInt("musicMute", Convert.ToInt16(musicSource.mute));
        Debug.Log("musicMute: " + PlayerPrefs.GetInt("musicMute").ToString());

    }
    public void SetSfxVolume()
    {
        float volume = sfxSlider.value;
        sfxSource.volume = volume;//Mathf.Log10(volume) * 20;
        //audioMixer.SetFloat("music",volume);

        PlayerPrefs.SetFloat("sfxVolume", volume);
        Debug.Log("sfxVolume: " + volume + " " + PlayerPrefs.GetInt("sfxVolume").ToString());
    }

    private void LoadSfxVolume()
    {
        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
            SetSfxVolume();
            Debug.Log("sfxVolume: " + PlayerPrefs.GetFloat("sfxVolume").ToString());
        }
        else
        {
            sfxSlider.value = 1;
            SetSfxVolume();
            Debug.Log("sfxVolume: " + PlayerPrefs.GetFloat("sfxVolume").ToString());

        }
        if (PlayerPrefs.HasKey("sfxMute"))
        {
            Debug.Log("sfxMute: "+ PlayerPrefs.GetInt("sfxMute").ToString());
            sfxSource.mute = Convert.ToBoolean(PlayerPrefs.GetInt("sfxMute"));
        }
        else
        {
            musicSource.mute = false;
            PlayerPrefs.SetInt("sfxMute", Convert.ToInt16(sfxSource.mute));
        }
    }
    public void TogleSfx()
    {
        sfxSource.mute = !sfxSource.mute;
        Debug.Log("sfxMute: " + PlayerPrefs.GetInt("sfxMute").ToString());
        PlayerPrefs.SetInt("sfxMute", Convert.ToInt16(sfxSource.mute));
    }
}
