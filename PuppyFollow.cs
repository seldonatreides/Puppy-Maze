using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class PuppyFollow : MonoBehaviour
{
    public float MoveSpeed;
    public float FollowTorque;

    public float MinDist;
    public float MaxDist;

    public GameObject PuppyToFollow;
    public GameObject followedBy;

    public bool isFollowing;
    public bool isFollowed;
    public bool isFollowingPlayer;

    public GameObject MainPuppy;

    private float DistToPlayer;
    private float step;
    private Vector3 moveStep;
    private Vector3 newDir;
    private Vector3 targetDir;
    private float angle;
    public Vector3 dogButtOffset;
    //private Vector3 offsetTarget;

    public AudioClip urfBark;
    public AudioClip arfBark;
    public AudioClip whineBark;
    private AudioSource bark;

    Animator anim;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        bark = GetComponent<AudioSource>();

        isFollowing = false;
        isFollowed = false;
        
    }

    void OnEnable()
    {
        EventManager.OnBarked += Startled;
    }

    void OnDisable()
    {
        EventManager.OnBarked -= Startled;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    void OnTriggerStay(Collider other)
    {
        //Is this puppy touching the player?
        if (other.gameObject.CompareTag("Player"))

        {
            if(isFollowing)
            {
                //Is the player being followed by this puppy?
                if (other.GetComponent<PlayerController>().isFollowed & PuppyToFollow == other.gameObject)
                {
                    MoveToward(PuppyToFollow);
                    isFollowingPlayer = true;
                }

            }
            //If the player isn't followed and this puppy isn't following.
            else if (!isFollowing & !other.GetComponent<PlayerController>().isFollowed)
            {
                Follow(other.gameObject);
                //increment count of puppies collected
                //MainPuppy.GetComponent<PlayerController>().count++;
                //MainPuppy.GetComponent<PlayerController>().SetCountText();
                bark.PlayOneShot(urfBark);
            }

        }

        //Is this puppy touching another puppy?
        if (other.gameObject.CompareTag("Puppy"))
        {
            //Is this puppy following another?
            if (isFollowing)
            {
                //If the other is being followed by this one, then move towards
                if (other.GetComponent<PuppyFollow>().isFollowed & PuppyToFollow == other.gameObject)
                {
                    MoveToward(PuppyToFollow);
                    
                    if (other.GetComponent<PuppyFollow>().isFollowingPlayer)
                    {
                        isFollowingPlayer = true;
                    }
                    else
                    {
                        isFollowingPlayer = false;
                    }

                }
                //if
                else if (!isFollowingPlayer & other.GetComponent<PuppyFollow>().isFollowingPlayer & !other.GetComponent<PuppyFollow>().isFollowed)
                {
                    unFollow(PuppyToFollow);
                    Follow(other.gameObject);

                }


            }
            //Is this puppy not following?
            else
            {
                //If the other isn't being followed but is following another, and we aren't following, then follow the other
                if (!other.GetComponent<PuppyFollow>().isFollowed & other.GetComponent<PuppyFollow>().isFollowing)
                {

                    Follow(other.gameObject);
                    //increment count of puppies collected
                    //MainPuppy.GetComponent<PlayerController>().count++;
                    //MainPuppy.GetComponent<PlayerController>().SetCountText();
                    bark.PlayOneShot(arfBark);
                }

            }
        }


            
    }

    /*
    Ability for puppies to be removed from the chain removed on 10/31/16 for more consistent gameplay


    void OnTriggerExit(Collider other)
    {


        unFollow(other.gameObject);

    }
    */

    void MoveToward(GameObject other) {

        dogButtOffset = other.transform.forward * -2f;
        targetDir = other.transform.position - transform.position + dogButtOffset;

        DistToPlayer = Vector3.Distance(transform.position, other.transform.position);
        if (DistToPlayer >= MinDist & DistToPlayer <= MaxDist)
        {
            moveStep = targetDir * Time.deltaTime;
            transform.position = transform.position + moveStep;
            anim.SetFloat("Speed", moveStep.magnitude);
            
            step = FollowTorque * Time.deltaTime;
            newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);

            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }

    void Startled()
    {
        isFollowed = false;
        isFollowing = false;
        isFollowingPlayer = false;
        followedBy = null;
        PuppyToFollow = null;
        MainPuppy.GetComponent<PlayerController>().isFollowed = false;
        MainPuppy.GetComponent<PlayerController>().followedBy = null;
        transform.position = transform.position - transform.forward * 1.5f;
        MainPuppy.GetComponent<PlayerController>().count = 0;
    }
    void Follow(GameObject otherObject)
    {


        if (otherObject == MainPuppy)
        {
            otherObject.GetComponent<PlayerController>().isFollowed = true;
            otherObject.GetComponent<PlayerController>().followedBy = gameObject;
            
            isFollowingPlayer = true;
            PuppyToFollow = otherObject;
            isFollowing = true;
        }
        else if(otherObject.CompareTag("Puppy"))
        {
            otherObject.GetComponent<PuppyFollow>().followedBy = gameObject;
            otherObject.GetComponent<PuppyFollow>().isFollowed = true;
            PuppyToFollow = otherObject;
            isFollowing = true;
        }

    }

    void unFollow(GameObject otherObject)
    {
        if(otherObject == PuppyToFollow)
        {
            if (otherObject == MainPuppy)
            {
                otherObject.GetComponent<PlayerController>().followedBy = null;
                otherObject.GetComponent<PlayerController>().isFollowed = false;
                PuppyToFollow = null;
                isFollowing = false;
                isFollowingPlayer = false;
            }
            else if (otherObject.CompareTag("Puppy"))
            {
                otherObject.GetComponent<PuppyFollow>().followedBy = null;
                otherObject.GetComponent<PuppyFollow>().isFollowed = false;
                PuppyToFollow = null;
                isFollowing = false;
                isFollowingPlayer = false;
            }
        }
 
    }
}

