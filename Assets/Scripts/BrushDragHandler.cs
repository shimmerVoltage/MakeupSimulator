using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BrushDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

	[SerializeField] private byte brushRotations;
	[SerializeField] private float brushMoveToButtonTime;
	[SerializeField] private float brushRotationDegree;
	[SerializeField] private float brushRotationOnButtonDegree;
	[SerializeField] private float brushRotateTime;
	[SerializeField] private float brushMoveToCenterTime;
	[SerializeField] private float faceTriggerHandleOffset;
	[SerializeField] private Vector2 brushOnShadowButtonOffset;
	[SerializeField] private Vector2 brushHandlePivot;
	[SerializeField] private RectTransform brushCenterPosition;
	[SerializeField] private RectTransform faceTrigger;
	[SerializeField] private List<GameObject> eyeShadowsList;
	[SerializeField] private List<Transform> buttonTransformsList;

	private bool isPrepared;
	private string faceTriggerName;
	private Vector2 defaulPivot;
	private Vector2 faceTriggerDefaultPosition;
	private RectTransform rectTransform;
	//private Vector2 startPosition;


	private void Awake()
	{
		isPrepared = false;
		faceTriggerDefaultPosition = faceTrigger.position;
		faceTriggerName = faceTrigger.name;
		rectTransform = GetComponent<RectTransform>();

		defaulPivot = rectTransform.pivot;
	}

	private void Start()
	{
		//startPosition = transform.position;
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
		rectTransform.pivot = defaulPivot;
		faceTrigger.position = faceTriggerDefaultPosition;

		if (isPrepared)
			if (eventData.pointerEnter?.GetComponent<UnityEngine.Transform>().name == faceTriggerName)
			{
			
			}
			else
			{
				transform.position = brushCenterPosition.position;
			}
	}

	private IEnumerator BrushMoveToCenter(int index)
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

	private IEnumerator BrushRotation(int index)
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

		StartCoroutine(BrushMoveToCenter(index));
	}

	private IEnumerator BrushMoveToButton(int index)
	{
		float elapsed = 0.0f;

		Vector2 startPosition = transform.position;
		Vector2 targetPosition = buttonTransformsList[index].position;

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

		StartCoroutine(BrushRotation(index));
	}

	private void DisableShadowObjects()
	{
		foreach (GameObject shadow in eyeShadowsList)
			shadow.SetActive(false);
	}

	private void BrushPreparation(int index)
	{
		StartCoroutine(BrushMoveToButton(index));
	}

	private void ShadowButtonCkicked(int index)
	{
		BrushPreparation(index);
		DisableShadowObjects();
		//eyeShadowsList[index].SetActive(true);
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
