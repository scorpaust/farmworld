using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DayTransitionUI : MonoBehaviour
{
	public Image panelImage;
	public TextMeshProUGUI descriptionText;
	public TextMeshProUGUI dayText;

	public float fadeDuration = 1f;
	public float holdDuration = 2f;

	private void Awake()
	{
		descriptionText.alpha = 0f;

		dayText.alpha = 0f;
	}

	public void ShowDayTransition(int dayNumber)
	{
		descriptionText.text = "A new day begins...";
		dayText.text = $"Day {dayNumber}";
		StartCoroutine(FadeSequence());
	}

	private IEnumerator FadeSequence()
	{
		// Fade in
		yield return StartCoroutine(FadeCanvas(0f, 1f));

		// Segura por X segundos
		yield return new WaitForSeconds(holdDuration);

		// Fade out
		yield return StartCoroutine(FadeCanvas(1f, 0f));

		PlayerController.instance.gameObject.transform.position = new Vector3(2.4f, -4.14f, 0f);
	}

	private IEnumerator FadeCanvas(float startAlpha, float endAlpha)
	{
		float elapsed = 0f;
		Color color = panelImage.color;
		Color dayTextColor = dayText.color;
		Color descriptionTextColor = descriptionText.color;

		while (elapsed < fadeDuration)
		{
			float t = elapsed / fadeDuration;
			float alpha = Mathf.Lerp(startAlpha, endAlpha, t);

			color.a = alpha;
			descriptionTextColor.a = alpha;
			dayTextColor.a = alpha;

			panelImage.color = color;
			descriptionText.color = descriptionTextColor;
			dayText.color = dayTextColor;

			elapsed += Time.deltaTime;
			yield return null;
		}

		// Garante o valor final
		color.a = endAlpha;
		descriptionTextColor.a = endAlpha;
		dayTextColor.a = endAlpha;
		panelImage.color = color;
		dayText.color = dayTextColor;
		descriptionText.color = descriptionTextColor;

		TimeController.Instance.StartDay();
	}
}
