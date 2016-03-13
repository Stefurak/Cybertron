using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuScript2 : MonoBehaviour {

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
        SceneManager.LoadScene("Level 1");
    }
}
