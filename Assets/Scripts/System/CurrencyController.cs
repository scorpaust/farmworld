using UnityEngine;

public class CurrencyController : MonoBehaviour
{
    public static CurrencyController Instance;

	[SerializeField]
	private float currentMoney;

	public float CurrentMoney {  get { return currentMoney; }  set { currentMoney = value; } }

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

	private void Start()
	{
		UIController.Instance.UpdateMoneyText(currentMoney);
	}

	public void SpendMoney(float amountToSpend)
	{
		currentMoney -= amountToSpend;

		UIController.Instance.UpdateMoneyText(currentMoney);
	}

	public void AddMoney(float amountToAdd)
	{
		currentMoney += amountToAdd;

		UIController.Instance.UpdateMoneyText(currentMoney);
	}

	public bool EnoughMoney(float amount)
	{
		return currentMoney >= amount;
	}
}
