using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goals : MonoBehaviour {

    public enum Geometry
    {
        SoftStar,
        Diamondo,
        SphereGemLarge,
        CubieBeveled,
        Heart
    }

    public enum Color
    {
        Blue,
        Red,
        Green,
        Yellow,
        White,
        Rainbow
    }

    public Color c;
    public Geometry g;
}
