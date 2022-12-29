using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardCreator : MonoSingleton<BoardCreator>
{
    #region Variables

    [Header("Map Values")]
    [SerializeField] [Range(2, 10)] private int boardSize = 2;
    [SerializeField] private List<Transform> spawnPoints;
    public List<BlockData> blockDataList;

    private TileManager tileManager;
    
    public BlockBase[,] spawnedObjects;
    
    private Pool<BlockBase> blockPool;

    public int BoardSize => boardSize;

    #endregion

    #region Callbacks

    private void Start()
    {
        blockPool = PoolManager.Instance.blockPool;
        tileManager = TileManager.Instance;

        spawnedObjects = new BlockBase[boardSize, boardSize];
        
        StartCoroutine(IECreateBoard());
    }

    private void OnEnable()
    {
        EventManager.OnBoardShuffle += ShuffleTheBoard;
    }

    private void OnDisable()
    {
        EventManager.OnBoardShuffle -= ShuffleTheBoard;
    }

    #endregion

    #region MyRegion

    private IEnumerator IECreateBoard()
    {
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                CreateBlock(i, j);
            }
            yield return  new  WaitForSeconds(0.25f);
        }
        
        EventManager.OnMaterialChanged?.Invoke();
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
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
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
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                if (spawnedObjects[j, i] == null)
                {
                    var obj = CreateBlock(i, j);
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
        
        EventManager.OnMaterialChanged?.Invoke();
    }
    
    public void DestroyBlocks(BlockBase blockBase)
    {
        List<BlockBase> neighbors = tileManager.FindBlocks(blockBase); 

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
    private void ShuffleTheBoard(int singleBlockCount)
    {
        if (singleBlockCount >= boardSize * boardSize)
        {
            Debug.Log("Shuffle the board worked");
            
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    var newIndex1 = Random.Range(0, boardSize);
                    var newIndex2 = Random.Range(0, boardSize);
                    
                    spawnedObjects[i, j].UpdateInformations(newIndex1, newIndex2);
                    spawnedObjects[newIndex1, newIndex2].UpdateInformations(i, j);
                    
                    spawnedObjects[i, j].transform.SetParent(spawnPoints[newIndex2]);
                    spawnedObjects[newIndex1, newIndex2].transform.SetParent(spawnPoints[j]);
                    
                    (spawnedObjects[i, j].transform.position, spawnedObjects[newIndex1, newIndex2].transform.position) =
                        (spawnedObjects[newIndex1, newIndex2].transform.position, spawnedObjects[i, j].transform.position);
                    
                    (spawnedObjects[i, j], spawnedObjects[newIndex1, newIndex2]) =
                             (spawnedObjects[newIndex1, newIndex2], spawnedObjects[i, j]);
                }
            }
        }
        
        EventManager.OnMaterialChanged?.Invoke();
    }
    #endregion
}
