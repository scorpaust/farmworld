using UnityEngine;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviour
{
	[SerializeField] private GameObject uiPrefab;

	private void Awake()
	{
		// Verifica se já existe um Canvas com a tag ou nome
		if (GameObject.FindWithTag("UI") == null)
		{
			var ui = Instantiate(uiPrefab);
			ui.tag = "UI"; // Certifica-te de que o prefab tem essa tag!
			DontDestroyOnLoad(ui);
		}

		// Vai para a cena principal
		SceneManager.LoadScene("Main");
	}
}
