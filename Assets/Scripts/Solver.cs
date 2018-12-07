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

    public List<IAGame.History> solve(IAGame game)
    {
        int maxDepth = 1;
        while (maxDepth<30)
        {
            Debug.Log("Buscando profundiade: " + maxDepth);
            List<IAGame.History> result;
            result = search(game, new List<IAGame.History>(), new Dictionary<Dictionary<int[], int>, List<IAGame.History>>(), 0, maxDepth);
            if (result.Count>0)
                return result;
            maxDepth++;
        }
        return new List<IAGame.History>();
    }

    public List<IAGame.History> search(IAGame game, List<IAGame.History> path, Dictionary<Dictionary<int[], int>, List<IAGame.History>> history, int depth, int maxDepth)
    {
        if(game.over())
            return path;

        if (depth == maxDepth)
            return new List<IAGame.History>();

        int[] indexes = game.robots.Values.ToArray<int>();
        Dictionary<int[], int> state = new Dictionary<int[], int>();
        state.Add(indexes, depth);

        if(history.ContainsKey(state))
            return new List<IAGame.History>();
        history.Add(state, path);

        List<string[]> moves = Game.instance.getMoves(new string[5] {"R", "G", "B", "Y", "W"});

        IAGame.History data;

        foreach(string[] move in moves)
        {
            data = game.doMove(move[0], move[1]);
            path.Add(data);
            List<IAGame.History> result = search(game, path, history, depth + 1, maxDepth);
            if (result.Count>0)
                return result;
            path.RemoveAt(path.Count - 1);
            game.undoMove();
        }

        return new List<IAGame.History>();
    }
}
