using System;

using System.Collections.Generic;
using UnityEngine;

public class SoundManager: Singleton<SoundManager>
{
	public enum SoundType
	{
		Bgm,
		SubBgm,
		Effect,
		Max,
	}
	
	private AudioSource[] audioSources = new AudioSource[(int)SoundType.Max];
	private Dictionary<string, AudioClip> audioClipDict = new Dictionary<string, AudioClip>();

	private GameObject soundRoot = null;

	[SerializeField]
	AudioClip bgmAudioClip;
	
	[SerializeField]
	AudioClip mergeAuidoClip;

	[SerializeField]
	AudioClip buttonAudioClip;
	
	[SerializeField]
	AudioClip attackAudioClip;
	
		
	[SerializeField]
	AudioClip setSlotAudioClip;


	[SerializeField]
	AudioClip destroyAudioClip;

	void Start()
	{
		if (soundRoot == null)
		{
			soundRoot = GameObject.Find("@SoundRoot");
			if (soundRoot == null)
			{
				soundRoot = new GameObject { name = "@SoundRoot" };
				UnityEngine.Object.DontDestroyOnLoad(soundRoot);

				string[] soundTypeNames = System.Enum.GetNames(typeof(SoundType));
				for (int count = 0; count < soundTypeNames.Length - 1; count++)
				{
					GameObject go = new GameObject { name = soundTypeNames[count] };
					audioSources[count] = go.AddComponent<AudioSource>();
					go.transform.parent = soundRoot.transform;
				}

				audioSources[(int)SoundType.Bgm].loop = true;
				audioSources[(int)SoundType.SubBgm].loop = true;
			}
		}
		
		PlayBackgroundBGM();
	}

	public void Clear()
	{
		foreach (AudioSource audioSource in audioSources)
			audioSource.Stop();
		audioClipDict.Clear();
	}

	public void Play(SoundType type)
	{
		AudioSource audioSource = audioSources[(int)type];
		audioSource.Play();
	}

	public void Play(SoundType type, AudioClip audioClip, float pitch = 1.0f)
	{
		AudioSource audioSource = audioSources[(int)type];

		if (type == SoundType.Bgm)
		{
			if (audioSource.isPlaying)
				audioSource.Stop();

			audioSource.clip = audioClip;
			audioSource.Play();
		}
		else if (type == SoundType.SubBgm)
		{
			if (audioSource.isPlaying)
				audioSource.Stop();

			audioSource.clip = audioClip;
			audioSource.Play();
		}
		else
		{
			audioSource.pitch = pitch;
			audioSource.PlayOneShot(audioClip);
		}
	}

	public void Stop(SoundType type)
	{
		AudioSource audioSource = audioSources[(int)type];
		audioSource.Stop();
	}

	public void PlayBackgroundBGM()
	{
		Play(SoundType.Bgm, bgmAudioClip);
	}

	public void PlayButton()
	{
		Play(SoundType.Effect, buttonAudioClip);
	}
	
	public void PlayMergeEffect()
	{
		Play(SoundType.Effect, mergeAuidoClip);
	}
	
	public void PlayAttackSound()
	{
		Play(SoundType.Effect, attackAudioClip);
	}
	
	public void PlaySetSlot()
	{
		Play(SoundType.Effect, setSlotAudioClip);
	}
	
	public void PlayDestroy()
	{
		Play(SoundType.Effect, destroyAudioClip);
	}
}
