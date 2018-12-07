using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


public class Board: MonoBehaviour
{
    public static Board instance = null;

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

    #region mascara
    /*
    static int M_NORTH = 0x01;
    static int M_EAST = 0x02;
    static int M_SOUTH = 0x04;
    static int M_WEST = 0x08;
    static int M_ROBOT = 0x10;

    Dictionary<string, int> M_LOOKUP = new Dictionary<string, int>
    {
            { NORTH, M_NORTH},
            { SOUTH, M_EAST },
            { EAST,  M_SOUTH},
            { WEST,  M_WEST }
    };
    */
    #endregion

    #region cores
    static string RED    = "R";
    static string GREEN  = "G";
    static string BLUE   = "B";
    static string YELLOW = "Y";
    static string WHITE  = "W";

    public static string[] colors = { RED, GREEN, BLUE, YELLOW, WHITE };
    #endregion

    #region formas
    static string CIRCLE   = "C";
    static string TRIANGLE = "T";
    static string SQUARE   = "Q";
    static string HEXAGON  = "H";

    static string[] shapes = { CIRCLE, TRIANGLE, SQUARE, HEXAGON };

    public string[] tokens;

    public string token;

    #endregion

    #region quadrantes
    static string[] QUAD_1A =
    {
        "NW","N","N","N","NE","NW","N","N",
        "W","S","X","X","X","X","SEYH","W",
        "WE","NWGT","X","X","X","X","N","X",
        "W","X","X","X","X","X","X","X",
        "W","X","X","X","X","X","S","X",
        "SW","X","X","X","X","X","NEBQ","W",
        "NW","X","E","SWRC","X","X","X","S",
        "W","X","X","N","X","X","E","NW"
    };

    static string[] QUAD_1B =
    {
        "NW","NE","NW","N","NS","N","N","N",
        "W","S","X","E","NWRC","X","X","X",
        "W","NEGT","W","X","X","X","X","X",
        "W","X","X","X","X","X","SEYH","W",
        "W","X","X","X","X","X","N","X",
        "SW","X","X","X","X","X","X","X",
        "NW","X","E","SWBQ","X","X","X","S",
        "W","X","X","N","X","X","E","NW"
    };

    static string[] QUAD_2A =
    {
        "NW","N","N","NE","NW","N","N","N",
        "W","X","X","X","X","E","SWBC","X",
        "W","S","X","X","X","X","N","X",
        "W","NEYT","W","X","X","S","X","X",
        "W","X","X","X","E","NWGQ","X","X",
        "W","X","SERH","W","X","X","X","X",
        "SW","X","N","X","X","X","X","S",
        "NW","X","X","X","X","X","E","NW"
    };

    static string[] QUAD_2B =
    {
        "NW","N","N","N","NE","NW","N","N",
        "W","X","SERH","W","X","X","X","X",
        "W","X","N","X","X","X","X","X",
        "WE","SWGQ","X","X","X","X","S","X",
        "SW","N","X","X","X","E","NWYT","X",
        "NW","X","X","X","X","S","X","X",
        "W","X","X","X","X","NEBC","W","S",
        "W","X","X","X","X","X","E","NW"
    };

    static string[] QUAD_3A =
    {
        "NW","N","N","NE","NW","N","N","N",
        "W","X","X","X","X","SEGH","W","X",
        "WE","SWRQ","X","X","X","N","X","X",
        "SW","N","X","X","X","X","S","X",
        "NW","X","X","X","X","E","NWYC","X",
        "W","X","S","X","X","X","X","X",
        "W","X","NEBT","W","X","X","X","S",
        "W","X","X","X","X","X","E","NW"
    };

    static string[] QUAD_3B =
    {
        "NW","N","NS","N","NE","NW","N","N",
        "W","E","NWYC","X","X","X","X","X",
        "W","X","X","X","X","X","X","X",
        "W","X","X","X","X","E","SWBT","X",
        "SW","X","X","X","S","X","N","X",
        "NW","X","X","X","NERQ","W","X","X",
        "W","SEGH","W","X","X","X","X","S",
        "W","N","X","X","X","X","E","NW"
    };

    static string[] QUAD_4A =
    {
        "NW","N","N","NE","NW","N","N","N",
        "W","X","X","X","X","X","X","X",
        "W","X","X","X","X","SEBH","W","X",
        "W","X","S","X","X","N","X","X",
        "SW","X","NEGC","W","X","X","X","X",
        "NW","S","X","X","X","X","E","SWRT",
        "WE","NWYQ","X","X","X","X","X","NS",
        "W","X","X","X","X","X","E","NW"
    };

    static string[] QUAD_4B =
    {
        "NW","N","N","NE","NW","N","N","N",
        "WE","SWRT","X","X","X","X","S","X",
        "W","N","X","X","X","X","NEGC","W",
        "W","X","X","X","X","X","X","X",
        "W","X","SEBH","W","X","X","X","S",
        "SW","X","N","X","X","X","E","NWYQ",
        "NW","X","X","X","X","X","X","S",
        "W","X","X","X","X","X","E","NW"
    };

