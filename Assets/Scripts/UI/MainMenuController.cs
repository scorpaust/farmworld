using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private string levelToStart;

	private void Start()
	{
        AudioManager.Instance.PlayTitle();
	}

	public void PlayGame()
    {
        SceneManager.LoadScene(levelToStart);

        AudioManager.Instance.NextBGM();

		AudioManager.Instance.PlaySFXPitchAdjust(5);
	}

    public void QuitGame()
    {
		AudioManager.Instance.PlaySFXPitchAdjust(5);

		Application.Quit();
    }
}
