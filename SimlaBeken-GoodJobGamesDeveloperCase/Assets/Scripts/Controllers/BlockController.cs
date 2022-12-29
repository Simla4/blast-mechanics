using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    #region MyRegion

    [SerializeField] private BlockBase blockBase;

    #endregion

    #region Callbacks
    
    private void OnMouseDown()
    {
        BoardCreator.Instance.DestroyTiles(blockBase);
    }

    #endregion
    
}
