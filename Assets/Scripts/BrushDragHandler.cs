using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BrushDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

	[SerializeField] private float brushMoveToButtonTime;
	[SerializeField] private float brushRotationDegree;
	[SerializeField] private Vector2 brushOnShadowButtonOffset;
	[SerializeField] private List<GameObject> eyeShadowsList;
	[SerializeField] private List<Transform> buttonTransformsList;
	//[SerializeField] private RectTransform faceTrigger;

	//private string faceTriggerName;
	//private Vector2 startPosition;

	private void Start()
	{
		//faceTriggerName = faceTrigger.name;
		//startPosition = transform.position;
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
		//if (eventData.pointerEnter?.GetComponent<UnityEngine.Transform>().name == faceTriggerName)
		//{
		//
		//}
		//else
		//{
		//	transform.position = startPosition;
		//}
	}

	private IEnumerator BrushMoveToButton(int i)
	{
		float elapsed = 0.0f;

		Vector2 startPosition = transform.position;
		Vector2 targetPosition = buttonTransformsList[i].position;
		
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
	}

	private void DisableShadowObjects()
	{
		foreach(GameObject shadow in eyeShadowsList)
			shadow.SetActive(false);
	}

	private void ShadowButtonCkicked(int i)
	{
		StartCoroutine(BrushMoveToButton(i));

		//DisableShadowObjects();
		//eyeShadowsList[i].SetActive(true);
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
