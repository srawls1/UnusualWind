using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private SceneReference sceneToLoad;

	#endregion // Editor Fields

	public void LoadNextScene()
	{
		SceneManager.LoadScene(sceneToLoad.BuildIndex);
	}
}