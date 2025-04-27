using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSeedDisplay : MonoBehaviour
{
    [SerializeField]
    private CropType cropType;

    [SerializeField]
    private Image seedImage;

    [SerializeField]
    private TMP_Text seedAmount, priceText;

    public void UpdateDisplay()
    {
		CropInfo info = CropController.Instance.GetCropInfo(cropType);

		seedImage.sprite = info.seedType;

		seedAmount.text = "x" + info.seedAmount;

        priceText.text = info.seedPrice + " € each";
	}

    public void BuySeed(int amount)
    {
		CropInfo info = CropController.Instance.GetCropInfo(cropType);

        if (CurrencyController.Instance.EnoughMoney(info.seedPrice * amount))
        {
            CropController.Instance.AddSeed(cropType, amount);

            CurrencyController.Instance.SpendMoney(info.seedPrice * amount);

            UpdateDisplay();
        }
	}
}
