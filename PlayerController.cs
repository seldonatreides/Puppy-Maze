using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public float speedScale;
    public float torque;
    public float fadeSpeed;

    public int count;

    public Text countText;
    public Text winText;
    public Text loseText;

    public bool isFollowed;
    public bool escaped;
    public bool running;

    public GameObject followedBy;

    public Vector3 movement;

    Animator anim;

    void Start()
    {

        count = 0;
        countText.text = "";
        winText.text = "";
		anim = GetComponent<Animator>();

        winText.text = "Welcome to the Puppy Maze! \nArrows or WASD to move \nESC to pause";
        fadeText(winText, 10);

    }

    void Update()
    {





    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        float moveVertical = Input.GetAxis("Vertical");
        movement = new Vector3(0.0f, 0.0f, moveVertical) * Time.deltaTime * speedScale;

        if (moveVertical > 0)
        {
            transform.Translate(movement.magnitude * Vector3.forward, Space.Self);
        }
        //reverse motion
        else
        {
            transform.Translate(movement.magnitude * Vector3.forward * (-1), Space.Self);
        }

        float step = torque * Time.deltaTime;
        if (moveHorizontal < 0)
        {
            Vector3 newDir = Vector3.RotateTowards(transform.forward, transform.right * (-1), step, 0.0F);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
        else if (moveHorizontal > 0)
        {
            Vector3 newDir = Vector3.RotateTowards(transform.forward, transform.right, step, 0.0F);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
        
    }



    void LateUpdate()
    {
        anim.SetFloat("Speed", movement.magnitude);

        /*
         * footstep audio to be added later
        if (movement.magnitude > 0.01 & !running)
        {
            sfx.PlayOneShot(thumping);
            running = true;
        }
        */

        SetCountText();
        
    }

    void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.CompareTag("Respawn"))
        {
            escaped = true;
            winText.text = "You have escaped the maze. Now frolic in the ocean!";
            fadeText(winText, 8);
        }
    }

	public void SetCountText()
	{
        count = NumFollowing(followedBy);
        countText.text = "Puppies Remaining: " + (6 - count).ToString();
        fadeText(countText, 7);
        if (count == 1 & !escaped)
        {
            
            winText.text = "You found the first puppy! Collect the rest, then escape the maze!";
            fadeText(winText, 6);
            
        }
        else if (count == 6 & !escaped) {
            
            winText.text = "You found all the puppies! Head for the exit and escape!";
            fadeText(winText, 6);
        }
	}


    public void playerBark()
    {
        
    }
    public void fadeText(Text toFade, float time)
    {
        //instant fade-in
        toFade.CrossFadeAlpha(1, 0.001f, false);
        
        //slow fade out
        toFade.CrossFadeAlpha(0, time, false);
    }

    int NumFollowing(GameObject current)
    {
        if(current == null)
        {
            return 0;
        }
        
        for(int i = 1; i < 10;  i++)
        {
            GameObject next = current.GetComponent<PuppyFollow>().followedBy;
            if (next == null)
                return i;
            current = next;
            
        }
        //If there is a cycle in the list, return a max value
        return 10;

    }
}