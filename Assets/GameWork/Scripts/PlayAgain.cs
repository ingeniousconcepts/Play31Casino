using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAgain : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void restartLevel(){

		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene (scene.name);
	}
}
