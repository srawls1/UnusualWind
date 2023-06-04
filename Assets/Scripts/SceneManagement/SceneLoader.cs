using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private SceneLoadChannelSO sceneLoadChannel;
	[SerializeField] private SceneReference startingScene;

	#endregion // Editor Fields

	#region Private Fields

	private List<Scene> openScenes;
	private List<SceneReference> desiredOpenScenes;

	#endregion // Private Fields

	#region Unity Functions

	private void OnEnable()
	{
		openScenes = new List<Scene>();
		for (int i = 0; i < SceneManager.sceneCount; ++i)
		{
			Scene scene = SceneManager.GetSceneAt(i);
			if (scene.buildIndex != gameObject.scene.buildIndex)
			{
				openScenes.Add(scene);
			}
		}

		desiredOpenScenes = new List<SceneReference>();

		sceneLoadChannel.Subscribe(HandleSceneLoadCommand);
		sceneLoadChannel.Broadcast(SceneLoadCommand.Load, startingScene);
	}

	private void OnDisable()
	{
		sceneLoadChannel.Unsubscribe(HandleSceneLoadCommand);
	}

	#endregion // Unity Functions

	#region Private Functions

	private void HandleSceneLoadCommand(SceneLoadCommand command, SceneReference scene)
	{
		switch (command)
		{
			case SceneLoadCommand.Load:
				if (!desiredOpenScenes.Contains(scene))
				{
					desiredOpenScenes.Add(scene);
				}
				break;
			case SceneLoadCommand.Unload:
				desiredOpenScenes.Remove(scene);
				break;
		}

		StartCoroutine(TransitionScenesImpl(desiredOpenScenes));
	}

	private IEnumerator TransitionScenesImpl(List<SceneReference> scenesThatShouldBeOpen)
	{
		HashSet<int> desiredBuildIndices = ConvertSceneReferenceArrayToBuildIndexSet(scenesThatShouldBeOpen);
		yield return StartCoroutine(UnloadScenes(desiredBuildIndices));
		yield return StartCoroutine(LoadScenes(desiredBuildIndices));
		SceneManager.SetActiveScene(openScenes[openScenes.Count - 1]);
	}

	private HashSet<int> ConvertSceneReferenceArrayToBuildIndexSet(List<SceneReference> sceneReferences)
	{
		HashSet<int> buildIndexSet = new HashSet<int>();

		for (int i = 0; i < sceneReferences.Count; ++i)
		{
			buildIndexSet.Add(sceneReferences[i].BuildIndex);
		}

		return buildIndexSet;
	}

	private IEnumerator UnloadScenes(HashSet<int> desireSceneIndices)
	{
		List<AsyncOperation> unloadOperations = new List<AsyncOperation>();
		for (int i = openScenes.Count; i-- > 0;)
		{
			if (!desireSceneIndices.Contains(openScenes[i].buildIndex))
			{
				unloadOperations.Add(SceneManager.UnloadSceneAsync(openScenes[i]));
				openScenes.RemoveAt(i);
			}
		}
		return unloadOperations.GetEnumerator();
	}

	private IEnumerator LoadScenes(HashSet<int> desiredSceneIndices)
	{
		List<int> openSceneIndices = new List<int>();
		for (int i = 0; i < openScenes.Count; ++i)
		{
			openSceneIndices.Add(openScenes[i].buildIndex);
		}

		desiredSceneIndices.ExceptWith(openSceneIndices);
		foreach (int buildIndex in desiredSceneIndices)
		{
			yield return SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
			openScenes.Add(SceneManager.GetSceneByBuildIndex(buildIndex));
		}
	}

	#endregion // Private Functions
}