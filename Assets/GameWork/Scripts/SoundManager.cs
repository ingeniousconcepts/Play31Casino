using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SoundManager provides music sound playback interface and mute control for the game
/// </summary>
public class SoundManager : RefSingleton<SoundManager> {

    public AudioClip click;
    public AudioClip button;
    public AudioClip flip;
    public AudioClip win;
    public AudioClip failed;

    public AudioSource uiAudio;
    public AudioSource musicAudio;


    public bool mute
    {
        get
        {
            return this.musicAudio.mute; 
        }
        set {
            this.musicAudio.mute = value;
            this.uiAudio.mute = value;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void PlayClick()
    {
        this.uiAudio.PlayOneShot(click);
    }
    public void PlayButton()
    {
        this.uiAudio.PlayOneShot(button);
    }
    public void PlayFlip()
    {
        this.uiAudio.PlayOneShot(flip);
    }

    public void PlayFailed()
    {
        this.uiAudio.PlayOneShot(failed);
    }

    public void PlayWin()
    {
        this.uiAudio.PlayOneShot(win);
    }
}
