using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardCreator : MonoBehaviour
{
    #region Variables

    [SerializeField] [Range(2, 10)] private int columnVal;
    [SerializeField] [Range(2, 10)]private int rowVal;

    [SerializeField] private List<BlockData> blockDataList;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private List<BlockBase> spawnedObjects;
    
    private Pool<BlockBase> blockPool;

    #endregion

    #region Callbacks

    private void OnEnable()
    {
        EventManager.OnBlockDestroyed += CoroutuneStarter;
    }

    private void OnDisable()
    {
        EventManager.OnBlockDestroyed -= CoroutuneStarter;
    }

    private void Start()
    {
        blockPool = PoolManager.Instance.blockPool;
        
        StartCoroutine(IECreateBoard());
    }

    #endregion

    #region MyRegion

    private IEnumerator IECreateBoard()
    {
        for (int i = 0; i < columnVal; i++)
        {
            for (int j = 0; j < rowVal; j++)
            {
                
                var newBlock = blockPool.Spawn();
                newBlock.transform.position = spawnPoints[j].position;
                newBlock.transform.SetParent(spawnPoints[j]);
                newBlock.name = "block(" + j.ToString() + "," + i.ToString() + ")";
                
                FillTheBlock(newBlock);
            }

            yield return  new  WaitForSeconds(0.25f);
        }
    }

    private void FillTheBlock(BlockBase newBlock)
    {
        var index = Random.Range(0, blockDataList.Count);
        
        newBlock.spriteRenderer.sprite = blockDataList[index].defaultSprite;
        spawnedObjects.Add(newBlock);
    }

    private void CoroutuneStarter()
    {
        StartCoroutine(IECreateNewBlock());
    }
    private IEnumerator IECreateNewBlock()
    {
        yield return new WaitForSeconds(0.2f);
        
        var newBlock = blockPool.Spawn();
        FillTheBlock(newBlock);
        
    }

    #endregion
}
