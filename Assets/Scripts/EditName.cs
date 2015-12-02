using UnityEngine;
using System.Collections;

public class EditName : MonoBehaviour {
    public string stringToEdit = "Player Name";
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        stringToEdit = GUI.TextField(new Rect(225, 275, 200, 20), stringToEdit, 25);
    }
}
