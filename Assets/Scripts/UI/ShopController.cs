using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField]
    private ShopSeedDisplay[] seeds;

    [SerializeField]
    private ShopCropDisplay[] crops;

    public void OpenClose()
    {
        if (!UIController.Instance.InvControl.gameObject.activeSelf)
        {
            gameObject.SetActive(!gameObject.activeSelf);

            if (gameObject.activeSelf)
            {
                foreach (ShopSeedDisplay seed in seeds)
                {
                    seed.UpdateDisplay();
                }

                foreach (ShopCropDisplay crop in crops)
                {
                    crop.UpdateDisplay();
                }
            }
        }
    }
}
