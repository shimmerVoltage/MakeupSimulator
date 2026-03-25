using UnityEngine;
using System.Collections.Generic;

public class BrushDragHandler : MonoBehaviour//, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[SerializeField] private List<GameObject> eyeShadowsList;

	private void DisableShadowObjects()
	{
		foreach(GameObject shadow in eyeShadowsList)
			shadow.SetActive(false);
	}

	private void ShadowButtonCkicked(int i)
	{
		DisableShadowObjects();
		eyeShadowsList[i].SetActive(true);
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

	

	//[SerializeField] private RectTransform faceTrigger;

	//private string faceTriggerName;
	//private Vector2 startPosition;

	

	//private void Start()
	//{
	//	//faceTriggerName = faceTrigger.name;
	//	startPosition = transform.position;
	//}
	//
	//public void OnBeginDrag(PointerEventData eventData)
	//{
	//	Debug.Log("OnBeginDrag");
	//}
	//
	//public void OnDrag(PointerEventData eventData)
	//{
	//	transform.position = eventData.position;
	//}
	//
	//public void OnEndDrag(PointerEventData eventData)
	//{
	//	if (eventData.pointerEnter?.GetComponent<UnityEngine.Transform>().name == faceTriggerName)
	//	{
	//		
	//	}
	//	else
	//	{
	//		transform.position = startPosition;
	//	}
	//}
}
