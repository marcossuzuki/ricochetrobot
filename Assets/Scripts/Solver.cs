using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Solver: MonoBehaviour
{

    public static Solver instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public List<string[]> solve(IAGame game)
    {
        int maxDepth = 1;
        
        while (maxDepth<=6)
        {
            Debug.Log("Buscando profundiade: " + maxDepth);
            List<string[]> result;
            result = search(game, new List<string>(), 0, maxDepth);
            if (result!=null)
                return result;
                
            maxDepth++;
        }
        return null;
    }

    public List<string[]> search(IAGame game, List<string> history, int depth, int maxDepth)
    {
        if (game.over())
        {
            List<string[]> path = new List<string[]>();
            foreach (IAGame.History s in game.histories)
            {
                path.Add(new string[2] {s.color, s.direction});
            }
            return path;
        }
            
 
        if (depth == maxDepth)
            return null;

        string state = "";
        foreach (int i in game.robots.Values)
            state += i + ",";
        state += depth;

        if(depth > 1 && history.Contains(state))
            return null;
        history.Add(state);

        List<string[]> moves = game.getMoves(new string[5] {"R", "G", "B", "Y", "W"});
        
        foreach (string[] move in moves)
        {
            game.doMove(move[0], move[1]);
            List<string[]> result = search(game, history, depth + 1, maxDepth);
            game.undoMove();
            if (result != null)
                return result;
        }
 
        return null;
    }


}
