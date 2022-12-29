using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardCreator : MonoSingleton<BoardCreator>
{
    #region Variables

    [Header("Map Values")]
    [SerializeField] [Range(2, 10)] private int columnVal;
    [SerializeField] [Range(2, 10)]private int rowVal;
    [SerializeField] private List<BlockData> blockDataList;
    [SerializeField] private List<Transform> spawnPoints;

    [Header("Condition Values")]
    [SerializeField] private int a = 4;
    [SerializeField] private int b = 7;
    [SerializeField] private int c = 9;
    
    public BlockBase[,] spawnedObjects;
    
    private Pool<BlockBase> blockPool;

    #endregion

    #region Callbacks

    private void OnEnable()
    {
        //EventManager.OnBlockDestroyed += CoroutuneStarter;
    }

    private void OnDisable()
    {
        //EventManager.OnBlockDestroyed -= CoroutuneStarter;
    }

    private void Start()
    {
        blockPool = PoolManager.Instance.blockPool;

        spawnedObjects = new BlockBase[columnVal, rowVal];
        
        StartCoroutine(IECreateBoard());
    }

    #endregion

    #region MyRegion

    private IEnumerator IECreateBoard()
    {
        for (int i = 0; i < rowVal; i++)
        {
            for (int j = 0; j < columnVal; j++)
            {
                CreateBlock(i, j);
            }
            yield return  new  WaitForSeconds(0.25f);
        }
        
        ChangeMaterails();
    }

    private BlockBase CreateBlock(int i, int j)
    {
        var newBlock = blockPool.Spawn();
        newBlock.transform.position = spawnPoints[j].position;
        newBlock.transform.SetParent(spawnPoints[j]);
        newBlock.name = "block(" + j.ToString() + "," + i.ToString() + ")";
        spawnedObjects[j, i] = newBlock;
        var blockDataIndex = FillTheBlock(newBlock);
                
        newBlock.Initialize(new Vector2Int(j, i), blockDataList[blockDataIndex]);

        return newBlock;
    }

    private int FillTheBlock(BlockBase newBlock)
    {
        var index = Random.Range(0, blockDataList.Count);
        newBlock.spriteRenderer.sprite = blockDataList[index].defaultSprite;
        return index;
    }
    
    private void CheckRows()
    {
        int nullCount = 0;
        for (int i = 0; i < rowVal; i++)
        {
            for (int j = 0; j < columnVal; j++)
            {
                if (spawnedObjects[i,j] == null)
                    nullCount++;
                else if (nullCount > 0)
                    spawnedObjects[i,j].MoveDownBlock(nullCount);
            }
            nullCount = 0;
        }
    }
    
    private IEnumerator RefillBoard()
    {
        for (int i = 0; i < rowVal; i++)
        {
            for (int j = 0; j < columnVal; j++)
            {
                if (spawnedObjects[j, i] == null)
                {
                    var obj = CreateBlock(i, j);
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
        
        ChangeMaterails();
    }
    
    public void DestroyTiles(BlockBase blockBase)
    {
        List<BlockBase> neighbors = FindBlocks(blockBase);

        if (neighbors.Count > 1)
        {
            StartCoroutine(Check(blockBase, neighbors));

        }
    }
    
    private IEnumerator Check(BlockBase blockBase, List<BlockBase> neighbors)
    {
        foreach (BlockBase t in neighbors)
        {
            blockPool.ReturnToPool(t);
        }
        UpdateBlockList();
        yield return new WaitForSeconds(0.1f);
    }

    private void UpdateBlockList()
    {
        CheckRows();
        StartCoroutine(RefillBoard());
    }

    private void ChangeMaterails()
    {
        for (int i = 0; i < rowVal; i++)
        {
            for (int j = 0; j < columnVal; j++)
            {
            List<BlockBase> listTemp = FindBlocks(spawnedObjects[i,j]);
            
            int size = listTemp.Count;
            
                if (size >= a && size < b)
                {
                    var newSprite = listTemp[0].blockData.firstSprite;
                    ChangeIcons(listTemp, newSprite);

                }
                else if (size >= b && size < c)
                {
                    var newSprite = listTemp[0].blockData.firstSprite;
                    ChangeIcons(listTemp, newSprite);

                }
                else if (size > c)
                {
                    var newSprite = listTemp[0].blockData.firstSprite;
                    ChangeIcons(listTemp, newSprite);
                }
            }
        }
    }
    
    private void ChangeIcons(List<BlockBase> list, Sprite newSprite)
    {
        foreach (BlockBase t in list)
        {
            t.gameObject.SetActive(false);
            t.spriteRenderer.sprite = newSprite;
            t.transform.gameObject.SetActive(true);         
        }
    }

    private List<GameObject> SearchNeighbors(BlockBase tile)
    {
        List<GameObject> neighbors = new List<GameObject>();
        int xIndex = tile.coordinates.x;
        int yIndex = tile.coordinates.y;

        if (xIndex - 1 >= 0 && xIndex - 1 < columnVal)
        {
            if (spawnedObjects[xIndex - 1, yIndex] != null && spawnedObjects[xIndex-1,yIndex].blockData.id == tile.blockData.id)
            {
                neighbors.Add(spawnedObjects[xIndex - 1,yIndex].gameObject);
            }

        }
        if (xIndex + 1 >= 0 && xIndex + 1 < columnVal)
        {
            if (spawnedObjects[xIndex + 1, yIndex ] != null && spawnedObjects[xIndex + 1, yIndex].blockData.id == tile.blockData.id)
            {
                neighbors.Add(spawnedObjects[xIndex + 1, yIndex].gameObject);
            }
        }
        if (yIndex + 1 >= 0 && yIndex + 1 < rowVal)
        {
            if (spawnedObjects[xIndex, yIndex + 1] != null && spawnedObjects[xIndex, yIndex+1].blockData.id == tile.blockData.id)
            {
                neighbors.Add(spawnedObjects[xIndex , yIndex+1].gameObject);
            }

        }
        if (yIndex - 1 >= 0 && yIndex - 1 < rowVal)
        {
            if (spawnedObjects[xIndex, yIndex - 1] != null && spawnedObjects[xIndex , yIndex-1].blockData.id == tile.blockData.id)
            {
                neighbors.Add(spawnedObjects[xIndex , yIndex-1].gameObject);
            }
        }
        return neighbors;
    }

    private List<BlockBase> FindBlocks(BlockBase tile)
    {
        List<GameObject> tileBlockList = new List<GameObject>();
        Stack<BlockBase> tileStack = new Stack<BlockBase>();
        
        if (tile.gameObject != null)
        {
        tileBlockList.Add(tile.gameObject);
        tileStack.Push(tile);

        while (tileStack.Count > 0)
        {
            List<GameObject> currentNeighbors = SearchNeighbors(tileStack.Peek());
            tileStack.Pop();
            foreach (GameObject g in currentNeighbors)
            {
                if (!tileBlockList.Contains(g))
                {
                    tileBlockList.Add(g);
                    tileStack.Push(g.GetComponent<BlockBase>());
                    }
            }
        }
        }

        List<BlockBase> tileControllerList = new List<BlockBase>();
      
        foreach (GameObject g in tileBlockList)
        {
            tileControllerList.Add(g.GetComponent<BlockBase>());
        }
        
        return tileControllerList;
    }



    #endregion
}
