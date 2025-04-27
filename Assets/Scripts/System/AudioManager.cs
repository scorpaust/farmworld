using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance;

	[SerializeField]
	private AudioSource titleMusic;

	[SerializeField]
	private AudioSource[] bgMusic;

	[SerializeField]
	private AudioSource[] sfx;

	private int currentTrack = -1;

	private bool isPaused;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void StopMusic()
	{
		foreach (AudioSource source in bgMusic)
		{
			source.Stop();
		}

		titleMusic.Stop();
	}

	public void PlayTitle()
	{
		StopMusic();
		titleMusic.Play();
		currentTrack = -1; // Resetar o contador
	}

	public void NextBGM()
	{
		StopMusic();

		currentTrack++;

		if (currentTrack >= bgMusic.Length)
		{
			currentTrack = 0;
		}

		bgMusic[currentTrack].Play();
	}

	void Start()
	{
		currentTrack = -1; // Não começa logo a tocar música
	}

	void Update()
	{
		if (isPaused == false)
		{
			if (currentTrack >= 0 && bgMusic[currentTrack].isPlaying == false)
			{
				NextBGM();
			}
		}
	}

	public void PauseMusic()
	{
		isPaused = true;

		bgMusic[currentTrack].Pause();
	}

	public void ResumeMusic()
	{
		isPaused = false;

		bgMusic[currentTrack].Play();
	}

	public void PlaySFX(int sfxToPlay)
	{
		sfx[sfxToPlay].Stop();

		sfx[sfxToPlay].Play();
	}

	public void PlaySFXPitchAdjust(int sfxToPlay)
	{
		sfx[sfxToPlay].pitch = Random.Range(.8f, 1.2f);

		PlaySFX(sfxToPlay);
	}

}
