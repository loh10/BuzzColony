using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GeneratorMap : MonoBehaviour
{
    #region MapDeatail
    [Header("Mesh Options")] 
    public bool generate;
    public int mapScale = 25;    
    #endregion
    #region MapNoiseOption
    [Header("Map Noise Options")]
    [Range(0, 1)]public float mapPerlinScale =0.5f;
    [Range(0, 1)]public float mapPersistence = .5f;
    [Range(0.001f, 1)] float mapLacunarity = 0.05f;
    public int mapOctaveCount = 1;
    [Header("Mountain Noise Options")]
    [Range(0, 1)]public float mountainPerlinScale =0.5f;
    [Range(0, 1)]public float mountainPersistence = .5f;
    [Range(0.001f, 1)] float mountainLacunarity = 0.05f;
    public float mountainRatio = .75f;
    public int mountainOctaveCount = 1;
    public int seed;
    #endregion
    #region NoiseOption
    [Header("Tile Options")]
    private float _colorForce;
    public List<Tile> tileMap;
    public Tile tileMap2;
    public Tilemap tileMapLayer1,tileMapLayer2;
    #endregion

    void Start()
    {
        ClearTileMaps();
        if (SaveAndLoad.Instance != null)
        {
            SaveAndLoad.Instance.LoadGame();
            seed = SaveAndLoad.Instance._seed;
        }
        GenerateMap();
    }
    
    void ClearTileMaps()
    {
        tileMapLayer1.ClearAllTiles();
        tileMapLayer2.ClearAllTiles();
    }
    
/// <summary>
/// genere la map sur mapSize par mapSize
/// </summary>
    void GenerateMap()
    {
        for (int i = 0; i < mapScale; i++)
        {
            for (int j = 0; j < mapScale; j++)
            {
                SetColor( i,  j);
                SetMountain(i, j);
            }
        }
    }

    /// <summary>
    /// Set la Tile sur la map selon la couleur dans perlin
    /// </summary>
    void SetColor(int x, int z)
    {
        float colorFloat = CalculatePerlin(x, z,mapPerlinScale,mapOctaveCount,mapLacunarity,mapPersistence);
        int indexColor;
        switch (colorFloat)
        {
            case <0.7f:
                indexColor = 0;
                break;
            case <0.8f:
                indexColor = 1;
                break;
            case <0.9f:
                indexColor = 2;
                break;
            default :
                indexColor = 3;
                break;
        }
        tileMapLayer1.SetTile(new Vector3Int(x,z,0),tileMap[indexColor]);
    }

    void SetMountain(int x, int z)
    {
        float colorMountain = CalculatePerlin(x, z, mountainPerlinScale, mountainOctaveCount, mountainLacunarity, mountainPersistence);
        if (colorMountain > mountainRatio)
        {
            tileMapLayer2.SetTile(new Vector3Int(x,z,0),tileMap2);
        }
    }
    
    /// <summary>
    /// Retourne un float via le perlin Noise
    /// </summary>
    float CalculatePerlin(int x, int z,float scale,int octave,float lacunarity,float persistency)
    {
        float number = 0;
        float amplitude = 1;
        float frequence =scale ;

        for (int i = 0; i < octave; i++)
        {
            number += Mathf.PerlinNoise(x * frequence + seed, z * frequence + seed) * amplitude;
            frequence /= lacunarity;
            amplitude *= persistency;
        }

        return number;
    }

}
