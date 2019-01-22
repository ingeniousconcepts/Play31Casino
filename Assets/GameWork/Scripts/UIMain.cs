using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// MainScene UI logic.
/// </summary>
public class UIMain : MonoBehaviour
{



    // Use this for initialization
    void Start()
    {
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Starts the game.
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }


}
