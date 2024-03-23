using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Card {
	public class CardView : MonoBehaviour
	{
		private CardInstance _cardInstance;

		private Image _image;

        public void Init(CardInstance cardInstance, Image image)
		{
			_cardInstance = cardInstance;
			_image = image;
        }

        public int getCardPosition
        {
            get => _cardInstance.CardPosition;
            set => _cardInstance.CardPosition = value;
        }

		public int TypeOfLayout
		{
			get => _cardInstance.typeOfLayout;
			set => _cardInstance.typeOfLayout = value;
		}

        public void Rotate(bool up)
		{
			if (up)
			{
				_image.sprite = _cardInstance.GetCardAsset.frontSide;
			}
			else
			{
				_image.sprite = _cardInstance.GetCardAsset.backSide;
			}
		}

		public void PlayCard()
		{
			if (_cardInstance.typeOfLayout == 2)
			{
                _cardInstance.typeOfLayout = 1;
                CardGame.Instance.MoveToCenter(_cardInstance);
			}
            else if (_cardInstance.typeOfLayout == 1)
            {
                _cardInstance.typeOfLayout = 3;
                CardGame.Instance.MoveToTrash(_cardInstance);
            }
        }
	}
}

