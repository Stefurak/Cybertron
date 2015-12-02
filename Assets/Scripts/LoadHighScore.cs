using UnityEngine;
using System.Collections;

public class LoadHighScore : MonoBehaviour {
    public float highscore;
    public string name;

    public GUIElement nameBox;
    public GUIElement scoreBox;

	// Use this for initialization
	void Start () {
        GameObject.FindGameObjectWithTag("EditorOnly").GetComponent<GameSaveLoad>().forcedLoad();
        highscore = GameObject.FindGameObjectWithTag("EditorOnly").GetComponent<GameSaveLoad>().highScore;
        name = GameObject.FindGameObjectWithTag("EditorOnly").GetComponent<GameSaveLoad>().PlayerName;

        nameBox.GetComponent<GUIText>().text = name;
        scoreBox.GetComponent<GUIText>().text = "" + highscore;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
