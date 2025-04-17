using UnityEngine;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance;

    [SerializeField]
    private float currentTime;

    public float CurrentTime { get { return currentTime; } private set { } }

	[SerializeField]
	private float dayStart;

    public float DayStart {  get { return dayStart; } private set { } }

	[SerializeField]
	private float dayEnd;

    public float DayEnd {  get { return dayEnd; } private set { } }

    [SerializeField]
    private float timeSpeed = 0.25f;

	[SerializeField]
	private int currentDay = 1;

	public int CurrentDay {  get {  return currentDay; } private set { } }

    private bool timeActive;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = dayStart;

        timeActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeActive)
		{
			ActivateTime();
		}
    }

    private void ActivateTime()
    {
		currentTime += Time.deltaTime * timeSpeed;

		if (currentTime > dayEnd)
		{
			currentTime = dayEnd;

			EndDay();
		}

		if (UIController.Instance != null)
		{
			UIController.Instance.UpdateTimeText(currentTime);
		}
	}

	public void StartDay()
	{
		timeActive = true;

		currentTime = 7;
	}

	public void EndDay()
	{
		timeActive = false;

		GridInfo.instance.GrowCrop();

		currentDay++;

		UIController.Instance.ActivateEndDayTransition();
	}
}
