using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Game : MonoBehaviour {


    public struct History
    {
        public string color;
        public string direction;
        public int start;
        public History(string c, string d, int s)
        {
            color = c;
            direction = d;
            start = s;
        }
    };
    
    public List<History> histories = new List<History>();
    public List<string> listMoves = new List<string>();
    public Dictionary<string, int> robots;
    public Dictionary<string, string> dText = new Dictionary<string, string>()
    {
        {"N", "N"},
        {"S", "S"},
        {"W", "W"},
        {"E", "E" }
    };
    public static List<int> center = new List<int> { 119, 120, 135, 136 };
    public string token;
    public int moves;
    public string[] last;
    public string activeRobot = "";
    public string direction = "";

    public Text movesUI;
    public Text historyUI;
    public bool solved = false;
    public GameObject finishMsg;

    public int[] robotsIndex = new int[5];

    #region singleton
    public static Game instance = null;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        try
        {
            movesUI = GameObject.Find("Moves_text").GetComponent<Text>();
            historyUI = GameObject.Find("History_text").GetComponent<Text>();
            finishMsg = GameObject.Find("FinishMsg");
            finishMsg.SetActive(false);
        }
        catch { }
        
        
    }
#endregion

    System.Random rnd = new System.Random();

    private Dictionary<string, int> PlaceRobots()
    {
        Dictionary<string, int> result = new Dictionary<string, int>
        {
            {Board.colors[0], 0 },
            {Board.colors[1], 0 },
            {Board.colors[2], 0 },
            {Board.colors[3], 0 },
            {Board.colors[4], 0 }
        };
        List<int> used = new List<int>();
        List<string> tokens = new List<string>();

        foreach (string t in Board.instance.tokens)
            tokens.Add(t);

        int index;

        
        foreach (string c in Board.colors)
        {
            while (true)
            {       
                index = rnd.Next(0, 256);

                if (center.Contains(index))
                    continue;

                string gridToken = Board.instance.grid[index];
                gridToken = gridToken.Substring(Math.Max(0, gridToken.Length - 2));
                if (tokens.Contains(gridToken))
                    continue;

                if (used.Contains(index))
                    continue;
                
                result[c] = index;
                used.Add(index);
                break;
            }
        }
        return result;
    }

    public string getRobots(int index)
    {
        string result = "";

        foreach (string c in robots.Keys)
            if (index == robots[c])
                result = c;
        
        return result;
    }

    public bool canMove(string color, string direction)
    {
        if (verifyLast(color, Board.reverse[direction]))
            return false;

        int index = robots[color];
        if (Board.instance.grid[index].Contains(direction))
            return false;

        int newIndex = index + Board.offset[direction];
        if(robots.ContainsValue(newIndex))
            return false;

        return true;
    }

    public int computeMove(string color, string direction)
    {
        int index = robots[color];
        while (true)
        {
            if (Board.instance.grid[index].Contains(direction))
                break;

            int newIndex = index + Board.offset[direction];
            if (robots.ContainsValue(newIndex))
                break;

            index = newIndex;
        }
        RenderBoard.instance.robotRender[color].gameObject.GetComponent<Animate>().Move(robots[color], index);
        return index;
    }

    public History doMove(string color, string direction)
    {
        if(!canMove(color, direction))
            return new History();

        int start = robots[color];

        if (verifyLast(color, Board.reverse[direction]))
            return new History();

        int end = computeMove(color, direction);
        if (start == end)
            return new History();

        updateLast(color, direction);
        updateMoves(moves+1);
        robots[color] = end;
        addListMoves(color, direction);
        histories.Add(new History(color, direction, start));
        return new History(color, direction, start);
    }

    public void undoMove()
    {
        if (moves > 0)
        {
            History undo = histories.Last();

            updateMoves(moves-1);
            updateLast(undo.color, undo.direction);
            robots[undo.color] = undo.start;

            int index = histories.LastIndexOf(undo);
            histories.RemoveAt(index);
            removeLastListMoves();
        }
    }

    public  List<string[]> getMoves(string[] colors)
    {
        List<string[]> result = new List<string[]>();
        foreach (string c in colors)
            foreach (string d in Board.directions)
                if (canMove(c, d))
                    result.Add(new string[2] {c, d});

        return result;
    }

    public bool over()
    {
        string color = token[0].ToString();

        return Board.instance.grid[robots[color]].Contains(token);
    }

    public bool newGame()
    {
        Board.instance.newBoard();
        token = Board.instance.token;
        updateMoves(0);
        last = new string[2] { "", "" };
        histories.Clear();
        listMoves.Clear();
        updateHistoryMsg();
        robots = PlaceRobots();
        RenderBoard.instance.generateWall();
        RenderBoard.instance.positionRobots();
        try
        {
            IAGame.instance.robots = new Dictionary<string, int>(robots);
            IAGame.instance.token = token;
            IAGame.instance.histories.Clear();
            IAGame.instance.last = last;
            IABoard.instance.grid = Board.instance.grid;
            IABoard.instance.token = Board.instance.token;
        }
        catch { }
        return true;
    }

    void keyPress()
    {
        if (solved)
        {
            if (Input.GetKeyUp("n"))
            {
                newGame();
                solved = false;
                finishMsg.SetActive(false);
            }
            else if (Input.GetKeyUp("t"))
            {
                while(histories.Count>0)
                    undoMove();
                RenderBoard.instance.positionRobots();
                solved = false;
                finishMsg.SetActive(false);
            }
            return;
        }
            

        foreach (string c in RenderBoard.instance.robotRender.Keys)
        {
            if (RenderBoard.instance.robotRender[c].GetComponent<Animator>().GetBool("move") == true)
                return;
        }

        if (Input.GetKeyUp("b"))
            activeRobot = "B";
        else if (Input.GetKeyUp("r"))
            activeRobot = "R";
        else if (Input.GetKeyUp("g"))
            activeRobot = "G";
        else if (Input.GetKeyUp("y"))
            activeRobot = "Y";
        else if (Input.GetKeyUp("w"))
            activeRobot = "W";
        else if (Input.GetKeyUp("u"))
        {
            undoMove();
            RenderBoard.instance.positionRobots();
            activeRobot = "";
        }
        else if (Input.GetKeyUp("n"))
            while (!newGame()) ;
        else if (Input.GetKeyUp("s"))
            Solver.instance.solve(IAGame.instance);
            



        if (activeRobot != "")
        {
            if (Input.GetKeyUp(KeyCode.UpArrow))
                direction = "N";
            else if (Input.GetKeyUp(KeyCode.DownArrow))
                direction = "S";
            else if (Input.GetKeyUp(KeyCode.RightArrow))
                direction = "E";
            else if (Input.GetKeyUp(KeyCode.LeftArrow))
                direction = "W";
        }

        if (activeRobot != "" && direction != "")
        {
            doMove(activeRobot, direction);
            direction = "";
        }
            
    }

    public void updateMoves(int m)
    {
        try
        {
            moves = m;
            movesUI.text = "Moves: " + moves;
        }catch{ }
    }

    public void addListMoves(string color, string direction)
    {
        listMoves.Add(color + dText[direction]);
        updateHistoryMsg();
    }

    public void removeLastListMoves()
    {
        listMoves.RemoveAt(listMoves.Count - 1);
        updateHistoryMsg();
    }


    public void updateHistoryMsg()
    {
        try
        {
            historyUI.text = "";
            foreach (string s in listMoves)
                historyUI.text += s + "  ";
        }
        catch { }
    }

    public void updateLast(string color, string direction)
    {
        last[0] = color;
        last[1] = direction;
    }

    public bool verifyLast(string color, string direction)
    {
        return (last[0] == color && last[1] == direction);
    }

    public void Update()
    {
        keyPress();
        int i = 0;
        foreach (string k in robots.Keys)
        {
            robotsIndex[i] = robots[k];
            i++;
        }
        if (over() && !solved)
        {
            solved = true;
            finishMsg.SetActive(true);
        }
    }

}
