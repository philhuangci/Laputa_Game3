using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]


public class CardsMessageGroup : MonoBehaviour
{
    public Card[] _cardList;
    private List<int> cardIdxList;
    // this is the cardIdx and Card Diction
    private Dictionary<int, Card> _cardDic;
    private Dictionary<int, List<int>> _colorToCardDic;

    private List<int> openedCardList;

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("cards number: " + _cardList.Length);

        _cardDic = new Dictionary<int, Card>();
        _cardDic.Clear();
        cardIdxList = new List<int>();
        foreach(Card cad in _cardList)
        {
            _cardDic.Add(cad.id, cad);
            cardIdxList.Add(cad.id);
        }
        _colorToCardDic = new Dictionary<int, List<int>>();
        _colorToCardDic.Clear();

        openedCardList = new List<int>();
        openedCardList.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayCardfForRound(int round)
    {
        openedCardList.Clear();
        // for round = 1    two types of cards, each time review 8 cards
        // for round = 2    four types of cards, each time review 8 cards
        // for round = 3    eight types of cards, each time review 8 cars;
        int cardNumberForOneType = 16 /(int)(Mathf.Pow(2, round));
        int displayNumber = cardNumberForOneType / 2;

        foreach (List<int> l_cardList in _colorToCardDic.Values)
        {
            if (l_cardList.Count != cardNumberForOneType)
            {
                Debug.LogError("incorrect cars number for colorcard dic" + cardNumberForOneType + " " + displayNumber + " " + l_cardList.Count + " "
                    + _colorToCardDic.Keys.Count);
                return;
            }
            openedCardList.AddRange(SortRandom(l_cardList).GetRange(0, displayNumber));
        }
        if (openedCardList.Count != 8)
        {
            Debug.LogError("incorrect display card number!");
            return; 
        }
        foreach(int l_cardIdx in openedCardList)
        {
            _cardDic[l_cardIdx].FlipCard();
        }
    }

    public void UnflipOpenedCards()
    {
        foreach (int l_cardIdx in openedCardList)
        {
            _cardDic[l_cardIdx].FlipCard();
        }
    }


    public void ShuffleCard()
    {
        // change the card texture
        InsideOutAlgorithm();
        // reset the color to cardIdx dic
        _colorToCardDic.Clear();

        foreach(int cardIdx in cardIdxList)
        {
            int textureId = _cardDic[cardIdx].colorAndTextureId;

            if (_colorToCardDic.ContainsKey(textureId))
                _colorToCardDic[textureId].Add(cardIdx);
            else
            {
                List<int> l_cardList = new List<int>();
                l_cardList.Add(cardIdx);
                _colorToCardDic.Add(textureId, l_cardList);
            }
        }

    }

    private List<int> SortRandom(List<int> array)
    {
        List<int> tmpList = new List<int>(array);
        int rand;
        int tmpValue;
        for (int i = tmpList.Count - 1; i >= 0; i--)
        {
            rand = Random.Range(0, i + 1);
            tmpValue = tmpList[rand];
            tmpList[rand] = tmpList[i];
            tmpList[i] = tmpValue;
        }

        return tmpList;
    }



    public void InsideOutAlgorithm()
    {
        List<int> tmpList = new List<int>(cardIdxList);

        int rand;
        int tmpValue;
        for (int i = tmpList.Count - 1; i >= 0; i--)
        {
            rand = Random.Range(0, i + 1);
            tmpValue = tmpList[rand];
            tmpList[rand] = tmpList[i];
            tmpList[i] = tmpValue;

        }
        cardIdxList = new List<int>(tmpList);

    }


    public void DisplayPartOfCards(int displayRound)
    {
        int displayStartIdx = 0;
        int displayEndIdx = 0;
        if (displayRound == 1)
        {
            displayStartIdx = 0;
            displayEndIdx = 5;
        }
        else if (displayRound == 2)
        {
            displayStartIdx = 5;
            displayEndIdx = 10;
        }
        else if (displayRound == 3)
        {
            displayStartIdx = 10;
            displayEndIdx =16;
        }

        for (int i = displayStartIdx; i != displayEndIdx; i++)
        {
            _cardDic[cardIdxList[i]].FlipCard();
        }

    }


   
    public void SetUpCardMaterials(int round)
    {
        int colorTypes = (int)Mathf.Pow(2.0f, (float)round + 1);
        
        for (int i = 0; i < colorTypes; i++)
        {
            for (int j = 16 / colorTypes * i; j < 16 / colorTypes * (i+1); j++)
            {
                //_cardDic[cardIdxList[j]].SetColor(i);
                _cardDic[cardIdxList[j]].SetTexture(i);
            }
        }
    }


}
