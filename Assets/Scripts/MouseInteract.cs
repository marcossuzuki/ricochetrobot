using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInteract : MonoBehaviour
{
    public GameObject selection;
    public GameObject selectedGO;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                GameObject go = hitInfo.transform.gameObject;
                Debug.Log("Hit " + go.name);
                if (go.tag == "Player")
                {
                    selectedGO = go;
                    selection.transform.position = go.transform.position;
                    selection.SetActive(true);
                    Debug.Log("It's working!");
                }
                else
                {
                    selectedGO.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    if (go.name == "ArrowRight")
                    {
                        selection.SetActive(false);
                        selectedGO.GetComponent<Rigidbody>().velocity = 10*Vector3.right;
                    }
                    else if(go.name == "ArrowLeft")
                    {
                        selection.SetActive(false);
                        selectedGO.GetComponent<Rigidbody>().velocity = 10 * Vector3.left;
                    }
                    else if (go.name == "ArrowUp")
                    {
                        selection.SetActive(false);
                        selectedGO.GetComponent<Rigidbody>().velocity = 10 * Vector3.forward;
                    }
                    else if (go.name == "ArrowDown")
                    {
                        selection.SetActive(false);
                        selectedGO.GetComponent<Rigidbody>().velocity = 10 * Vector3.back;
                    }
                }
            }
            else
            {
                Debug.Log("No hit");
            }

        }
    }
}