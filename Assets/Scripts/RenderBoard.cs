using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderBoard : MonoBehaviour {

#region prefab
    public GameObject groundPrefab;
    public GameObject wallPrefab;
    public GameObject shapeCirclePrefab;
    public GameObject shapeSquarePrefab;
    public GameObject shapeTrianglePrefab;
    public GameObject shapeHexagonPrefab;

    public GameObject robotsPrefab_R;
    public GameObject robotsPrefab_G;
    public GameObject robotsPrefab_B;
    public GameObject robotsPrefab_Y;
    public GameObject robotsPrefab_W;
#endregion

    public Dictionary<string,GameObject> robotRender = null;
    public GameObject wall = null;
    public GameObject token = null;
    public int[] lastIndex = new int[5];

    public Game game;
    public Board board;

    public static RenderBoard instance = null;

    private void Awake()
    {
        game = FindObjectOfType<Game>();
        board = FindObjectOfType<Board>();

        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private int[] indexToXY(int index)
    {
        return Board.Posxy(index, 16);
    }

    private void generateGround()
    {
        GameObject ground = new GameObject();
        ground.name = "Ground";
        ground.transform.SetParent(this.transform);

        for(int i=0; i<16; i++)
            for(int j=0; j<16; j++)
            {
                GameObject groundTmp = Instantiate<GameObject>(groundPrefab);
                groundTmp.name = "Ground_" + j + "_" + i;
                groundTmp.transform.position = new Vector3(j, 0, 15 - i);
                groundTmp.transform.parent = ground.transform;

                if (new List<int> { 119, 120, 135, 136 }.Contains(j*16+i))
                    groundTmp.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.black);
                    
            }
    }

    public void generateWall()
    {
        if (wall != null)
            Destroy(wall);

        wall = new GameObject();
        wall.name = "Wall";
        wall.transform.SetParent(this.transform);

        bool isInstToken = false;

        for (int i = 0; i < 16; i++)
            for (int j = 0; j < 16; j++)
                foreach (char c in board.grid[j * 16 + i])
                    if (!"RGBYCQTHX".Contains(c.ToString()))
                    {
                        GameObject wallTmp = Instantiate<GameObject>(wallPrefab);
                        wallTmp.name = "Wall_" + (j * 16 + i) + c.ToString();
                        wallTmp.transform.position = new Vector3(i, 0.5f, 15 - j + 0.5f);
                        if (c.ToString() == "E")
                        {
                            wallTmp.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                            wallTmp.transform.position = wallTmp.transform.position + new Vector3(0.5f, 0f, -0.5f);
                        }
                        else if (c.ToString() == "W")
                        {
                            wallTmp.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                            wallTmp.transform.position = wallTmp.transform.position + new Vector3(-0.5f, 0f, -0.5f);
                        }
                        else if (c.ToString() == "S")
                        {
                            wallTmp.transform.position = wallTmp.transform.position + new Vector3(0f, 0f, -1f);
                        }
                        wallTmp.transform.parent = wall.transform;
                    }
                    else if(board.grid[j * 16 + i].Contains(game.token) && !isInstToken) //create token
                    {
                        createToken(j * 16 + i);
                        isInstToken = true;
                    }

                        
    }

    void createToken(int index)
    {
        if (token != null)
            Destroy(token);

        if (board.grid[index].Contains("C"))
            token = Instantiate<GameObject>(shapeCirclePrefab);
        else if (board.grid[index].Contains("Q"))
            token = Instantiate<GameObject>(shapeSquarePrefab);
        else if (board.grid[index].Contains("T"))
            token = Instantiate<GameObject>(shapeTrianglePrefab);
        else if (board.grid[index].Contains("H"))
            token = Instantiate<GameObject>(shapeHexagonPrefab);

        if (board.grid[index].Contains("R"))
            token.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
        else if (board.grid[index].Contains("G"))
            token.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
        else if (board.grid[index].Contains("B"))
            token.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.blue);
        else if (board.grid[index].Contains("Y"))
            token.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.yellow);
        token.name = "Token";
        int[] pos = indexToXY(index);
        token.transform.position = new Vector3(pos[0], 0.5f, 15 - pos[1]);
        token.transform.parent = transform;
    }

    public void positionRobots()
    {

        if (robotRender == null)
        {
            robotRender = new Dictionary<string, GameObject>();
            foreach (string c in game.robots.Keys)
            {
                GameObject robotsTmp = null;
                if (c == "R")
                    robotsTmp = Instantiate<GameObject>(robotsPrefab_R);        
                else if (c == "G")
                    robotsTmp = Instantiate<GameObject>(robotsPrefab_G);
                else if (c == "B")
                    robotsTmp = Instantiate<GameObject>(robotsPrefab_B);
                else if (c == "Y")
                    robotsTmp = Instantiate<GameObject>(robotsPrefab_Y);
                else if (c == "W")
                    robotsTmp = Instantiate<GameObject>(robotsPrefab_W);

                robotsTmp.name = "Robot_" + c;
                int index = game.robots[c];
                int[] pos = Board.Posxy(index, 16);
                robotsTmp.transform.position = new Vector3(pos[0], 0, 15 - pos[1]);
                robotsTmp.transform.parent = transform;
                robotRender.Add(c, robotsTmp);
                clearTrail(robotsTmp);
            }
        }
        else
            foreach (string c in game.robots.Keys)
            {
                int index = game.robots[c];
                int[] pos = indexToXY(index);
                robotRender[c].transform.position = new Vector3(pos[0], 0, 15 - pos[1]);
                robotRender[c].transform.parent = transform;
                clearTrail(robotRender[c]);
            }

        
    }

    public void clearTrail(GameObject g)
    {
        g.GetComponentInChildren<TrailRenderer>().Clear();
        //g.GetComponent<TrailRenderer>().enabled = true;
    }

        
    // Use this for initialization
    void Start () {

        while (!game.newGame());

        generateGround();
        generateWall();
        positionRobots();

    }
	
	// Update is called once per frame
	void Update () {
        
        //positionRobots();
    }
}
