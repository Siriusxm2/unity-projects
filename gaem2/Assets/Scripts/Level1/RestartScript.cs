using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScript : MonoBehaviour {

	public void RestartScene()
    {
		Time.timeScale = 1f;
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.name);
    }

	public void MainMenuScene()
	{
		SceneManager.LoadScene(0, LoadSceneMode.Single);
	}


}
