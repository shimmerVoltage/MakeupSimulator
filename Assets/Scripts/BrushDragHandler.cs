using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BrushDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

	[SerializeField] private byte brushRotations;
	[SerializeField] private byte brushPaintRotations;
	[SerializeField] private float brushMoveToButtonTime;
	[SerializeField] private float brushRotationDegree;
	[SerializeField] private float brushRotationOnButtonDegree;
	[SerializeField] private float brushRotateTime;
	[SerializeField] private float brushMoveToCenterTime;
	[SerializeField] private float faceTriggerHandleOffset;
	[SerializeField] private float paintAngleDegree;
	[SerializeField] private float brushMoveToPaintTime;
	[SerializeField] private float brushPaintRotation;
	[SerializeField] private float brushPaintRotateTime;
	[SerializeField] private float brushMoveToStartTime;
	[SerializeField] private float shadowsFadeDuration;
	[SerializeField] private Vector2 brushOnShadowButtonOffset;
	[SerializeField] private Vector2 brushHandlePivot;
	[SerializeField] private RectTransform brushCenterPosition;
	[SerializeField] private RectTransform faceTrigger;
	[SerializeField] private RectTransform paintPosition;
	[SerializeField] private List<GameObject> eyeShadowsList;
	[SerializeField] private List<Transform> buttonTransformsList;

	private bool isPrepared;
	private int shadowIndex;
	private string faceTriggerName;
	private Vector2 defaulPivot;
	private Vector2 faceTriggerDefaultPosition;
	private Vector2 startPosition;
	private RectTransform rectTransform;


	private void Awake()
	{
		isPrepared = false;
		faceTriggerDefaultPosition = faceTrigger.position;
		faceTriggerName = faceTrigger.name;
		rectTransform = GetComponent<RectTransform>();
		startPosition = transform.position;

		defaulPivot = rectTransform.pivot;
	}

	private void Start()
	{

	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		rectTransform.pivot = brushHandlePivot;
		faceTrigger.position = new Vector2(faceTriggerDefaultPosition.x, faceTriggerDefaultPosition.y - faceTriggerHandleOffset);
	}

	public void OnDrag(PointerEventData eventData)
	{
		transform.position = eventData.position;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		//rectTransform.pivot = defaulPivot;
		faceTrigger.position = faceTriggerDefaultPosition;

		//Debug.Log("faceTriggerName = " + faceTriggerName);
		//Debug.Log("eventData.pointerEnter?.GetComponent<UnityEngine.Transform>().name = " + eventData.pointerEnter?.GetComponent<UnityEngine.Transform>().name);

		if (isPrepared)
		{
			if (eventData.pointerEnter?.GetComponent<UnityEngine.Transform>().name == faceTriggerName)
			{
				StartCoroutine(BrushMoveToPaint());
			}
			else
			{
				transform.position = brushCenterPosition.position;
			}
		}
	}

	private IEnumerator ShadowsFadeIn()
	{
		float elapsed = 0.0f;

		while (elapsed < shadowsFadeDuration)
		{
			elapsed += Time.deltaTime;
			float time = elapsed / shadowsFadeDuration;
			float newAlpha = Mathf.Lerp(0.0f, 1.0f, time);

			Image leftShadow = eyeShadowsList[shadowIndex].transform.Find("Left").GetComponent<Image>();
			Image rightShadow = eyeShadowsList[shadowIndex].transform.Find("Right").GetComponent<Image>();

			leftShadow.color = new Color(
				leftShadow.color.r,
				leftShadow.color.g,
				leftShadow.color.b,
				newAlpha
			);

			rightShadow.color = new Color(
				rightShadow.color.r,
				rightShadow.color.g,
				rightShadow.color.b,
				newAlpha
			);

			yield return null;
		}
	}

	private IEnumerator BrushMoveToStart()
	{
		float elapsed = 0.0f;
		float startAngle = transform.eulerAngles.z;
		Vector2 startPosition = transform.position;
		Vector2 targetPosition = this.startPosition;

		while (elapsed < brushMoveToStartTime)
		{
			elapsed += Time.deltaTime;
			float time = elapsed / brushMoveToStartTime;

			float smoothTime = Mathf.SmoothStep(0f, 1f, time);

			float currentAngle = Mathf.Lerp(startAngle, 0.0f, smoothTime);
			Vector2 newPosition = Vector2.Lerp(startPosition, targetPosition, smoothTime);

			transform.eulerAngles = new Vector3(0, 0, currentAngle);
			transform.position = newPosition;

			yield return null;
		}
	}

	private IEnumerator BrushPaint()
	{
		StartCoroutine(ShadowsFadeIn());

		float elapsed = 0.0f;
		float startAngle = transform.eulerAngles.z;

		while (elapsed < brushPaintRotateTime / 2)
		{
			elapsed += Time.deltaTime;
			float time = elapsed / (brushPaintRotateTime / 2);

			float smoothTime = Mathf.SmoothStep(0f, 1f, time);

			float currentAngle = Mathf.Lerp(startAngle, startAngle + brushPaintRotation / 2, smoothTime);
			transform.eulerAngles = new Vector3(0, 0, currentAngle);

			yield return null;
		}

		startAngle = transform.eulerAngles.z;

		for (int i = 0; i < brushPaintRotations; i++)
		{
			elapsed = 0.0f;

			while (elapsed < brushPaintRotateTime / 2)
			{
				elapsed += Time.deltaTime;
				float time = elapsed / (brushPaintRotateTime / 2);

				float smoothTime = Mathf.SmoothStep(0f, 1f, time);

				float currentAngle = Mathf.Lerp(startAngle, startAngle - brushPaintRotation, smoothTime);
				transform.eulerAngles = new Vector3(0, 0, currentAngle);

				yield return null;
			}

			elapsed = 0.0f;

			while (elapsed < brushPaintRotateTime / 2)
			{
				elapsed += Time.deltaTime;
				float time = elapsed / (brushPaintRotateTime / 2);

				float smoothTime = Mathf.SmoothStep(0f, 1f, time);

				float currentAngle = Mathf.Lerp(startAngle - brushPaintRotation, startAngle, smoothTime);
				transform.eulerAngles = new Vector3(0, 0, currentAngle);

				yield return null;
			}
		}

		elapsed = 0.0f;

		while (elapsed < brushPaintRotateTime / 2)
		{
			elapsed += Time.deltaTime;
			float time = elapsed / (brushPaintRotateTime / 2);

			float smoothTime = Mathf.SmoothStep(0f, 1f, time);

			float currentAngle = Mathf.Lerp(startAngle, startAngle - brushPaintRotation / 2, smoothTime);
			transform.eulerAngles = new Vector3(0, 0, currentAngle);

			yield return null;
		}

		StartCoroutine(BrushMoveToStart());
	}

	private IEnumerator BrushMoveToPaint()
	{
		float elapsed = 0.0f;

		Vector2 startPosition = transform.position;
		Vector2 targetPosition = paintPosition.position;
		Vector2 startPivot = rectTransform.pivot;

		while (elapsed < brushMoveToPaintTime)
		{
			elapsed += Time.deltaTime;
			float time = elapsed / brushMoveToPaintTime;

			float smoothTime = Mathf.SmoothStep(0f, 1f, time);

			float currentAngle = Mathf.Lerp(0.0f, paintAngleDegree, smoothTime);
			Vector2 newPosition = Vector2.Lerp(startPosition, targetPosition, smoothTime);
			Vector2 newPivot = Vector2.Lerp(startPivot, defaulPivot, smoothTime);

			transform.eulerAngles = new Vector3(0, 0, currentAngle);
			transform.position = newPosition;
			rectTransform.pivot = newPivot;	

			yield return null;
		}

		StartCoroutine(BrushPaint());
	}

	private IEnumerator BrushMoveToCenter()
	{
		float elapsed = 0.0f;
		float startAngle = transform.eulerAngles.z;
		Vector2 startPosition = transform.position;
		Vector2 targetPosition = brushCenterPosition.position;

		while (elapsed < brushMoveToCenterTime)
		{
			elapsed += Time.deltaTime;
			float time = elapsed / brushMoveToCenterTime;

			float smoothTime = Mathf.SmoothStep(0f, 1f, time);

			float currentAngle = Mathf.Lerp(startAngle, 0.0f, smoothTime);
			Vector2 newPosition = Vector2.Lerp(startPosition, targetPosition, smoothTime);

			transform.eulerAngles = new Vector3(0, 0, currentAngle);
			transform.position = newPosition;

			yield return null;
		}
		
		isPrepared = true;
	}

	private IEnumerator BrushRotation()
	{
		float elapsed = 0.0f;
		float startAngle = transform.eulerAngles.z;

		while (elapsed < brushRotateTime / 2)
		{
			elapsed += Time.deltaTime;
			float time = elapsed / (brushRotateTime / 2);

			float smoothTime = Mathf.SmoothStep(0f, 1f, time);

			float currentAngle = Mathf.Lerp(startAngle, startAngle + brushRotationOnButtonDegree / 2, smoothTime);
			transform.eulerAngles = new Vector3(0, 0, currentAngle);

			yield return null;
		}

		startAngle = transform.eulerAngles.z;

		for (int i = 0; i < brushRotations; i++)
		{
			elapsed = 0.0f;

			while (elapsed < brushRotateTime / 2)
			{
				elapsed += Time.deltaTime;
				float time = elapsed / (brushRotateTime / 2);

				float smoothTime = Mathf.SmoothStep(0f, 1f, time);

				float currentAngle = Mathf.Lerp(startAngle, startAngle - brushRotationOnButtonDegree, smoothTime);
				transform.eulerAngles = new Vector3(0, 0, currentAngle);

				yield return null;
			}

			elapsed = 0.0f;			
			
			while (elapsed < brushRotateTime / 2)
			{
				elapsed += Time.deltaTime;
				float time = elapsed / (brushRotateTime / 2);
			
				float smoothTime = Mathf.SmoothStep(0f, 1f, time);
			
				float currentAngle = Mathf.Lerp(startAngle - brushRotationOnButtonDegree, startAngle, smoothTime);
				transform.eulerAngles = new Vector3(0, 0, currentAngle);
			
				yield return null;
			}
		}

		elapsed = 0.0f;

		while (elapsed < brushRotateTime / 2)
		{
			elapsed += Time.deltaTime;
			float time = elapsed / (brushRotateTime / 2);

			float smoothTime = Mathf.SmoothStep(0f, 1f, time);

			float currentAngle = Mathf.Lerp(startAngle, startAngle - brushRotationOnButtonDegree / 2, smoothTime);
			transform.eulerAngles = new Vector3(0, 0, currentAngle);

			yield return null;
		}

		StartCoroutine(BrushMoveToCenter());
	}

	private IEnumerator BrushMoveToButton()
	{
		float elapsed = 0.0f;

		Vector2 startPosition = transform.position;
		Vector2 targetPosition = buttonTransformsList[shadowIndex].position;

		targetPosition += brushOnShadowButtonOffset;

		while (elapsed < brushMoveToButtonTime)
		{
			elapsed += Time.deltaTime;
			float time = elapsed / brushMoveToButtonTime;

			float smoothTime = Mathf.SmoothStep(0f, 1f, time);

			float currentAngle = Mathf.Lerp(0.0f, brushRotationDegree, smoothTime);
			Vector2 newPosition = Vector2.Lerp(startPosition, targetPosition, smoothTime);

			transform.eulerAngles = new Vector3(0, 0, currentAngle);
			transform.position = newPosition;

			yield return null;
		}

		StartCoroutine(BrushRotation());
	}

	private void DisableShadowObjects()
	{
		foreach (GameObject shadow in eyeShadowsList)
		{
			Image leftShadow = shadow.transform.Find("Left").GetComponent<Image>();
			Image rightShadow = shadow.transform.Find("Right").GetComponent<Image>();

			leftShadow.color = new Color(
				leftShadow.color.r,
				leftShadow.color.g,
				leftShadow.color.b,
				0.0f
			);

			rightShadow.color = new Color(
				rightShadow.color.r,
				rightShadow.color.g,
				rightShadow.color.b,
				0.0f
			);

			shadow.SetActive(false);
		}
	}

	private void BrushPreparation()
	{
		StartCoroutine(BrushMoveToButton());
	}

	private void ShadowButtonCkicked(int index)
	{
		shadowIndex = index;
		BrushPreparation();
		DisableShadowObjects();
		eyeShadowsList[index].SetActive(true);
	}

	public void Shadow_01()
	{
		ShadowButtonCkicked(0);
	}
	public void Shadow_02()
	{
		ShadowButtonCkicked(1);
	}
	public void Shadow_03()
	{
		ShadowButtonCkicked(2);
	}
	public void Shadow_04()
	{
		ShadowButtonCkicked(3);
	}
	public void Shadow_05()
	{
		ShadowButtonCkicked(4);
	}
	public void Shadow_06()
	{
		ShadowButtonCkicked(5);
	}
	public void Shadow_07()
	{
		ShadowButtonCkicked(6);
	}
	public void Shadow_08()
	{
		ShadowButtonCkicked(7);
	}
	public void Shadow_09()
	{
		ShadowButtonCkicked(8);
	}

}
