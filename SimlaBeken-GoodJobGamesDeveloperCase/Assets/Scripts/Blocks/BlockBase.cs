using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBase : MonoBehaviour, ISpawned, IDespawned
{
    #region Variables

    public SpriteRenderer spriteRenderer;

    public Vector2Int coordinates;
    public BlockData blockData;

    private BoardCreator boarCreator;
    private Vector3 firstPos;

    #endregion

    #region Callbacks

    private void Start()
    {
        firstPos = transform.position;
        boarCreator = BoardCreator.Instance;
    }

    #endregion

    #region Other Methods
    
    public void Initialize(Vector2Int coordinates,BlockData blockData)
    {
        this.blockData = blockData;
        this.coordinates = coordinates;
    }

    public void OnSpawned()
    {
        gameObject.SetActive(true);
    }

    public void MoveDownBlock(int moveDownAmount)
    {
        var newYPos = coordinates.y - moveDownAmount;
        
        UpdateArrayPos(newYPos);
    }

    private void UpdateArrayPos(int newYPos)
    {
        boarCreator.spawnedObjects[coordinates.x, newYPos] = boarCreator.spawnedObjects[coordinates.x, coordinates.y];
        boarCreator.spawnedObjects[coordinates.x, coordinates.y] = null;
        coordinates.y = newYPos;
        gameObject.name = "block" + "( " + coordinates.x + ", " + coordinates.y + " )";
    }

    public void UpdateInformations(int row, int col)
    {
        coordinates.x = row;
        coordinates.y = col;
        gameObject.name = "block" + "( " + coordinates.x + ", " + coordinates.y + " )";
    }

    public void OnDespawned()
    {
        boarCreator.spawnedObjects[coordinates.x, coordinates.y] = null;
    }

    #endregion
}
