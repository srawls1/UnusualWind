using UnityEngine;

public class SceneLoaderZone : MonoBehaviour
{
	[SerializeField] private SceneLoadChannelSO channel;
	[SerializeField] private SceneLoadCommand command;
	[SerializeField] private SceneReference scene;
	[SerializeField] private string tagToCheck;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(tagToCheck))
		{
			channel.Broadcast(command, scene);
		}
	}
}
