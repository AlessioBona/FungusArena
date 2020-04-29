using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerScript : MonoBehaviour
{

    [Header("Fungus variables")]
    public float sporulation = .5f;
    public float toxin = .5f;
    public float resistance = .5f;
    public float overgrowth = .5f;
    public int startX, startY;
    public float growthRate;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CalculateGrowthRate()
    {
        growthRate = 0.6f - sporulation - .1f - (overgrowth - .25f) - toxin - .1f - resistance - .1f;
    }
    
}
