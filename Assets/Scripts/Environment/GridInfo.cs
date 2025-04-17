using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class GridInfo : MonoBehaviour
{
    public static GridInfo instance;

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

    [SerializeField]
    private bool hasGrid;

    public bool HasGrid { get { return hasGrid; } set { hasGrid = value; } }

    private List<InfoRow> theGrid;

    public List<InfoRow> TheGrid {  get { return theGrid; } }

    public void CreateGrid()
    {
        hasGrid = true;

		// Instancia a lista antes de usar
		theGrid = new List<InfoRow>();

		for (int y = 0; y < GridController.instance.BlockRows.Count; y++)
        {
            if (theGrid == null) return;

            theGrid.Add(new InfoRow());

            for (int x = 0; x < GridController.instance.BlockRows[y].blocks.Count; x++)
            {
                theGrid[y].Blocks.Add(new BlockInfo());
            }
        }
    }

	public void UpdateInfo(GrowBlock theBlock, int xPos, int yPos)
	{
        if (theBlock == null || theGrid == null) return;

        theGrid[yPos].Blocks[xPos].CurrentStage = theBlock.currentStage;

		theGrid[yPos].Blocks[xPos].IsWatered = theBlock.IsWatered;
	}

    public void GrowCrop()
    {

		for (int y = 0; y < theGrid.Count; y++)
        {
            for (int x = 0; x < theGrid[y].Blocks.Count; x++)
            {
                if (theGrid[y].Blocks[x].IsWatered == true)
                {
                    switch (theGrid[y].Blocks[x].CurrentStage)
                    {
                        case GrowBlock.GrowthStage.planted:

                            theGrid[y].Blocks[x].CurrentStage = GrowBlock.GrowthStage.growing1;

                            break;

						case GrowBlock.GrowthStage.growing1:

							theGrid[y].Blocks[x].CurrentStage = GrowBlock.GrowthStage.growing2;

							break;

						case GrowBlock.GrowthStage.growing2:

							theGrid[y].Blocks[x].CurrentStage = GrowBlock.GrowthStage.ripe;

							break;
					}

                    GridController.instance.BlockRows[y].blocks[x].AdvanceCrop();

					theGrid[y].Blocks[x].IsWatered = false;
                }
            }
        }
    }

	private void Update()
	{
		if (Keyboard.current.yKey.wasPressedThisFrame)
        {
            GrowCrop();
        }
	}
}

[System.Serializable]
public class BlockInfo
{
    private bool isWatered;

    public bool IsWatered { get { return isWatered; } set { isWatered = value; } }

    private GrowBlock.GrowthStage currentStage;

    public GrowBlock.GrowthStage CurrentStage { get { return currentStage; } set { currentStage = value; } }
}

[System.Serializable]
public class InfoRow
{
    private List<BlockInfo> blocks = new List<BlockInfo>();

    public List<BlockInfo> Blocks { get { return blocks; } }
}
