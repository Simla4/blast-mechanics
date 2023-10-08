using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconManager : MonoSingleton<IconManager>
{
    #region Variables

    [Header("Condition Values")]
    [SerializeField] private int a = 4;
    [SerializeField] private int b = 7;
    [SerializeField] private int c = 9;

    private TileManager tileManager;
    private BoardCreator boardCreator;
    private int boardSize;

    #endregion

    #region Callbacks

    private void Start()
    {
        tileManager = TileManager.Instance;
        Initialize();
    }

    private void OnEnable()
    {
        EventManager.OnMaterialChanged += ChangeMaterails;
    }

    private void OnDisable()
    {
        EventManager.OnMaterialChanged -= ChangeMaterails;
    }

    #endregion

    #region OtherMethods

    private void Initialize()
    {
        boardCreator = BoardCreator.Instance;
        boardSize = boardCreator.BoardSize;
    }
    
    private void ChangeMaterails()
    {
        var spawnedObjects = boardCreator.spawnedObjects;

        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                List<BlockBase> listTemp = tileManager.FindBlocks(spawnedObjects[i,j]);
            
                int size = listTemp.Count;
                
                if (size < a )
                {
                    var newSprite = listTemp[0].blockData.defaultSprite;
                    ChangeIcons(listTemp, newSprite);
                }
                
                else if (size >= a && size < b)
                {
                    var newSprite = listTemp[0].blockData.firstSprite;
                    ChangeIcons(listTemp, newSprite);

                }
                else if (size >= b && size < c)
                {
                    var newSprite = listTemp[0].blockData.secondSprite;
                    ChangeIcons(listTemp, newSprite);

                }
                else if (size > c)
                {
                    var newSprite = listTemp[0].blockData.thirthSprite;
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

    #endregion
}
