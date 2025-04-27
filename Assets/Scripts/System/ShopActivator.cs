using UnityEngine;
using UnityEngine.InputSystem;

public class ShopActivator : MonoBehaviour
{
	private bool canOpen;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			canOpen = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			canOpen = false;
		}
	}

	private void Update()
	{
		if (canOpen)
		{
			if (Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.eKey.wasPressedThisFrame)
			{
				if (!UIController.Instance.ShopControl.gameObject.activeSelf)
				{
					UIController.Instance.ShopControl.OpenClose();

					AudioManager.Instance.PlaySFX(0);
				}
			}
		}
	}
}
