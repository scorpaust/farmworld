using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CropDisplay : MonoBehaviour
{
	[SerializeField]
	private CropType crop;

	[SerializeField]
	private Image cropImage;

	[SerializeField]
	private TMP_Text amountText;

	public void UpdateDisplay()
	{
		CropInfo info = CropController.Instance.GetCropInfo(crop);

		cropImage.sprite = info.finalCrop;

		amountText.text = "x" + info.cropAmount;
	}
}
