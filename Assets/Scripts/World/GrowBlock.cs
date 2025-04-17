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

    private Vector2Int gridPos;

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
        switch(currentStage)
        {
            case GrowthStage.planted:

                cropSr.sprite = cropPlanted;

                break;

            case GrowthStage.growing1:

                cropSr.sprite = cropGrowing1;

                break;

            case GrowthStage.growing2:

                cropSr.sprite = cropGrowing2;

                break;

            case GrowthStage.ripe:

                cropSr.sprite = cropRipe;

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

    public void PlantCrop()
    {
        if (currentStage == GrowthStage.ploughed && IsWatered && !preventUse)
        {
            currentStage = GrowthStage.planted;

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
