using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrowBlock : MonoBehaviour
{
    public enum GrowthStage 
    {
        barren,
        ploughed,
        planted,
        growing1,
        growing2,
        ripe
    }

    public GrowthStage currentStage;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

	public SpriteRenderer SpriteRenderer
	{
		get
		{
			return spriteRenderer;
		}

        set
        {
            spriteRenderer = value;
        }
	}

	[SerializeField]
    private Sprite soilTilled;

    public Sprite SoilTilled { get { return soilTilled; } set { } }

	[SerializeField]
	private Sprite soilWatered;

	public Sprite SoilWatered { get { return soilWatered; } set { } }

	[SerializeField]
    private bool isWatered;

    public bool IsWatered {  get { return isWatered; } set { } }

    [SerializeField]
    private SpriteRenderer cropSr;

    [SerializeField]
    private Sprite cropPlanted, cropGrowing1, cropGrowing2, cropRipe;

    [SerializeField]
    private bool preventUse;

    public bool PreventUse { get { return preventUse; } set { preventUse = value; } }

    [SerializeField]
    private CropType cropType;

    public CropType CropType { get { return cropType; } set { cropType = value; } }
    
    private Vector2Int gridPos;

    private float growthFailChance;

    public float GrowthFailChance { get {  return growthFailChance; } set { growthFailChance = value; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
		yield return new WaitUntil(() => GridInfo.instance != null);

		currentStage = GrowthStage.barren;

		SetSoilSprite();
		
        UpdateCropSprite();
	}

    // Update is called once per frame
    void Update()
    {
        #if UNITY_EDITOR
                    if (Keyboard.current.nKey.wasPressedThisFrame)
                    {
                        AdvanceCrop();
                    }
        #endif
    }

    public void UpdateCropSprite()
    {
        CropInfo activeCrop = CropController.Instance.GetCropInfo(cropType);

        switch(currentStage)
        {
            case GrowthStage.planted:

                cropSr.sprite = activeCrop.planted;

                break;

            case GrowthStage.growing1:

                cropSr.sprite = activeCrop.growStage1;

                break;

            case GrowthStage.growing2:

                cropSr.sprite = activeCrop.growStage2;

                break;

            case GrowthStage.ripe:

                cropSr.sprite = activeCrop.ripe;

                break;
        }

        UpdateGridInfo();
    }

    public void AdvanceStage()
    {
        currentStage = currentStage == GrowthStage.ripe ?  GrowthStage.barren : currentStage += 1;
    }

    public void SetSoilSprite()
    {
        if (currentStage == GrowthStage.barren)
        {
            spriteRenderer.sprite = null;
        }
        else
        {
            if (isWatered)
            {
                spriteRenderer.sprite = soilWatered;
            }
            else
            {
				spriteRenderer.sprite = soilTilled;
			}   
        }

        UpdateGridInfo();
    }

    public void PloughSoil()
    {
        if (currentStage == GrowthStage.barren && !preventUse)
        {
            currentStage = GrowthStage.ploughed;

            SetSoilSprite();
        }
    }

    public void WaterSoil()
    {

        if (!preventUse)
        {
            isWatered = true;

            SetSoilSprite();
        }
    }

    public void PlantCrop(CropType cropToPlant)
    {
        if (currentStage == GrowthStage.ploughed && IsWatered && !preventUse)
        {
            currentStage = GrowthStage.planted;

            cropType = cropToPlant;

            growthFailChance = CropController.Instance.GetCropInfo(cropToPlant).growthFailChance;

            CropController.Instance.UseSeed(cropToPlant);

            UpdateCropSprite();
        }
    }

    public void AdvanceCrop()
    {
        if (isWatered && !preventUse)
        {
            if (currentStage == GrowthStage.planted || currentStage == GrowthStage.growing1 || currentStage == GrowthStage.growing2)
            {
                currentStage++;

                isWatered = false;

                SetSoilSprite();

                UpdateCropSprite();
            }
        }
	}

    public void HarvestCrop()
    {
        if (currentStage == GrowthStage.ripe && !preventUse)
        {
            currentStage = GrowthStage.ploughed;

            SetSoilSprite();

            cropSr.sprite = null;

            CropController.Instance.AddCrop(cropType);
        }
    }

    public void SetGridPosition(int x, int y)
    {
        gridPos = new Vector2Int(x, y);
    }

    private void UpdateGridInfo()
    {
        GridInfo.instance.UpdateInfo(this, gridPos.x, gridPos.y);
    }
}
