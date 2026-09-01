using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Card_DatabaseManager : MonoBehaviour
{
    public static Card_DatabaseManager instance;
    [SerializeField] string csv_FileName;
    Dictionary<int, Card> cardDic = new Dictionary<int, Card> ();
    public List<int> overlapList = new List<int> ();
    public static bool isFinish = false;

    void Awake()
    {
        if(instance == null) //최초 1회, 데이터가 존재하지 않을 때만 실행
        {
            instance = this;
            Card_Parser parser = GetComponent<Card_Parser>(); //파싱한 데이터베이스를 가져오기
            Card[] cards = parser.Parse(csv_FileName); //얘가 모든 카드의 데이터를 가지고 있음
            for(int i = 0; i < cards.Length; i++)
            {
                cardDic.Add(i, cards[i]); //가져온 값들을 딕셔너리(원하는 것만 골라 뽑게 가공) 로 추가
                overlapList.Add(i);
            }
            isFinish = true;
        }
    }

    //카드를 선택 리스트에 추가하는 메소드. 매개변수는 랜덤으로 산출된 ID값
    public Card[] GetCards(int _cardLeftID, int _cardCenterID, int _cardRightID)
    {
        List<Card> cardList = new List<Card>();
        cardList.Add(cardDic[_cardLeftID]);
        cardList.Add(cardDic[_cardCenterID]);
        cardList.Add(cardDic[_cardRightID]);

        return cardList.ToArray();
    }

    public void RemoveCard(int _cardID)
    {
        overlapList.Remove(_cardID);
    }

}
