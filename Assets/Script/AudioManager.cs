using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Background Music")]
    public AudioSource backgroundMusic;
    public AudioSource winlose;
    [Header("Sound Effects")]
    public AudioSource jumpSFX;
    public AudioSource attackSFX;
    public AudioSource hurtSFX;
    public AudioSource runSFX;
    public AudioSource coinSFX;
    public AudioSource clickSFX;
    public AudioSource winSFX;
    public AudioSource loseSFX;

    private bool isMusicOn = true;
    private bool isSFXOn = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    // ------------------------
    // MUSIC TOGGLE
    // ------------------------
    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        if (backgroundMusic != null)
            backgroundMusic.mute = !isMusicOn;
    }

    public bool IsMusicOn() => isMusicOn;

    // ------------------------
    // SFX TOGGLE
    // ------------------------
    public void ToggleSFX()
    {
        isSFXOn = !isSFXOn;
    }

    public bool IsSFXOn() => isSFXOn;

    // ------------------------
    // PLAY SPECIFIC SOUND
    // ------------------------
    public void PlayJump() => PlaySFX(jumpSFX);
    public void PlayAttack() => PlaySFX(attackSFX);
    public void PlayHurt() => PlaySFX(hurtSFX);
    public void PlayRun() => PlaySFX(runSFX);
    public void PlayCoin() => PlaySFX(coinSFX);
    public void PlayClick() => PlaySFX(clickSFX);
    public void PlayWin() => PlaySFX(winSFX);
    public void PlayLose() => PlaySFX(loseSFX);
    // ------------------------
    // GENERIC PLAY METHOD
    // ------------------------
    public void PlaySFX(AudioSource sfx)
    {
        if (isSFXOn && sfx != null)
        {
            sfx.Play();
        }
    }
    public void StopBackgroundMusic()
    {
        if (backgroundMusic != null)
            backgroundMusic.Stop();
    }

    public void StopAllEffects()
    {
        if (jumpSFX != null) jumpSFX.Stop();
        if (attackSFX != null) attackSFX.Stop();
        if (hurtSFX != null) hurtSFX.Stop();
        if (runSFX != null) runSFX.Stop();
        if (coinSFX != null) coinSFX.Stop();
        if (clickSFX != null) clickSFX.Stop();
        
    }

    
}
