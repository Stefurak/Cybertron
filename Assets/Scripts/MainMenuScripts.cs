using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuScripts : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void newGame()
    {
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("EditorOnly"));
        SceneManager.LoadScene("Level 1");
    }

    public void highScore()
    {
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("EditorOnly"));
        SceneManager.LoadScene("High Score");
    }
}
