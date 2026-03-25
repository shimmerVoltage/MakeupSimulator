using System.Collections;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	CanvasGroup canvasGroup;

	[SerializeField] private byte creamMoves;
	[SerializeField] private float acneRemoveDuration;
	[SerializeField] private float randomPositionOffset;
	[SerializeField] private Image acneImage;
	[SerializeField] private RectTransform faceTrigger;

	private string faceTriggerName;
	private Vector2 startPosition;
	//private RectTransform faceTriggerRectTransform;

	private void Start()
	{
		faceTriggerName = faceTrigger.name;
		startPosition = transform.position;
		canvasGroup = GetComponent<CanvasGroup>();
		
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		//throw new System.NotImplementedException();
		Debug.Log("OnBeginDrag");
	}

	public void OnDrag(PointerEventData eventData)
	{
		//throw new System.NotImplementedException();
		transform.position = eventData.position;
		//Debug.Log("OnDrag");
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		//throw new System.NotImplementedException();
		//Debug.Log("OnEndDrag");
		//Debug.Log("eventData.pointerEnter?.GetComponent<Transform>().name = " + eventData.pointerEnter?.GetComponent<Transform>().name);

		if (eventData.pointerEnter?.GetComponent<UnityEngine.Transform>().name == faceTriggerName)
		{
			Debug.Log("true");
			//acneObject.GetComponent<Image>().color = Color.blue;
			//acneImage.color = Color.blue;
			StartCoroutine(AcneFadeOut());
			StartCoroutine(CreamAmination());
		}
		else
		{
			Debug.Log("false");
			transform.position = startPosition;
		}
	}

	private IEnumerator AcneFadeOut()
	{
		float elapsed = 0.0f;

		while (elapsed < acneRemoveDuration)
		{
			elapsed += Time.deltaTime;
			float time = elapsed / acneRemoveDuration;
			float newAlpha = Mathf.Lerp(1.0f, 0.0f, time);

			acneImage.color = new Color(
				acneImage.color.r,
				acneImage.color.g,
				acneImage.color.b,
				newAlpha
			);

			yield return null;
		}
	}

	private IEnumerator CreamAmination()
	{
		for (int i = 0; i <= creamMoves; i++)
		{
			float elapsed = 0.0f;

			Vector2 startPosition = transform.position;
			Vector2 targetPosition = Vector2.zero;

			if (i != creamMoves)
			{
				targetPosition = new Vector2
					(
						Random.Range
							(
								faceTrigger.position.x - faceTrigger.rect.width / 2,
								faceTrigger.position.x + faceTrigger.rect.width / 2
							),
						Random.Range
							(
								faceTrigger.position.y - faceTrigger.rect.height / 2,
								faceTrigger.position.y + faceTrigger.rect.height / 2
							)
					);
			}
			else
			{
				targetPosition = this.startPosition;
			}

			while (elapsed < (acneRemoveDuration / creamMoves))
			{
				elapsed += Time.deltaTime;
				float time = elapsed / (acneRemoveDuration / creamMoves);
				Vector2 newPosition = Vector2.Lerp(startPosition, targetPosition, time);
				transform.position = newPosition;

				yield return null;
			}
		}
	}
}
