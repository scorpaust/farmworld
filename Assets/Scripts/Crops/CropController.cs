using UnityEngine;
using System.Collections.Generic;

public enum CropType
{
	pumpkin,
	lettuce,
	carrot,
	hay,
	potato,
	strawberry,
	tomato,
	avocado
};

public class CropController : MonoBehaviour
{
    public static CropController Instance;

	[SerializeField]
	private List<CropInfo> cropList = new List<CropInfo>();

	public List<CropInfo> CropList { get { return cropList; } private set { } }

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

	public CropInfo GetCropInfo(CropType cropToGet)
	{
		int position = -1;

		for (int i = 0; i < cropList.Count; i++)
		{
			if (cropList[i].cropType == cropToGet)
			{
				position = i;
			}
		}

		if (position >= 0)
		{
			return cropList[position];
		}
		else
		{
			return null;
		}
	}

	public void UseSeed(CropType cropToUse)
	{
		foreach (CropInfo info in cropList)
		{
			if (info.cropType == cropToUse)
			{
				info.seedAmount--;
			}
		}
	}

	public void AddCrop(CropType cropToAdd)
	{
		foreach (CropInfo info in cropList)
		{
			if (info.cropType == cropToAdd)
			{
				info.cropAmount++;
			}
		}
	}
}

[System.Serializable]
public class CropInfo
{
	public CropType cropType;

	public Sprite finalCrop, seedType, planted, growStage1, growStage2, ripe;

	public int seedAmount, cropAmount;

	[Range(0f, 100f)]
	public float growthFailChance;
}
