using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    #region MyRegion

    [SerializeField] private BlockBase blockBase;
    
    private Pool<BlockBase> blockPool;

    #endregion

    #region Callbacks

    private void Start()
    {
        blockPool = PoolManager.Instance.blockPool;
    }
    
    private void OnMouseDown()
    {
        EventManager.OnBlockDestroyed?.Invoke();
        blockPool.ReturnToPool(blockBase);
    }

    #endregion
    
}
