using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GB_Manager : MonoBehaviour
{
    private Dictionary<Vector2, GB_Particle> allTiles = new Dictionary<Vector2, GB_Particle>();
    private List<GB_Particle> movingParticles = new List<GB_Particle>();

    [SerializeField] int xBounds = 20;
    [SerializeField] int yBounds = 20;

    [SerializeField] GameObject sandParticle = null;
    [SerializeField, Range(1,50)] int blobSize = 20;

    #region Unity

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawCube(new Vector2(-xBounds, 0), new Vector2(1, yBounds * 2));
    //    Gizmos.DrawCube(new Vector2(xBounds, 0), new Vector2(1, yBounds * 2));
    //    Gizmos.DrawCube(new Vector2(0, -yBounds), new Vector2(xBounds * 2, 1));
    //    Gizmos.DrawCube(new Vector2(0, yBounds), new Vector2(xBounds * 2, 1));
    //}

    private void OnGUI()
    {
        GUIStyle _style = new GUIStyle();
        _style.fontSize = 50;

        GUI.Label(new Rect(Vector2.zero, new Vector2(500, 100)), (1 / Time.deltaTime).ToString("000"), _style);
    }

    private void Start()
    {
        InitDictionnary();

        GameObject _go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _go.transform.position = new Vector2(-xBounds, 0);
        _go.transform.localScale = new Vector2(1, yBounds * 2);

        _go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _go.transform.position = new Vector2(xBounds, 0);
        _go.transform.localScale = new Vector2(1, yBounds * 2);


        _go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _go.transform.position = new Vector2(0, -yBounds);
        _go.transform.localScale = new Vector2(xBounds * 2, 1);


        _go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _go.transform.position = new Vector2(0, yBounds);
        _go.transform.localScale = new Vector2(xBounds * 2, 1);

    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            SpawnParticlesBlob(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        UpdateMovingParticles();

        UpdateTiles();
    }

    #endregion

    void InitDictionnary()
    {
        allTiles.Clear();

        for (int x = -xBounds; x < xBounds; x++)
        {
            for (int y = -yBounds; y < yBounds; y++)
            {
                allTiles.Add(new Vector2(x, y), null);
            }
        }
    }

    void SpawnParticlesBlob(Vector2 _pos)
    {
        _pos.x = (int)_pos.x;
        _pos.y = (int)_pos.y;

        for (int i = -(blobSize / 2); i < blobSize / 2; i++)
        {
            for (int ii = -(blobSize / 2); ii < blobSize / 2; ii++)
            {
                if (allTiles.ContainsKey(new Vector2(i, ii) + _pos) && allTiles[new Vector2(i, ii) + _pos] || !InBound(new Vector2(i, ii) + _pos))
                    continue;

                GameObject _go = Instantiate(sandParticle, _pos + new Vector2(i, ii), Quaternion.identity);
                _go.name = _go.name + " " + _go.transform.position;
                GB_Particle _p = _go.GetComponent<GB_Particle>();
                movingParticles.Add(_p);
                allTiles[_pos + new Vector2(i, ii)] = _p;
            }
        }
    }

    GB_Particle particleBuffer = null;
    Vector2 particlePositionBuffer = Vector2.zero;
    GB_Particle tileOccupied = null;

    void UpdateMovingParticles()
    {
        List<GB_Particle> _toRemove = new List<GB_Particle>();

        particleBuffer = null;
        particlePositionBuffer = Vector2.zero;
        tileOccupied = null;

        for (int i = 0; i < movingParticles.Count; i++)
        {
            particleBuffer = movingParticles[i];

            if (!particleBuffer)
                continue;

            for (int ii = 0; ii < particleBuffer.Velocity; ii++)
            {

                particlePositionBuffer = particleBuffer.Position;

                allTiles.TryGetValue(particlePositionBuffer + Vector2.down, out tileOccupied);

                // If below clear && in boundaries
                if (!tileOccupied && InBound((Vector2)particleBuffer.Position + Vector2.down))
                {
                    // Sets the particle position and register it
                    particleBuffer.Position = particlePositionBuffer + Vector2.down;

                    allTiles[particleBuffer.Position] = particleBuffer;
                    allTiles[particlePositionBuffer] = null;

                    //particleBuffer = null;
                    continue;
                }

                else
                {
                    // Random to sometimes start left, sometimes start right
                    int _rnd = Random.Range(0, 2);
                    if (_rnd == 0)
                    {
                        allTiles.TryGetValue(particlePositionBuffer + Vector2.down + Vector2.left, out tileOccupied);

                        // If down left clear && in boundaries
                        if (!tileOccupied && InBound(particlePositionBuffer + Vector2.down + Vector2.left))
                        {
                            // Sets the particle position and register it
                            particleBuffer.transform.position = particlePositionBuffer + Vector2.down + Vector2.left;

                            allTiles[particleBuffer.Position] = particleBuffer;
                            allTiles[particlePositionBuffer] = null;

                            //particleBuffer = null;
                            continue;
                        }

                        allTiles.TryGetValue(particlePositionBuffer + Vector2.down + Vector2.right, out tileOccupied);

                        // Else if right clear && in boundaries
                        if (!tileOccupied && InBound(particlePositionBuffer + Vector2.down + Vector2.right))
                        {
                            // Sets the particle position and register it
                            particleBuffer.transform.position = particlePositionBuffer + Vector2.down + Vector2.right;

                            allTiles[particleBuffer.Position] = particleBuffer;
                            allTiles[particlePositionBuffer] = null;

                            //particleBuffer = null;
                            continue;
                        }
                    }

                    else
                    {

                        allTiles.TryGetValue(particlePositionBuffer + Vector2.down + Vector2.right, out tileOccupied);

                        // If down left clear && in boundaries
                        if (!tileOccupied && InBound(particlePositionBuffer + Vector2.down + Vector2.right))
                        {
                            // Sets the particle position and register it
                            particleBuffer.transform.position = particlePositionBuffer + Vector2.down + Vector2.right;

                            allTiles[particleBuffer.Position] = particleBuffer;
                            allTiles[particlePositionBuffer] = null;

                            //particleBuffer = null;
                            continue;
                        }

                        allTiles.TryGetValue(particlePositionBuffer + Vector2.down + Vector2.left, out tileOccupied);

                        // Else if right clear && in boundaries
                        if (!tileOccupied && InBound(particlePositionBuffer + Vector2.down + Vector2.left))
                        {
                            // Sets the particle position and register it
                            particleBuffer.transform.position = particlePositionBuffer + Vector2.down + Vector2.left;

                            allTiles[particleBuffer.Position] = particleBuffer;
                            allTiles[particlePositionBuffer] = null;

                            //particleBuffer = null;
                            continue;
                        }
                    }
                }
                _toRemove.Add(particleBuffer);
                continue;
            }

            particleBuffer = null;

        }

        for (int i = 0; i < _toRemove.Count; i++)
        {
            movingParticles.Remove(_toRemove[i]);
        }
    }

    void UpdateTiles()
    {
        foreach (KeyValuePair<Vector2, GB_Particle> _tile in allTiles)
        {
            if(_tile.Value)
            {
                allTiles.TryGetValue(_tile.Key + Vector2.down, out tileOccupied);
                if (!tileOccupied)
                    movingParticles.Add(_tile.Value);
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
        if (_pos.x > -xBounds && _pos.x < xBounds && _pos.y > -yBounds && _pos.x < yBounds)
            return true;

        else
            return false;
    }
}
