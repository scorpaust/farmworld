using UnityEngine;
using UnityEngine.InputSystem;

public class BedController : MonoBehaviour
{
    private bool canSleep;

    // Update is called once per frame
    void Update()
    {
        if (canSleep)
		{
			if ((Mouse.current.leftButton.wasPressedThisFrame) || (Keyboard.current.spaceKey.wasPressedThisFrame) || (Keyboard.current.eKey.wasPressedThisFrame))
			{
				if (TimeController.Instance != null)
				{
					TimeController.Instance.EndDay();
				}
			}

		}
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
        {
            canSleep = true;
        }
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			canSleep = false;
		}
	}
}
