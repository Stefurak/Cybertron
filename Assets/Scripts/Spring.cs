using UnityEngine;
using System.Collections;

public class Spring : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Debug.Log(collider.gameObject.GetComponent<CharacterLogic>().moveDirection.y);
            collider.gameObject.GetComponent<CharacterLogic>().appliedForces.y = -(collider.gameObject.GetComponent<CharacterLogic>().moveDirection.y) * 0.5f;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            collider.gameObject.GetComponent<CharacterLogic>().appliedForces.y = 0;
        }
    }
}
