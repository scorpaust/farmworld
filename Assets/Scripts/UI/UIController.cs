using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField]
	private GameObject[] toolbarActivators;

    [SerializeField]
    private TMP_Text timeText;

    public TMP_Text TimeText { get { return timeText; } private set { } }

    [SerializeField]
    private InventoryController invControl;

    public InventoryController InvControl { get { return invControl; } private set { } }

	[SerializeField]
	private ShopController shopControl;

	public ShopController ShopControl { get { return shopControl; } private set { } }

	[SerializeField]
    private Image seedImage;

    private DayTransitionUI dayTransitionPanel;

    [SerializeField]
    private TMP_Text moneyText;

    public TMP_Text MoneyText {  get { return moneyText; } set { moneyText = value; } }

    [SerializeField]
    private GameObject pauseScreen;

    public GameObject PauseScreen {  get { return pauseScreen; } private set { } }

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			
            DontDestroyOnLoad(gameObject);
		}
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        dayTransitionPanel = GetComponent<DayTransitionUI>();

        SwitchTool(0);
    }

    // Update is called once per frame
    void Update()
    {
        ManageInventory();

#if UNITY_EDITOR

        ManageShop();
#endif

        if (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.pKey.wasPressedThisFrame)
        {
            PauseUnpause();
        }
    }

    private void ManageInventory()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            invControl.OpenClose();
        }
    }

	private void ManageShop()
	{
		if (Keyboard.current.bKey.wasPressedThisFrame)
		{
			shopControl.OpenClose();
		}
	}

	public void DeactivateToolbarActivators()
    {
        foreach (GameObject icon in toolbarActivators)
        {
            icon.SetActive(false);
        }
    }

    public void SwitchTool(int selected)
    {
        DeactivateToolbarActivators();

        toolbarActivators[selected].SetActive(true);
    }

    public void UpdateTimeText(float currentTime)
    {
        if (currentTime < 12)
        {
            timeText.text = Mathf.FloorToInt(currentTime) + "AM";
        }
		else if (currentTime < 13)
		{
			timeText.text = "12PM";
		}
        else if (currentTime < 24)
        {
			timeText.text = Mathf.FloorToInt(currentTime - 12) + "PM";
		}
        else if (currentTime < 25)
        {
			timeText.text = "12AM";
		}
        else
        {
			timeText.text = Mathf.FloorToInt(currentTime - 24) + "AM";
		}
	}

    public void UpdateMoneyText(float currentMoney)
    {
        moneyText.text = currentMoney + " €";
	}

    public void ActivateEndDayTransition()
    {
        if (TimeController.Instance != null)
        {
            dayTransitionPanel.ShowDayTransition(TimeController.Instance.CurrentDay);

			AudioManager.Instance.PauseMusic();

			AudioManager.Instance.PlaySFX(1);
		}
    }

    public void SwitchSeed(CropType crop)
    {
        seedImage.sprite = CropController.Instance.GetCropInfo(crop).seedType;

		AudioManager.Instance.PlaySFXPitchAdjust(5);
	}

    public void PauseUnpause()
    {
        if (!pauseScreen.activeSelf)
        {
            pauseScreen.SetActive(true);

            Time.timeScale = 0f;
        }
        else
        {
            pauseScreen.SetActive(false);

            Time.timeScale = 1f;
        }

		AudioManager.Instance.PlaySFXPitchAdjust(5);
	}

    public void BackToMainMenu()
    {
		Time.timeScale = 1f;

		SceneManager.LoadScene("Menu");

        Destroy(gameObject);

        Destroy(PlayerController.instance.gameObject);

        Destroy(TimeController.Instance.gameObject);

          Destroy(CurrencyController.Instance.gameObject);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
