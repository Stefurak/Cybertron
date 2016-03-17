using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CharacterLogic : MonoBehaviour {


    public int score = 0;
    public int level = 1;
    public float timeleft;

    public float speed = 2;
    public float maxSpeed = 10;                 // max speed of the character
    public float acceleration = 1.01f;          // acceleration as you run
    public float deceleration = 0.7f;           // deceleration as you stop
    public float horizontalSpeed;               // current speed of the player
    public float verticalSpeed;                 // the players vertical speed
    public float rotationalSpeed;               // the rotation speed

    public CharacterController playerController;	// what is being controlled
    public Vector3 appliedForces;               // Forces being applied to the Character
    public Vector3 moveDirection = Vector3.zero;// what direction is the character moving in


    private float jumpDelay = 0.7f;                // Time delay between jumping
    private float timeSinceLastJump;            // Time since the last jump was made
    private Vector3 pausedSpeed;                // Stores the speed when paused
    private float startAngle;                   // Angle at which the rotation starts
    private float currentRotation;              // Current progress through rotation
    private bool doubleJump;      			    // should the character be Jumping
    private bool rotating;                      // should the character be rotating

    // Use this for initialization
    void Start () 
	{
        timeleft = 180;
        rotating = false;
        doubleJump = false;
		
        GUIText levelText;
        levelText = GameObject.FindGameObjectWithTag("Level").GetComponent<GUIText>();
        levelText.text = "Level " + level;
		
		// Grab Controller attached to character
		// collisions and what not...
		playerController = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void FixedUpdate() 
	{
        timeleft -= Time.deltaTime;
        GUIText timeText = GameObject.FindGameObjectWithTag("Time").GetComponent<GUIText>();
        timeText.text = "Time Left: " + Mathf.RoundToInt(timeleft);

        if (timeleft <= 0)
        {
            SceneManager.LoadScene("Lose");
        }

        //getInput();
        VerticalMovement();
        HorizontalMovement();
        AppliedForcesMovement();

        verticalSpeed -= 0.5f;

        // Check if Character is on ground
        if (playerController.isGrounded)
        {
            if (verticalSpeed < -5)
            {
                verticalSpeed = -5;
            }

            if (horizontalSpeed == 0)
            {
                //TODO:Idle? Maybe?	
            }
        }

        //check to see if you are rotating before moving
        ScreenRotation();

        //get the direction the player is moving
        moveDirection = playerController.transform.forward * horizontalSpeed;
        moveDirection.y += verticalSpeed;
        // Move Character in specified direction
        playerController.Move(moveDirection * Time.deltaTime);

    } //end Update

    void VerticalMovement()
    {
        timeSinceLastJump += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && playerController.isGrounded)
        {
            verticalSpeed += 20;
            doubleJump = false;
            timeSinceLastJump = 0;
        }
        //Double Jump
        else if (Input.GetKeyDown(KeyCode.Space) && doubleJump == false && timeSinceLastJump > jumpDelay)
        {
            verticalSpeed += 20;
            doubleJump = true;
        }

        else if(Input.GetKeyDown(KeyCode.Space) && doubleJump == false && !playerController.isGrounded)
        {
            verticalSpeed += 20;
            doubleJump = true;
        }
        else if(playerController.isGrounded)
        {
            doubleJump = false;
        }
    }

    void HorizontalMovement()
    {
        // Is Right Key pressed
        if (Input.GetAxis("Horizontal") > .2)
        {
            Debug.Log("moved");
            if (horizontalSpeed == 0)
            {
                horizontalSpeed = speed;
            }
            else if (horizontalSpeed >= speed && horizontalSpeed <= maxSpeed)
            {
                horizontalSpeed *= acceleration;
            }
            else if (horizontalSpeed < -speed)
            {
                horizontalSpeed = horizontalSpeed * deceleration;
            }
            Debug.Log(horizontalSpeed);
        }

        if (Input.GetAxis("Horizontal") < -.2)
        {
            Debug.Log("moved");
            if (horizontalSpeed == 0)
            {
                horizontalSpeed = -speed;
            }
            else if (horizontalSpeed <= -speed && horizontalSpeed >= -maxSpeed)
            {
                horizontalSpeed *= acceleration;
            }
            else if (horizontalSpeed > speed)
            {
                horizontalSpeed = Mathf.Abs(horizontalSpeed) * deceleration;
            }
            Debug.Log(horizontalSpeed);
        }

        //a zeroing factor for deceleration
        if (horizontalSpeed < 5 && horizontalSpeed > -5 && horizontalSpeed != 0)
            horizontalSpeed = 0;

        //Decelerate
        if (Input.GetAxis("Horizontal") == 0 && playerController.isGrounded)
        {
            //left decel
            if (horizontalSpeed < 0 && rotating == false)
            {
                horizontalSpeed *= deceleration;
            }
            //right decel
            else if ( horizontalSpeed > 0 && rotating == false)
            {
                horizontalSpeed = Mathf.Abs(horizontalSpeed) * deceleration;
            }
        }
    }

    void AppliedForcesMovement()
    {
        verticalSpeed += appliedForces.y;
        horizontalSpeed += appliedForces.x;
    }

    void ScreenRotation()
    {
        if(Input.GetKeyDown(KeyCode.E) && !rotating)
        {
            pausedSpeed = new Vector3(horizontalSpeed, verticalSpeed, 0);
            startAngle = transform.rotation.y;
            horizontalSpeed = 0;
            verticalSpeed = 0;
            rotating = true;
            rotationalSpeed = -1.0f;
        }

        else if (Input.GetKeyDown(KeyCode.Q) && !rotating)
        {
            pausedSpeed = new Vector3(horizontalSpeed, verticalSpeed, 0);
            startAngle = transform.rotation.y;
            horizontalSpeed = 0;
            verticalSpeed = 0;
            rotating = true;
            rotationalSpeed = 1.0f;
        }

        else if (rotating && Mathf.Abs(currentRotation) < 90)
        {
            transform.Rotate(new Vector3(0, rotationalSpeed, 0));
            currentRotation += rotationalSpeed;
            horizontalSpeed = 0;
            verticalSpeed = 0;
        }

        else if (rotating && Mathf.Abs(currentRotation) >= 90)
        {
            rotating = false;
            currentRotation = 0;
            horizontalSpeed = pausedSpeed.x;
            verticalSpeed = pausedSpeed.y;
        }

    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Coin")
        {
            score++;
            GUIText guiText;
            guiText = GameObject.FindGameObjectWithTag("Score").GetComponent<GUIText>();
            guiText.text = "Coins: " + score;

            Destroy(hit.gameObject);
        }
        else if (hit.gameObject.tag == "Finish")
        {
            
            DontDestroyOnLoad(GameObject.FindGameObjectWithTag("EditorOnly"));
            if (Application.loadedLevel == 1)
            {
                level++;
                GameObject.FindGameObjectWithTag("EditorOnly").GetComponent<GameSaveLoad>().forcedSave();
                Application.LoadLevel("Level 2");
            }
            else if (Application.loadedLevel == 2)
            {
                GameObject.FindGameObjectWithTag("EditorOnly").GetComponent<GameSaveLoad>().forcedSave();
                Application.LoadLevel("Win");
            }
        }
    }
	
}
