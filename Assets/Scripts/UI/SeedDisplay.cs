using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeedDisplay : MonoBehaviour
{
    [SerializeField]
    private CropType crop;

    [SerializeField]
    private Image seedImage;

    [SerializeField]
    private TMP_Text seedAmount;

    public void UpdateDisplay()
    {
        CropInfo info = CropController.Instance.GetCropInfo(crop);

        seedImage.sprite = info.seedType;

        seedAmount.text = "x" + info.seedAmount;
    }

    public void SelectSeed()
    {
        PlayerController.instance.SwitchSeed(crop);

        UIController.Instance.SwitchSeed(crop);

        UIController.Instance.InvControl.OpenClose();
    }
}
