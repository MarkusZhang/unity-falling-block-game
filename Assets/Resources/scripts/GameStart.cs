using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//TODO: rename to StartSceneManager
public class GameStart : MonoBehaviour {

	public void StartGame(){
		SceneManager.LoadScene ("play-scene");
	}

	public void GoToHelp(){
		SceneManager.LoadScene ("help-scene");
	}

	public void BackToMain(){
		SceneManager.LoadScene ("start-scene");
	}
}
