using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public AudioSource BGMSource;
    public float BGMcurrentVolume;
    public float SFXcurrentVolume;
    public float BGMFadeSpeed;

    WaitForSeconds waitSound;

    public delegate void SFXSoundFnc();
    SFXSoundFnc SFXSoundAction;

    void Start()
    {
        SFXSoundAction = new SFXSoundFnc(CheckSoundVolume);
        Debug.Log("> Start SoundManager");
        waitSound = new WaitForSeconds(BGMFadeSpeed * 0.01f);
    }
    void CheckSoundVolume()
    {
        if (SFXcurrentVolume < 0) SFXcurrentVolume = 0;
        else if (SFXcurrentVolume > 1) SFXcurrentVolume = 1;
    }

    public void StartGameBGM()
    {
        BGMFadeInSound();
    }

    public void SetBGMVolume(float volume)
    {
        BGMSource.volume = volume;
    }

    public void SetSFXAudio(AudioSource audioSource)
    {
        audioSource.volume = SFXcurrentVolume;
        void SetSFX()
        {
            audioSource.volume = SFXcurrentVolume;
        }
        SFXSoundAction += SetSFX;
    }
    public void SetSFXVolume(float volume)
    {
        SFXcurrentVolume = volume;
        SFXSoundAction();
    }


    /// <summary>
    /// 점점 볼륨 줄이기
    /// </summary>
    /// <param name="action"></param>
    public void BGMFadeOutSound(Action action = null)
    {
        //소리 올림 멈춤
        StopCoroutine(FadeInSoundCoroutine());
        //소리 줄여짐
        StartCoroutine(FadeOutSoundCoroutine(action));
    }
    IEnumerator FadeOutSoundCoroutine(Action action = null)
    {
        float maxVolume = BGMcurrentVolume;
        float volume = maxVolume / BGMFadeSpeed;
        if (!BGMSource.isPlaying) BGMSource.Play();
        while (volume > 0)
        {
            volume -= volume;
            BGMSource.volume = volume;
            yield return waitSound;
        }
        BGMSource.volume = 0f;

        //볼륨 올리는 함수 실행시 필요
        action?.Invoke();
    }

    /// <summary>
    /// 점점 볼륨 높이기
    /// </summary>
    /// <param name="action">callback</param>
    public void BGMFadeInSound(Action action = null)
    {
        //소리 줄임 멈춤
        StopCoroutine(FadeOutSoundCoroutine());

        //소리 올라감
        StartCoroutine(FadeInSoundCoroutine(action));
    }
    IEnumerator FadeInSoundCoroutine(Action action = null)
    {
        if (!BGMSource.isPlaying) BGMSource.Stop();
        BGMSource.volume = 0;
        BGMSource.Play();

        float maxVolume = BGMcurrentVolume;
        float volume = maxVolume / BGMFadeSpeed;

        //노래 변경
        action?.Invoke();

        while (volume < maxVolume)
        {
            volume += volume;
            BGMSource.volume = volume;
            if (!BGMSource.isPlaying) BGMSource.Play();
            yield return waitSound;
        }
        BGMSource.volume = maxVolume;
    }


    public void SetBGM(string clipName = "")
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
