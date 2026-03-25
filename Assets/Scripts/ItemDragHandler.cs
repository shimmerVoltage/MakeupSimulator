using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	CanvasGroup canvasGroup;

	[SerializeField] private float acneRemoveDuration;
	[SerializeField] private Transform faceTrigger;
	[SerializeField] private Image acneImage;

	private string faceTriggerName;
	private Vector2 startPosition;

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

		if (eventData.pointerEnter?.GetComponent<Transform>().name == faceTriggerName)
		{
			Debug.Log("true");
			//acneObject.GetComponent<Image>().color = Color.blue;
			//acneImage.color = Color.blue;
			StartCoroutine(AcneFadeOut());
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
}
