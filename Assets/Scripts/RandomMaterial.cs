using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMaterial : MonoBehaviour {

    public Material material;
    public MeshRenderer meshRenderer;
    public Color[] colors;
    public int index;
    // Use this for initialization
    void Start () {
        meshRenderer = GetComponent<MeshRenderer>();
        material = meshRenderer.material;
        index = (int)Random.Range(1f, (float)colors.Length+1) -1;
        material.SetColor("_Color", colors[index]);
        meshRenderer.material = material;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
