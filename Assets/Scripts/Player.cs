using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        CheckCollisionPlayer();

    }

   /* private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("wall") || other.CompareTag("Player"))
        {
            FixPposisition();
        }
            
    }
    */
    private void CheckCollisionPlayer()
    {
        RaycastHit hitInfo;
        Vector3 direction = new Vector3();
        if (gameObject.GetComponent<Rigidbody>().velocity.x > 0)
            direction = Vector3.right;
        else if(gameObject.GetComponent<Rigidbody>().velocity.x < 0)
            direction = Vector3.left;
        else if(gameObject.GetComponent<Rigidbody>().velocity.z > 0)
            direction = Vector3.forward;
        else if(gameObject.GetComponent<Rigidbody>().velocity.z < 0)
            direction = Vector3.back;

        Physics.Raycast(gameObject.transform.position, direction, out hitInfo, 1f);

        if (hitInfo.transform != null && (hitInfo.collider.gameObject.CompareTag("Player") || hitInfo.collider.gameObject.CompareTag("wall")))
        {
            FixPposisition();
        }
       


    }

    private void FixPposisition()
    {
        if (gameObject.GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
            RaycastHit hitInfo;
            Physics.Raycast(gameObject.transform.position, Vector3.down, out hitInfo, 2f);
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.transform.position = new Vector3(hitInfo.transform.position.x, gameObject.transform.position.y, hitInfo.transform.position.z);
        }
    }
}
