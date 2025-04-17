using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField]
    private GameObject[] toolbarActivators;

    [SerializeField]
    private TMP_Text timeText;

    public TMP_Text TimeText { get { return timeText; } private set { } }

    private DayTransitionUI dayTransitionPanel;

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

    public void ActivateEndDayTransition()
    {
        if (TimeController.Instance != null)
        {
            dayTransitionPanel.ShowDayTransition(TimeController.Instance.CurrentDay);
		}
    }
}
