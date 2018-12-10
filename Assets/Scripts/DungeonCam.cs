using UnityEngine;
using System.Collections;

public class DungeonCam : MonoBehaviour {

	public Transform player;
	Vector3 offset;
	public float damping = 1;
    public string playerColor;

	// Use this for initialization
	void Start () {
        offset = new Vector3(0f, 0f, -2f);
    }

    // Update is called once per frame
    void Update() {
        try { 
            if (Game.instance.activeRobot != playerColor && Game.instance.activeRobot != "")
            {
                playerColor = Game.instance.activeRobot;
                player = RenderBoard.instance.robotRender[playerColor].transform;
            }
        }catch{}

        if (player != null) { 

            Vector3 desiredPosition = player.position + offset;

            transform.position = Vector3.Lerp(transform.position,
                                              desiredPosition,
                                              damping * Time.deltaTime);
            transform.LookAt(player);

        }
    }
}
