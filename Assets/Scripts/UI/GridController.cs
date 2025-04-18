using UnityEngine;
using System.Collections.Generic;

public class GridController : MonoBehaviour
{
    [SerializeField]
    private Transform minPoint, maxPoint;

    [SerializeField]
    private GrowBlock baseGridBlock;

    private Vector2Int gridSize;

    [SerializeField]
    private List<BlockRow> blockRows = new List<BlockRow>();

    public List<BlockRow> BlockRows { get { return blockRows; } set { blockRows = value; } }

    [SerializeField]
    private LayerMask gridBlockers;

	public static GridController instance;

	private void Awake()
	{
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateGrid()
    {
        minPoint.position = new Vector3(Mathf.Round(minPoint.position.x), Mathf.Round(minPoint.position.y), 0f);

		maxPoint.position = new Vector3(Mathf.Round(maxPoint.position.x), Mathf.Round(maxPoint.position.y), 0f);

        Vector3 startPosition = minPoint.position + new Vector3(.5f, .5f, 0f);

        // Instantiate(baseGridBlock, startPosition.position, Quaternion.identity);

        gridSize = new Vector2Int(Mathf.RoundToInt(maxPoint.position.x - minPoint.position.x), Mathf.RoundToInt(maxPoint.position.y - minPoint.position.y));

        for (int y = 0; y < gridSize.y; y++)
        {
            blockRows.Add(new BlockRow());

            for (int x = 0; x < gridSize.x; x++)
            {
                GrowBlock newBlock = Instantiate(baseGridBlock, startPosition + new Vector3(x, y, 0f), Quaternion.identity);

                newBlock.transform.SetParent(transform);

                newBlock.SpriteRenderer.sprite = null; 

                newBlock.SetGridPosition(x, y);

                blockRows[y].blocks.Add(newBlock);

                if (Physics2D.OverlapBox(newBlock.transform.position, new Vector2(.9f, .9f), 0f, gridBlockers))
                {
                    newBlock.SpriteRenderer.sprite = null;

                    newBlock.PreventUse = true;
                }

                if (GridInfo.instance.HasGrid == true)
                {
                    BlockInfo storedBlock = GridInfo.instance.TheGrid[y].Blocks[x];

                    newBlock.currentStage = storedBlock.CurrentStage;

                    newBlock.IsWatered = storedBlock.IsWatered;

                    newBlock.CropType = storedBlock.CropType;

                    newBlock.GrowthFailChance = storedBlock.GrowthFailChance;

                    newBlock.SetSoilSprite();

                    newBlock.UpdateCropSprite();
                }
            }
        }

        if (GridInfo.instance.HasGrid == false)
        {
            GridInfo.instance.CreateGrid();
        }

        baseGridBlock.gameObject.SetActive(false);
	}

	public GrowBlock GetBlock(float x, float y)
	{
		int intX = Mathf.RoundToInt(x - minPoint.position.x);
		int intY = Mathf.RoundToInt(y - minPoint.position.y);

		if (intX >= 0 && intX < gridSize.x && intY >= 0 && intY < gridSize.y)
		{
			return blockRows[intY].blocks[intX];
		}

		Debug.LogWarning($"[GetBlock] Índice fora do alcance: ({intX}, {intY}) - Grid size: {gridSize}");
		return null;
	}
}

[System.Serializable]
public class BlockRow
{
    public List<GrowBlock> blocks = new List<GrowBlock>();
}
