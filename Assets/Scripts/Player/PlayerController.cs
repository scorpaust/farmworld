using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public bool CanUseTools = true;

    [SerializeField]
    private CropType seedCropType;

    public CropType SeedCropType {  get { return seedCropType; } private set { } }

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

	[Header("Components")]
    [SerializeField]
    private Rigidbody2D theRb;

    [SerializeField]
    private Animator anim;

    [Header("Interaction")]
    [SerializeField]
    private InputActionReference moveInput, actionInput;

    [Header("Player Properties")]
    [SerializeField]
    [Range(1f, 10f)]
    private float moveSpeed;

    public enum ToolType
    {
        plough,
        wateringCan,
        seeds,
        basket
    }

    [SerializeField]
    private ToolType currentTool;

    public ToolType CurrentTool { get { return currentTool; } set { currentTool = value; } }

    [SerializeField]
    private float toolWaitTime = .5f;

    public float ToolWaitTime { get { return toolWaitTime; } set { toolWaitTime = value; } }

    private float toolWaitCounter;

    [SerializeField]
    private Transform toolIndicator;

    [SerializeField]
    private float toolRange = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIController.Instance.SwitchTool((int)CurrentTool);
    }

    // Update is called once per frame
    void Update()
    {
        if (toolWaitCounter > 0)
        {
            toolWaitCounter -= Time.deltaTime;

            theRb.linearVelocity = Vector2.zero;
        }
        else
        {
			Move();

			FlipDirection();
		}

        SelectTool();

        if (GridController.instance != null)
        {
            if (actionInput.action.WasPressedThisFrame())
            {
                UseTool();
            }

            ManageToolIndicator();
        }
        else
        {
            toolIndicator.position = new Vector3(0f, 0f, -20f);
        }
	}

    private void ManageToolIndicator()
    {
		toolIndicator.position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

		toolIndicator.position = new Vector3(toolIndicator.position.x, toolIndicator.position.y, 0f);

		if (Vector3.Distance(toolIndicator.position, transform.position) > toolRange)
		{
			Vector2 direction = toolIndicator.position - transform.position;

			direction = direction.normalized * toolRange;

			toolIndicator.position = transform.position + new Vector3(direction.x, direction.y, 0f);
		}

		toolIndicator.position = new Vector3(Mathf.FloorToInt(toolIndicator.position.x) + .5f, Mathf.FloorToInt(toolIndicator.position.y) + .5f, 0f);
	}

    private void Move()
    {
        theRb.linearVelocity = moveInput.action.ReadValue<Vector2>().normalized * moveSpeed;

        anim.SetFloat("Speed", theRb.linearVelocity.magnitude);        
	}

    private void FlipDirection()
    {
        if (theRb.linearVelocity.x < 0f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);

        }
        else if (theRb.linearVelocity.x > 0f)
		{
            transform.localScale = Vector3.one;
		}
    }

    private void SelectTool()
    {
        bool hasSwitchedTool = false;

        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            CurrentTool++;

            if ((int)CurrentTool >= 4)
            {
                CurrentTool = ToolType.plough;
            }

            hasSwitchedTool = true;
        }

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            CurrentTool = ToolType.plough;

			hasSwitchedTool = true;
		}

		if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            CurrentTool = ToolType.wateringCan;

			hasSwitchedTool = true;
		}

		if (Keyboard.current.digit3Key.wasPressedThisFrame)
		{
			CurrentTool = ToolType.seeds;

			hasSwitchedTool = true;
		}

		if (Keyboard.current.digit4Key.wasPressedThisFrame)
		{
			CurrentTool = ToolType.basket;

            hasSwitchedTool = true;
		}

        if (hasSwitchedTool)
        {
            UIController.Instance.SwitchTool((int)CurrentTool);
        }
	}

    private void UseTool()
    {
        if (!CanUseTools) return;

        GrowBlock block = null;

        block = GridController.instance.GetBlock(toolIndicator.position.x - .5f, toolIndicator.position.y - .5f);

	    // block.PloughSoil();

		toolWaitCounter = toolWaitTime;

        if (block != null)
        {
            switch(CurrentTool)
            {
                case ToolType.plough:

                    block.PloughSoil();

                    anim.SetTrigger("usePlough");

                    break;

				case ToolType.wateringCan:

                    block.WaterSoil();

                    anim.SetTrigger("useWateringCan");

					break;

				case ToolType.seeds:

                    if (CropController.Instance.GetCropInfo(seedCropType).seedAmount > 0)
                    {
						block.PlantCrop(seedCropType);

						CropController.Instance.UseSeed(seedCropType);
					}

                    break;

				case ToolType.basket:

                    block.HarvestCrop();

					break;
			}
        }
    }
}
