using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private SeedDisplay[] seeds;

    [SerializeField]
    private CropDisplay[] crops;

    public void OpenClose()
    {
        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);

            UpdateDisplay();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void UpdateDisplay()
    {
        foreach (SeedDisplay seed in seeds) 
        {
            seed.UpdateDisplay();
        }

        foreach (CropDisplay crop in crops)
        {
            crop.UpdateDisplay();
        }
    }
}
