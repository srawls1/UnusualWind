using UnityEngine;

[ExecuteAlways]
public class ChangeMeshRendererLayer : MonoBehaviour
{
    [SerializeField] private string layerName;
    [SerializeField] private int orderInLayer;

	private void OnEnable()
	{
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.sortingLayerName = layerName;
        renderer.sortingOrder = orderInLayer;
	}
}
