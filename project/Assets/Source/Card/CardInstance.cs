using UnityEngine;
using System.Collections;

namespace Card
{
	public class CardInstance
	{
		private CardAsset _cardAsset;

        public CardAsset GetCardAsset
        {
            get
            {
                return _cardAsset;
            }
        }

        public int LayoutId;

        public int CardPosition;

        public int typeOfLayout;

        public CardInstance(CardAsset cardAsset)
        {
            _cardAsset = cardAsset;
        }

    }
}

