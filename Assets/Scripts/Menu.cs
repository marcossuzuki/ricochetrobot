using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public Camera cam;
	// Use this for initialization
	void Start () {
        cam = FindObjectOfType<Camera>();
	}

    // Update is called once per frame
    private void LateUpdate()
    {
        if (transform.position == new Vector3(7.5f, 40, 2.5f)) {
            StartCoroutine("cameraMove");
        }
    }
    private void Update ()
    {
        if (Input.GetKeyUp("n"))
        {
            transform.position = new Vector3(7.5f, 40, 2.5f);
            transform.rotation = Quaternion.Euler( new Vector3(0, 0, 0));
        }
    }

    public IEnumerator cameraMove()
    {
        yield return new WaitForSecondsRealtime(1);
        SceneManager.LoadScene("boardGame");
        
    }
}
