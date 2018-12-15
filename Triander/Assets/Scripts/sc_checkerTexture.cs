using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_checkerTexture : MonoBehaviour {

    public Texture2D mainTexture;

    public int mainTexWidth;
    public int mainTexHeight;

    // Use this for initialization
    void Start () 
    {
        setMainTextureSize();
        createPattern();
    }
	
	// Update is called once per frame
	void Update () 
    {		
	}

    void setMainTextureSize()
    {
        mainTexture = new Texture2D(mainTexWidth, mainTexHeight);
    }

    void createPattern()
    {
        for (int i = 0; i < mainTexWidth; i++)
        {
            for (int j = 0; j < mainTexWidth; j++)
            {
                if (((i + j) % 2) == 1)
                    mainTexture.SetPixel(i, j, Color.gray);
                else
                    mainTexture.SetPixel(i, j, Color.white);
            }
        }
        mainTexture.Apply();
        GetComponent<Renderer>().material.mainTexture = mainTexture;
        mainTexture.wrapMode = TextureWrapMode.Clamp;
        mainTexture.filterMode = FilterMode.Point;
    }
}
