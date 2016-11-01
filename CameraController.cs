using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    public GameObject player;

    public float cameraSpeed;
    private float step;

    private Vector3 target;
    private Vector3 behindPlayerPosition;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {

        target = player.transform.position - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, target, cameraSpeed*Time.deltaTime, cameraSpeed*Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(newDir);

        behindPlayerPosition = player.transform.position - player.transform.forward * 10f + player.transform.up * 5f;
        transform.position = Vector3.MoveTowards(transform.position, behindPlayerPosition, cameraSpeed*Time.deltaTime);

    }

}
