using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IABoard : MonoBehaviour {

    public static IABoard instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }


    #region direções
    static string NORTH = "N";
    static string EAST = "E";
    static string SOUTH = "S";
    static string WEST = "W";

    public static string[] directions = { NORTH, EAST, SOUTH, WEST };
    public static Dictionary<string, string> reverse = new Dictionary<string, string>()
        {
            { NORTH, SOUTH },
            { EAST,  WEST  },
            { SOUTH, NORTH },
            { WEST,  EAST  }
        };

    public static Dictionary<string, int> offset = new Dictionary<string, int>()
        {
            { NORTH,-16 },
            { SOUTH, 16 },
            { EAST,  1  },
            { WEST, -1  }
        };
    #endregion

    #region cores
    static string RED = "R";
    static string GREEN = "G";
    static string BLUE = "B";
    static string YELLOW = "Y";
    static string WHITE = "W";

    public static string[] colors = { RED, GREEN, BLUE, YELLOW, WHITE };
    #endregion

    #region formas

    public string token;

    #endregion

    public string[] grid;

   

   
}
