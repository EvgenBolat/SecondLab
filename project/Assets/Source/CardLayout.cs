using UnityEngine;
using System.Collections.Generic;
using Card;
using System;
using Unity.VisualScripting;

public class CardLayout : MonoBehaviour
{

    public bool FaceUp;

    internal int cardsCount = 0;

    public int LayoutId;
	[SerializeField] private Vector2 Offset;

    private void Update()
    {
       List<CardView> repository = CardGame.Instance.GetCardsInLayout(LayoutId);

        foreach (var card in repository)
        {
            Transform transform = card.GetComponent<Transform>();
            if (card.TypeOfLayout == 2)
            {
                FaceUp = true;
                transform.localPosition = FindPosition(card.getCardPosition, 2);
            }
            else if (card.TypeOfLayout == 0)
            {
                FaceUp = false;
                transform.localPosition = FindPosition(card.getCardPosition, 0);
            }
            else if (card.TypeOfLayout == 1)
            {
                FaceUp = true;
                transform.localPosition = new Vector2(0f,0f);
            }
            else if (card.TypeOfLayout == 3)
            {
                FaceUp = false;
                transform.localPosition = FindPosition(card.getCardPosition, 3);
            }
            card.Rotate(FaceUp);
        }
    }

    private Vector3 FindPosition(int getCardPosition, int v)
    {
        Vector2 position;
        if (v == 2)
        {
            position = new Vector2(getCardPosition * Offset.x, Offset.y);
        }
        else if (v == 0)
        {
           position = new Vector2(getCardPosition * Offset.x, 0);
        }
        else if (v == 3)
        {
            position = new Vector2(0, Offset.y * getCardPosition );
        }
        else
        {
            throw new ArgumentException("Неизвестное место для карты");
        }
        return position;
    }
}

