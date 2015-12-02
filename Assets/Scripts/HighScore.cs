using UnityEngine;
using System.Collections;

public class HighScore : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseEnter()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }

    void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }

    void OnMouseDown()
    {
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("EditorOnly"));
        Application.LoadLevel("High Score");
    }
}
