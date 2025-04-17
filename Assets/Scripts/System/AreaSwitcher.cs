using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaSwitcher : MonoBehaviour
{
    [SerializeField]
    private string sceneToLoad;

    [SerializeField]
    private Transform startPoint;

    [SerializeField]
    private string transitionName;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Indoors" && PlayerController.instance != null)
        {

            PlayerController.instance.CanUseTools = false;
        }
        else 
        {
            PlayerController.instance.CanUseTools = true;
        }

        if (PlayerPrefs.HasKey("Transition"))
        {
            if (PlayerPrefs.GetString("Transition") == transitionName)
            {
                PlayerController.instance.transform.position = startPoint.position;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneToLoad);

            PlayerPrefs.SetString("Transition", transitionName);
        }
	}
}
