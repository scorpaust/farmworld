using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopCropDisplay : MonoBehaviour
{
	[SerializeField]
	private CropType crop;

	[SerializeField]
	private Image cropImage;

	[SerializeField]
	private TMP_Text amountText, priceText;

	public void UpdateDisplay()
	{
		CropInfo info = CropController.Instance.GetCropInfo(crop);

		cropImage.sprite = info.finalCrop;

		amountText.text = "x" + info.cropAmount;

		priceText.text = info.cropPrice + " € each";
	}

	public void SellCrop()
	{
		CropInfo info = CropController.Instance.GetCropInfo(crop);

		if (info.cropAmount > 0)
		{
			CurrencyController.Instance.AddMoney(info.cropAmount * info.cropPrice);

			CropController.Instance.RemoveCrop(crop);

			UpdateDisplay();

			AudioManager.Instance.PlaySFXPitchAdjust(5);
		}	
	}
}
