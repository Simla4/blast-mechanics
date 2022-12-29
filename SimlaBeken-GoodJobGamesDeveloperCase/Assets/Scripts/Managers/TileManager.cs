using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoSingleton<TileManager>
{
    #region Variables

    private int boardSize;
    private BoardCreator boardCreator;

    #endregion

    #region Callbacks

    private void Start()
    {
        Initialize();
    }

    #endregion

    #region Other Methods

    private void Initialize()
    {
        boardCreator = BoardCreator.Instance;
        boardSize = boardCreator.BoardSize;
    }
    
    public List<GameObject> SearchNeighbors(BlockBase block)
    {
        List<GameObject> neighbors = new List<GameObject>();
        int xIndex = block.coordinates.x;
        int yIndex = block.coordinates.y;

        var spawnedObjects = boardCreator.spawnedObjects;

        if (xIndex - 1 >= 0 && xIndex - 1 < boardSize)
        {
            if (spawnedObjects[xIndex - 1, yIndex] != null && spawnedObjects[xIndex-1,yIndex].blockData.id == block.blockData.id)
            {
                neighbors.Add(spawnedObjects[xIndex - 1,yIndex].gameObject);
            }

        }
        if (xIndex + 1 >= 0 && xIndex + 1 < boardSize)
        {
            if (spawnedObjects[xIndex + 1, yIndex ] != null && spawnedObjects[xIndex + 1, yIndex].blockData.id == block.blockData.id)
            {
                neighbors.Add(spawnedObjects[xIndex + 1, yIndex].gameObject);
            }
        }
        if (yIndex + 1 >= 0 && yIndex + 1 < boardSize)
        {
            if (spawnedObjects[xIndex, yIndex + 1] != null && spawnedObjects[xIndex, yIndex+1].blockData.id == block.blockData.id)
            {
                neighbors.Add(spawnedObjects[xIndex , yIndex+1].gameObject);
            }

        }
        if (yIndex - 1 >= 0 && yIndex - 1 < boardSize)
        {
            if (spawnedObjects[xIndex, yIndex - 1] != null && spawnedObjects[xIndex , yIndex-1].blockData.id == block.blockData.id)
            {
                neighbors.Add(spawnedObjects[xIndex , yIndex-1].gameObject);
            }
        }
        return neighbors;
    }

    public List<BlockBase> FindBlocks(BlockBase block)
    {
        List<GameObject> BlockList = new List<GameObject>();
        Stack<BlockBase> blockStack = new Stack<BlockBase>();
        
        if (block.gameObject != null)
        {
            BlockList.Add(block.gameObject);
            blockStack.Push(block);

            while (blockStack.Count > 0)
            {
                List<GameObject> currentNeighbors = SearchNeighbors(blockStack.Peek());
                blockStack.Pop();
                foreach (GameObject g in currentNeighbors)
                {
                    if (!BlockList.Contains(g))
                    {
                        BlockList.Add(g);
                        blockStack.Push(g.GetComponent<BlockBase>());
                    }
                }
            }
        }

        List<BlockBase> blockBaseList = new List<BlockBase>();
      
        foreach (GameObject g in BlockList)
        {
            blockBaseList.Add(g.GetComponent<BlockBase>());
        }
        
        return blockBaseList;
    }

    #endregion
}
