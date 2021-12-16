using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SO_Data
{
    public List<GI_Particle> AllParticles = new List<GI_Particle>();
    public List<GI_Particle> MovingParticles = new List<GI_Particle>();

    public void RegisterNewParticle(GI_Particle _p)
    {
        if (!AllParticles.Contains(_p))
            AllParticles.Add(_p);

        if (_p.IsMoving && !MovingParticles.Contains(_p))
            MovingParticles.Add(_p);
    }

    public void RemoveParticle(GI_Particle _p)
    {
        if (MovingParticles.Contains(_p))
            MovingParticles.Remove(_p);

        if (AllParticles.Contains(_p))
            AllParticles.Remove(_p);
    }

    public void RemoveMovingParticle(GI_Particle _p)
    {
        if (AllParticles.Contains(_p) && MovingParticles.Contains(_p))
            MovingParticles.Remove(_p);
    }

    public void AddMovingParticle(GI_Particle _p)
    {
        if (AllParticles.Contains(_p) && !MovingParticles.Contains(_p))
            MovingParticles.Add(_p);
    }


    public void GetNextParticlePosition(GI_Particle _p)
    {
        // If below clear && in boundaries
        if (!AllParticles.Find(p => p.Position == _p.Position + Vector2.down) &&
            GI_Manager.I.InBound(_p.Position + Vector2.down))
        {
            // Sets the particle position and register it
            _p.transform.position = _p.Position + Vector2.down;
            _p.Position = _p.Position + Vector2.down;

            _p.SinceLastMove = 0;
            //particleBuffer = null;
            return;
        }

        else
        {
            // Random to sometimes start left, sometimes start right
            int _rnd = UnityEngine.Random.Range(0, 2);
            if (_rnd == 0)
            {
                // If down left clear && in boundaries
                if (!AllParticles.Find(p => p.Position == _p.Position + Vector2.down + Vector2.left) &&
                   GI_Manager.I.InBound(_p.Position + Vector2.down + Vector2.left))
                {
                    // Sets the particle position and register it
                    _p.transform.position = _p.Position + Vector2.down + Vector2.left;
                    _p.Position = _p.Position + Vector2.down + Vector2.left;

                    _p.SinceLastMove = 0;
                    //particleBuffer = null;
                    return;
                }

                // Else if right clear && in boundaries
                else if (!AllParticles.Find(p => p.Position == _p.Position + Vector2.down + Vector2.right) &&
                    GI_Manager.I.InBound(_p.Position + Vector2.down + Vector2.right))
                {
                    // Sets the particle position and register it
                    _p.transform.position = _p.Position + Vector2.down + Vector2.right;
                    _p.Position = _p.Position + Vector2.down + Vector2.right;

                    _p.SinceLastMove = 0;
                    //particleBuffer = null;
                    return;
                }
            }

            else
            {
                // If right clear && in boundaries
                if (!AllParticles.Find(p => p.Position == _p.Position + Vector2.down + Vector2.right) &&
                    GI_Manager.I.InBound(_p.Position + Vector2.down + Vector2.right))
                {
                    // Sets the particle position and register it
                    _p.transform.position = _p.Position + Vector2.down + Vector2.right;
                    _p.Position = _p.Position + Vector2.down + Vector2.right;

                    _p.SinceLastMove = 0;
                    //particleBuffer = null;
                    return;
                }

                // Else if down left clear && in boundaries
                else if (!AllParticles.Find(p => p.Position == _p.Position + Vector2.down + Vector2.left) &&
                   GI_Manager.I.InBound(_p.Position + Vector2.down + Vector2.left))
                {
                    // Sets the particle position and register it
                    _p.transform.position = _p.Position + Vector2.down + Vector2.left;
                    _p.Position = _p.Position + Vector2.down + Vector2.left;

                    _p.SinceLastMove = 0;
                    //particleBuffer = null;
                    return;
                }
            }
        }

        // If nothing is clear
        _p.SinceLastMove++;

        // Remove particle from moving ones
        if (_p.SinceLastMove > 5)
        {
            RemoveMovingParticle(_p);
            //GI_Particle _buffer = AllParticles.Find(p => p.Position == _p.Position + Vector2.down);
            //if(_buffer)
            //{
            //    if (AllParticles.Contains(_buffer))
            //        AllParticles.Remove(_buffer);
            //}
        }

        _p.IsMoving = false;
        return;
    }

}
