using System;
using System.Collections.Generic;
using System.Linq;
using Card;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class CardGame
{
    private static CardGame game;
    private static int cardNameCounter = 1;

    public List<CardLayout> layOutList = new();

    private List<CardAsset> listOfStartedCards;

    private readonly Dictionary<CardInstance, CardView> _cardDictionary = new();

    private CardLayout beated;

    public CardLayout center;

    public int HandCapacity;

    public static CardGame Instance
    {
        get
        {
            if (game == null)
            {
                game = new CardGame();
            }
            return game;
        }
    }

    public void Init(List<CardLayout> cardLayouts, List<CardAsset> cardAssets, int handCapacity, CardLayout center, CardLayout beated)
    {
        layOutList = cardLayouts;

        listOfStartedCards = cardAssets;

        this.HandCapacity = handCapacity;

        this.center = center;
        this.beated = beated;

        StartGame();
    }

    private void CreateCardView(CardInstance instance)
    {
        GameObject newCardInstance = new GameObject($"{instance.GetCardAsset.name} {cardNameCounter}");

        ++cardNameCounter;

        CardView cardView = newCardInstance.AddComponent<CardView>();
        Image image = newCardInstance.AddComponent<Image>();

        cardView.Init(instance, image);

        Button button = newCardInstance.AddComponent<Button>();

        button.onClick.AddListener(cardView.PlayCard);

        newCardInstance.transform.SetParent(layOutList[instance.LayoutId].transform);

        _cardDictionary[instance] = cardView;
    }

    private void RecalculateLayout(int layoutId)
    {
        var cards = GetCardsInLayout(layoutId);

        for (int i = 0; i < cards.Count; ++i)
        {
            cards[i].getCardPosition = i;
        }
    }

    private void MoveToLayout(CardInstance card, int layout)
    {
        int currentCardLayout = card.LayoutId;
        card.LayoutId = layout;

        _cardDictionary[card].transform.SetParent(layOutList[layout].transform);

        RecalculateLayout(currentCardLayout);
        RecalculateLayout(layout);
    }

    public void MoveToCenter(CardInstance card)
    {
        int currentCardLayout = card.LayoutId;

        card.LayoutId = center.LayoutId;

        _cardDictionary[card].transform.SetParent(center.transform);

        RecalculateLayout(currentCardLayout);
        RecalculateLayout(center.LayoutId);
    }

    public void MoveToTrash(CardInstance card)
    {
        int currentCardLayout = card.LayoutId;
        card.LayoutId = beated.LayoutId;
        _cardDictionary[card].transform.SetParent(beated.transform);

        try
        {
            Button button = _cardDictionary[card].GetComponent<Button>();
            button.enabled = false;
            button.onClick.RemoveAllListeners();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

        RecalculateLayout(currentCardLayout);
        RecalculateLayout(beated.LayoutId);
    }

    private void CreateCard(CardAsset asset, int layout)
    {
        var instance = new CardInstance(asset)
        {
            LayoutId = layout,
            CardPosition = layOutList[layout].cardsCount++
        };
        CreateCardView(instance);
        MoveToLayout(instance, layout);
    }

    private void StartGame()
    {
        foreach (var layout in layOutList)
        {
            foreach (var startCard in listOfStartedCards)
            {
                CreateCard(startCard, layout.LayoutId);
            }
        }
    }

    public List<CardView> GetCardsInLayout(int id)
    {
        return _cardDictionary.Where(x => x.Key.LayoutId == id).Select(x => x.Value).ToList();
    }

    private List<CardInstance> GetLayoutInstances(int id)
    {
        return _cardDictionary.Where(x => x.Key.LayoutId == id).Select(x => x.Key).ToList();
    }

    private void ShuffleLayout(int id)
    {
        var cards = GetLayoutInstances(id);

        List<(int, int)> pairs = new();
        for (int i = 0; i < cards.Count; ++i)
        {
            for (int j = i + 1; j < cards.Count; ++j)
            {
                pairs.Add((i, j));
            }
        }

        Random rnd = new();
        pairs = pairs.OrderBy(_ => rnd.Next()).ToList();

        for (var i = 1; i < cards.Count; ++i)
        {
            var pair_item = pairs[i].Item1;
            var item = cards[pair_item];
            _cardDictionary[item].transform.SetSiblingIndex(pairs[i].Item2);
        }
    }

    public void StartTurn()
    {
        foreach (var layout in layOutList)
        {
            ShuffleLayout(layout.LayoutId);

            layout.FaceUp = true;

            var cards = GetCardsInLayout(layout.LayoutId);

            for (int i = 0; i < HandCapacity; ++i)
            {
                cards[i].TypeOfLayout = 2;
            }
        }
    }
}