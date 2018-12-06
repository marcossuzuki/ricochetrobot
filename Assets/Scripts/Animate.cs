using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate : MonoBehaviour {

    public Vector3 endPosition;
    Animator animator;
    public float speed;
    private float time = 0f;

    public void Move(int start, int end)
    {
        int[] s = Board.Posxy(start,16);
        int[] e = Board.Posxy(end,16);

        Vector3 direction = new Vector3(e[0] - s[0], 0f, - e[1] + s[1]);
        endPosition = new Vector3(e[0], 0f, 15 - e[1]);

        animator.SetBool("move", true);
        gameObject.transform.forward = direction / direction.magnitude;
        StartCoroutine(run(direction/direction.magnitude, endPosition));
    }

    IEnumerator run(Vector3 direction, Vector3 end)
    {

        while(Vector3.Magnitude(end - gameObject.transform.position)> 0.1)
        {
            time += Time.deltaTime;
            if(time>= 0.01)
            {
                gameObject.transform.position += speed * direction * 0.2f;
                time = 0f;
            }
                
            yield return null;
        }
        time = 0f;
        RenderBoard.instance.positionRobots();
        animator.SetBool("move", false);
    }

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }
}
