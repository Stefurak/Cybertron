using UnityEngine;
using System.Collections;

public class Instantiation : MonoBehaviour
{
    public Transform wall;
    public Transform ground;
    public Transform pillar;
    public Transform fan;
    public Transform spring;
    public Transform exit;


    void Start()
    {
        GameObject.FindGameObjectWithTag("EditorOnly").GetComponent<GameSaveLoad>().forcedLoad();
        GameObject.FindGameObjectWithTag("Score").GetComponent<GUIText>().text = "Coins: " + GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterLogic>().score.ToString();
        GameObject.FindGameObjectWithTag("Level").GetComponent<GUIText>().text = "Level " + GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterLogic>().level.ToString();
        
        //First Segment
        Instantiate(wall, new Vector3(-5, 3, 40), Quaternion.Euler(0, 90, 0));
        Instantiate(ground, new Vector3(0, 0, 40), Quaternion.identity);
        Instantiate(fan, new Vector3(0, 1, 70), Quaternion.Euler(-90, 0, 0));

        //Second Segment
        Instantiate(wall, new Vector3(-5, 18, 130), Quaternion.Euler(0, 90, 0));
        Instantiate(ground, new Vector3(0, 15, 130), Quaternion.identity);

        //Third Segment
        Instantiate(wall, new Vector3(50, 18, 185), Quaternion.Euler(0, 0, 0));
        Instantiate(ground, new Vector3(50, 15, 180), Quaternion.Euler(0,90,0));
        Instantiate(spring, new Vector3(95, 15, 180), Quaternion.identity);

        //Fourth Segment
        Instantiate(wall, new Vector3(100, 33, 120), Quaternion.Euler(0, 90, 0));
        Instantiate(ground, new Vector3(95, 30, 120), Quaternion.identity);

        //Fifth Segment
        Instantiate(wall, new Vector3(100, 3, 20), Quaternion.Euler(0, 90, 0));
        Instantiate(ground, new Vector3(95, 0, 20), Quaternion.identity);
        Instantiate(exit, new Vector3(95, 1.5f, -30), Quaternion.identity);
    }
}
