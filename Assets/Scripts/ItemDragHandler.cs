using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	CanvasGroup canvasGroup;

	private void Start()
	{
		canvasGroup = GetComponent<CanvasGroup>();
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		throw new System.NotImplementedException();
	}

	public void OnDrag(PointerEventData eventData)
	{
		//throw new System.NotImplementedException();
		transform.position = eventData.position;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		throw new System.NotImplementedException();
	}
}
