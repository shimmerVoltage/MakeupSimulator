using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreamDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[SerializeField] private byte creamMoves;
	[SerializeField] private byte creamRotations;
	[SerializeField] private float acneRemoveDuration;
	//[SerializeField] private float randomPositionOffset;
	[SerializeField] private float creamRotationDegree;
	[SerializeField] private Image acneImage;
	[SerializeField] private RectTransform faceTrigger;

	private string faceTriggerName;
	private Vector2 startPosition;

	private void Start()
	{
		faceTriggerName = faceTrigger.name;
		startPosition = transform.position;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		Debug.Log("OnBeginDrag");
	}

	public void OnDrag(PointerEventData eventData)
	{
		transform.position = eventData.position;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (eventData.pointerEnter?.GetComponent<UnityEngine.Transform>().name == faceTriggerName)
		{
			StartCoroutine(AcneFadeOut());
			StartCoroutine(CreamMove());
			StartCoroutine(CreamRotation());
		}
		else
		{
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

	private IEnumerator CreamMove()
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
								faceTrigger.position.x - faceTrigger.rect.width / 4,
								faceTrigger.position.x + faceTrigger.rect.width / 4
							),
						Random.Range
							(
								faceTrigger.position.y - faceTrigger.rect.height / 4,
								faceTrigger.position.y + faceTrigger.rect.height / 4
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

				float smoothTime = Mathf.SmoothStep(0f, 1f, time);

				Vector2 newPosition = Vector2.Lerp(startPosition, targetPosition, smoothTime);
				transform.position = newPosition;

				yield return null;
			}
		}
	}

	private IEnumerator CreamRotation()
	{
		float rotateTime = acneRemoveDuration / creamRotations;

		for (int i = 0; i < creamRotations; i++)
		{
			float elapsed = 0.0f;

			while (elapsed < rotateTime / 2)
			{
				elapsed += Time.deltaTime;
				float time = elapsed / (rotateTime / 2);

				float smoothTime = Mathf.SmoothStep(0f, 1f, time);

				float currentAngle = Mathf.Lerp(0.0f, -creamRotationDegree, smoothTime);
				transform.eulerAngles = new Vector3(0, 0, currentAngle);

				yield return null;
			}

			elapsed = 0.0f;

			while (elapsed < rotateTime / 2)
			{
				elapsed += Time.deltaTime;
				float time = elapsed / (rotateTime / 2);

				float smoothTime = Mathf.SmoothStep(0f, 1f, time);

				float currentAngle = Mathf.Lerp(-creamRotationDegree, 0.0f, smoothTime);
				transform.eulerAngles = new Vector3(0, 0, currentAngle);

				yield return null;
			}
		}
	}
}
