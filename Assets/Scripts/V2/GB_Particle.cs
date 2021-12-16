using System;
using System.Collections.Generic;
using UnityEngine;

public class GB_Particle : MonoBehaviour
{
    public Vector2 Position
    {
        get => new Vector2((int)transform.position.x, (int)transform.position.y);
        set => transform.position = value;
    }

    public int Velocity = 3;



}

