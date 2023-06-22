using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEvent : MonoBehaviour
{
	public void SceneLoader(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}
}