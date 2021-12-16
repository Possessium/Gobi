using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GI_Manager : MonoBehaviour
{
    public static GI_Manager I { get; private set; }

    [SerializeField] int delay = 3;
    [SerializeField] GameObject particle = null;

    [SerializeField] int squareBoundaries = 25;

    [SerializeField, Range(1, 50)] int blobSize = 10;
    [SerializeField] SO_Data data = null;
    public SO_Data Data { get { return data; } }
    //public List<GI_Particle> SpawnedParticles { get; private set; } = new List<GI_Particle>();
    //List<GI_Particle> movingParticles = new List<GI_Particle>();

    GI_Particle particleBuffer = null;

    #region Unity
    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        data = new SO_Data();
        //StartCoroutine(SpawnParticles());
    }

    private void OnGUI()
    {
        GUIStyle _style = new GUIStyle();
        _style.fontSize = 50;

        GUI.Label(new Rect(Vector2.zero, new Vector2(500, 100)), (1 / Time.deltaTime).ToString(), _style);
        //GUI.Box(new Rect(Vector2.zero, new Vector2(500,100)), (1 / Time.deltaTime).ToString());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SpawnParticlesBlob(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        for (int i = 0; i < Data.MovingParticles.Count; i++)
        {
            particleBuffer = Data.MovingParticles[i];

            GetNextParticleMove();

            particleBuffer = null;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector3(-squareBoundaries, 0), new Vector3(1, squareBoundaries * 2));
        Gizmos.DrawCube(new Vector3(squareBoundaries, 0), new Vector3(1, squareBoundaries * 2));
        Gizmos.DrawCube(new Vector3(0, -squareBoundaries), new Vector3(squareBoundaries * 2, 1));
        Gizmos.DrawCube(new Vector3(0, squareBoundaries), new Vector3(squareBoundaries * 2, 1));
    }
    #endregion

    void SpawnParticlesBlob(Vector2 _pos)
    {
        _pos.x = (int)_pos.x;
        _pos.y = (int)_pos.y;

        for (int i = -(blobSize / 2); i < blobSize / 2; i++)
        {
            for (int ii = -(blobSize / 2); ii < blobSize / 2; ii++)
            {
                GameObject _go = Instantiate(particle, _pos + new Vector2(i, ii), Quaternion.identity);
                _go.name = _go.name + " " + _go.transform.position;
                if (_go.TryGetComponent(out particleBuffer))
                {
                    particleBuffer.IsMoving = true;
                    particleBuffer.Position = _go.transform.position;
                    Data.RegisterNewParticle(particleBuffer);
                }
            }
        }
    }

    /// <summary>
    /// Return true if the given position is in the boundaries of the game
    /// </summary>
    /// <param name="_pos"></param>
    /// <returns></returns>
    public bool InBound(Vector2 _pos)
    {
        if (_pos.x > -squareBoundaries && _pos.x < squareBoundaries && _pos.y > -squareBoundaries && _pos.x < squareBoundaries)
            return true;

        else
            return false;
    }

    IEnumerator SpawnParticles()
    {
        while(true)
        {
            // Spawn a new particle, give it to the lists and registers its position
            GameObject _go = Instantiate(particle, transform.position, Quaternion.identity);
            _go.name = _go.name + " " + _go.transform.position;
            if ( _go.TryGetComponent(out particleBuffer))
            {
                particleBuffer.IsMoving = true;
                Data.RegisterNewParticle(particleBuffer);
            }

            particleBuffer = null;

            // Delay
            for (int i = 0; i < delay; i++)
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }


    public void GetNextParticleMove()
    {
        for (int i = 0; i < particleBuffer.Velocity; i++)
        {
            Data.GetNextParticlePosition(particleBuffer);
        }

        particleBuffer = null;
    }

}