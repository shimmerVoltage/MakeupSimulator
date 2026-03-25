using UnityEngine;

public class LoofahScript : MonoBehaviour
{
	[SerializeField] private BrushDragHandler brushDragHandler;

	public void ClearAll()
	{
		brushDragHandler.DisableShadowObjects();
	}
}
