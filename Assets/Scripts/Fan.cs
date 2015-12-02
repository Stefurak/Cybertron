using UnityEngine;
using System.Collections;

public class Fan : MonoBehaviour {

    
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<CharacterLogic>().appliedForces = (Vector3.up * 5);
            Debug.Log("Hit");
        }
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<CharacterLogic>().appliedForces = (Vector3.up * 3);
            Debug.Log("Stay");
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<CharacterLogic>().appliedForces = Vector3.zero;
            Debug.Log("Exit");
        }
    }
}
