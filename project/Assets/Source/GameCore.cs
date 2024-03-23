using UnityEngine;
using System.Collections.Generic;
using Card;

public class GameCore : MonoBehaviour
{
    [SerializeField] private List<CardLayout> cardLayouts;
    [SerializeField] private List<CardAsset> cardAssets;
    [SerializeField] private int handCapacity;
    [SerializeField] private CardLayout center;
    [SerializeField] private CardLayout bucket;


    private void Start()
    {
        int id = 0;
        foreach (var layout in cardLayouts)
        {
            layout.LayoutId = id;
            ++id;
        }
        center.LayoutId = id;
        bucket.LayoutId = id + 1;

        CardGame.Instance.Init(cardLayouts, cardAssets, handCapacity, center, bucket);
    }

    public void StartTurn()
    {
        CardGame.Instance.StartTurn();
    }
}