    Dictionary<int, string[]> QUADS = new Dictionary<int, string[]>
    {
        {0, QUAD_1A}, {1, QUAD_1B}, {2, QUAD_2A}, {3, QUAD_2B},
        {4, QUAD_3A}, {5, QUAD_3B}, {6, QUAD_4A}, {7, QUAD_4B},
    };
    #endregion

    #region rotações
    int[] ROTATE_QUAD = new int[64]
    {
        56, 48, 40, 32, 24, 16,  8,  0,
        57, 49, 41, 33, 25, 17,  9,  1,
        58, 50, 42, 34, 26, 18, 10,  2,
        59, 51, 43, 35, 27, 19, 11,  3,
        60, 52, 44, 36, 28, 20, 12,  4,
        61, 53, 45, 37, 29, 21, 13,  5,
        62, 54, 46, 38, 30, 22, 14,  6,
        63, 55, 47, 39, 31, 23, 15,  7,
    };

    Dictionary<string, string> ROTATE_WALL = new Dictionary<string, string>
    {
        { NORTH, EAST },
        { EAST, SOUTH },
        { SOUTH, WEST },
        { WEST, NORTH }
    };
    #endregion

    private System.Random rnd = new System.Random();

    public void newBoard()
    {
        tokens = tokensCombinations(colors, shapes);
        while(!CreateGrid());
        token = tokens[rnd.Next(0,tokens.Length)];
    }

    #region Helper Functions
    int IDx(int x, int y, int size)
    {
        return y * size + x;
    }

    public static int[] Posxy(int index, int size)
    {
        int x = index % size;
        int y = index / size;
        return new int[2] { x, y };
    }

    string[] tokensCombinations(string[] colors, string[] shapes)
    {
        string[] aux = new string[(colors.Length - 1) * shapes.Length];
        int count = 0;
        foreach (string c in colors)
        {
            if (c != "W")
                foreach (string s in shapes)
                {
                    aux[count] = c.ToString() + s.ToString();
                    count++;
                }
        }
        return aux;
    }
    #endregion

    public string[] grid;

    private string[] RotateQuad(string[] data, int times)
    {
        string[] quad = new string[64];

        string[]  newData = new string[64];

        for (int j = 0; j < quad.Length; j++)
        {
            quad[j] = data[j];
            newData[j] = data[j];
        }
            

        for (int i = 0; i < times; i++)
        {
            for (int j = 0; j < quad.Length; j++)
            {
                quad[j] = newData[ROTATE_QUAD[j]];
                if (quad[j] != "X")
                {
                    string rotate = "";
                    foreach (char c in quad[j])
                    {
                        string aux = c.ToString();
                        if (aux == "N" || aux == "S" || aux == "W" || aux == "E")
                            aux = ROTATE_WALL[aux];
                        rotate += aux;
                    }
                    quad[j] = rotate;
                }
            }
            for (int j = 0; j < quad.Length; j++)
                newData[j] = quad[j];
        }

        if (times == 0)
            for (int j = 0; j < quad.Length; j++)
                quad[j] = data[j];

        return quad;
    }

    public bool CreateGrid()
    {
        
        int[] quads = new int[4];

        //escolhe entre 2 de cada quadrante
        quads[0] = rnd.Next(0, 2);
        quads[1] = rnd.Next(2, 4);
        quads[2] = rnd.Next(4, 6);
        quads[3] = rnd.Next(6, 8);
        
        //embaralha quatro quadrantes
        for (int i = 0; i < 4; i++)
        {
            int a = rnd.Next(0, 4);
            int b = rnd.Next(0, 4);
            int aux = quads[a];
            quads[a] = quads[b];
            quads[b] = aux;
        }
       
        string[] quad0 = RotateQuad(QUADS[quads[0]], 0);
        string[] quad1 = RotateQuad(QUADS[quads[1]], 1);
        string[] quad2 = RotateQuad(QUADS[quads[2]], 3);
        string[] quad3 = RotateQuad(QUADS[quads[3]], 2);
        
        Dictionary<int, string[]> quadrantes = new Dictionary<int, string[]>
        {
            {0, quad0},
            {1, quad1},
            {2, quad2},
            {3, quad3}
        };

        grid = new string[256];
        for (int i=0; i < 4; i++)
        {
            string[] quadrante = new string[64];
            quadrante = quadrantes[i];
            int[] delta = Posxy(i, 2);

            for (int j = 0; j < quadrante.Length; j++)
            {
                int[] pos = Posxy(j,8);
                pos[0] += delta[0] * 8;
                pos[1] += delta[1] * 8;
                int index = IDx(pos[0], pos[1], 16);
                grid[index] = quadrante[j];
            }
        }
        return true;
    }

}
