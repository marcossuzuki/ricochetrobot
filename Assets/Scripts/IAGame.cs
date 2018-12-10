using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IAGame : MonoBehaviour {

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
    public Dictionary<string, int> robots = new Dictionary<string, int>();
    public string token;
    public string[] last;

    #region singleton
    public static IAGame instance = null;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }
    #endregion

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
        //if (verifyLast(color, IABoard.reverse[direction]))
        //    return false;

        int index = robots[color];
        if (IABoard.instance.grid[index].Contains(direction))
            return false;

        int newIndex = index + IABoard.offset[direction];
        if (robots.ContainsValue(newIndex))
            return false;

        return true;
    }

    public int computeMove(string color, string direction)
    {
        int index = robots[color];
        while (true)
        {
            if (IABoard.instance.grid[index].Contains(direction))
                break;

            int newIndex = index + IABoard.offset[direction];
            if (robots.ContainsValue(newIndex))
                break;

            index = newIndex;
        }
        return index;
    }

    public string[] doMove(string color, string direction)
    {
        if (!canMove(color, direction))
            return null;

        int start = robots[color];
        
        int end = computeMove(color, direction);
        if (start == end)
            return null;

        updateLast(color, direction);
        robots[color] = end;
        histories.Add(new History(color, direction, start));
        return new string[2] { color, direction};
    }

    public void undoMove()
    {
        if (histories.Count > 0)
        { 
            History undo = histories.Last();

            updateLast(undo.color, undo.direction);
            robots[undo.color] = undo.start;

            int index = histories.LastIndexOf(undo);
            histories.RemoveAt(index);
        }
    }

    public List<string[]> getMoves(string[] colors)
    {
        List<string[]> result = new List<string[]>();
        foreach (string c in colors)
            foreach (string d in IABoard.directions)
                if (canMove(c, d))
                    result.Add(new string[2] { c, d });

        return result;
    }

    public bool over()
    {
        string color = token[0].ToString();
        return IABoard.instance.grid[robots[color]].Contains(token);
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

}
