using UnityEngine;
using UnityEngine.UI;

public class MainMenuFallingObject : MonoBehaviour
{
	public float minFallSpeed = 100f; // Velocidade mínima de queda
	public float maxFallSpeed = 300f; // Velocidade máxima de queda

	public float minScale = 0.5f;     // Escala mínima
	public float maxScale = 1.5f;     // Escala máxima

	public Sprite[] sprites;          // Array de Sprites para randomizar

	private RectTransform rectTransform;
	private Image imageComponent;

	private float fallSpeed;
	private float rotSpeed;
	private float rotValue;

	private float alphaFadeSpeed = 2f; // Velocidade do fade (desaparecer)

	void Start()
	{
		rectTransform = GetComponent<RectTransform>();
		imageComponent = GetComponent<Image>();

		RandomizeAll(); // Randomiza tudo no início
	}

	void Update()
	{
		// Mover para baixo
		rectTransform.anchoredPosition += Vector2.down * fallSpeed * Time.deltaTime;

		// Atualizar rotação
		rotValue += rotSpeed * Time.deltaTime;
		rectTransform.rotation = Quaternion.Euler(0f, 0f, rotValue);

		// Se passou o fundo, fazer fade-out e reset
		if (rectTransform.anchoredPosition.y < -Screen.height / 2f - 100f)
		{
			StartCoroutine(FadeOutAndReset());
		}
	}

	private void RandomizeAll()
	{
		// Posição aleatória
		rectTransform.anchoredPosition = new Vector2(
			Random.Range(-Screen.width / 2f, Screen.width / 2f),
			Screen.height / 2f + 100f
		);

		// Sprite aleatória
		if (sprites != null && sprites.Length > 0)
		{
			int index = Random.Range(0, sprites.Length);
			imageComponent.sprite = sprites[index];
		}

		// Escala aleatória
		float randomScale = Random.Range(minScale, maxScale);
		rectTransform.localScale = Vector3.one * randomScale;

		// Velocidade de queda aleatória
		fallSpeed = Random.Range(minFallSpeed, maxFallSpeed);

		// Rotação inicial aleatória
		rotValue = Random.Range(0f, 360f);
		rectTransform.rotation = Quaternion.Euler(0f, 0f, rotValue);

		// Velocidade de rotação aleatória
		rotSpeed = Random.Range(-90f, 90f);

		// Resetar alpha para 1
		Color c = imageComponent.color;
		c.a = 1f;
		imageComponent.color = c;
	}

	private System.Collections.IEnumerator FadeOutAndReset()
	{
		// Fazer fade-out
		Color color = imageComponent.color;
		while (color.a > 0f)
		{
			color.a -= Time.deltaTime * alphaFadeSpeed;
			imageComponent.color = color;
			yield return null;
		}

		// Depois de desaparecer, randomizar tudo de novo
		RandomizeAll();
	}
}
