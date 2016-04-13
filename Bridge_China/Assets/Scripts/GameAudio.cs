using UnityEngine;
using System.Collections;

public class GameAudio : MonoBehaviour 
{
	public  AudioSource Music_bg;
	public  AudioSource Music_help;
	public  AudioSource Music_gameOver;
	public  AudioSource Music_coin;
	public  AudioSource Music_ganZi;

	public float musicValue = 1.0f;		//音乐声音大小
	public float soundValue = 1.0f;     //音效声音大小
	
	
	static GameAudio instance;
	public static GameAudio sharedInstance {
		get {
			if(instance == null) {
				instance = new GameObject("Audio").AddComponent<GameAudio>();
			}
			return instance;
		}
	}

	void InitSoundVolume()
	{
		if(PlayerPrefs.HasKey("SOUND_VOLUMN"))
		{
			soundValue = PlayerPrefs.GetFloat("SOUND_VOLUMN", 1f);	
		}

		if(PlayerPrefs.HasKey("MUSIC_VOLUMN"))
		{
			musicValue = PlayerPrefs.GetFloat("MUSIC_VOLUMN", 1f);		
		}		
	}

	void Awake () 
	{
		if(instance == null) instance = this;

		DontDestroyOnLoad (gameObject);

		//InitSoundVolume();
	}


	public void Play(AudioSource Music)
	{
		if (!Music.isPlaying)
		{
			Music.Play();
		}
	}
	
	public void Pause(AudioSource Music)
	{
		if (Music.isPlaying)
		{
			Music.Pause();
		}
	}
	
	public void Stop(AudioSource Music)
	{
		if (Music.isPlaying)
		{
			Music.Stop();
		}
	}
	
	public void PauseOrStartAllMusic(bool IsPause)
	{
		if(IsPause)
		{
			Pause(Music_bg);
			Pause(Music_help);
            Pause(Music_gameOver);
		}
		else
		{
			Play(Music_bg);
            Play(Music_help);
            Play(Music_gameOver);
		}
	}

	//
	void Update()
	{
        //Music_fight.volume = musicValue;
        //Music_main.volume = musicValue;
        //Music_login.volume = musicValue;
	}
	
}
