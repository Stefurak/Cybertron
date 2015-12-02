using UnityEngine;
using System.Collections;

public class Instantiation : MonoBehaviour
{
    public Transform wall;
    public Transform ground;
    public Transform pillar;
    public Transform fan;
    public Transform Spring;

    void Start()
    {
        GameObject.FindGameObjectWithTag("EditorOnly").GetComponent<GameSaveLoad>().forcedLoad();
        GameObject.FindGameObjectWithTag("Score").GetComponent<GUIText>().text = "Coins: " + GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterLogic>().score.ToString();
        GameObject.FindGameObjectWithTag("Level").GetComponent<GUIText>().text = "Level " + GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterLogic>().level.ToString();
        Instantiate(wall, new Vector3(-5, 3, 40), Quaternion.Euler(0, 90, 0));
        Instantiate(ground, new Vector3(0, 0, 40), Quaternion.identity);
        Instantiate(fan, new Vector3(0, 1.5f, 50), Quaternion.Euler(-90, 0, 0));
        Instantiate(Spring, new Vector3(0, 3, 75), Quaternion.Euler(0, 0, 0));
        Instantiate(ground, new Vector3(0, 20, 130), Quaternion.identity);
        Instantiate(ground, new Vector3(0, 0, 250), Quaternion.identity);
    }
}
