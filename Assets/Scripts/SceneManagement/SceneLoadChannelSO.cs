using UnityEngine;
using UnityEngine.Events;

public enum SceneLoadCommand
{
    Load,
    Unload
}

[CreateAssetMenu]
public class SceneLoadChannelSO : ScriptableObject
{
    [SerializeField] private UnityEvent<SceneLoadCommand, SceneReference> onChannelBroadcast;

    public void Subscribe(UnityAction<SceneLoadCommand, SceneReference> action)
	{
        onChannelBroadcast.AddListener(action);
	}

    public void Unsubscribe(UnityAction<SceneLoadCommand, SceneReference> action)
	{
        onChannelBroadcast.RemoveListener(action);
	}

    public void Broadcast(SceneLoadCommand command, SceneReference scene)
	{
        onChannelBroadcast.Invoke(command, scene);
	}
}
