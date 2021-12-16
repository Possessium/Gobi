using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GI_Particle : MonoBehaviour
{
    public Vector2 Position;
    public int Velocity = 3;
    public int SinceLastMove = 0;
    public bool IsMoving = false;

    private void Start()
    {
        Position = new Vector2((int)transform.position.x, (int)transform.position.y);
    }

    private void Update()
    {
        //UpdateParticle();
    }

    

    //private void OnDestroy()
    //{
    //    GI_Manager.I.Data.RemoveParticle(this);
    //}

    public void UpdateParticle()
    {
        if (IsMoving && !System.Array.Find(GI_Manager.I.Data.AllParticles.ToArray(), p => p.Position == Position + Vector2.down))
            GI_Manager.I.Data.AddMovingParticle(this);
    }
}
