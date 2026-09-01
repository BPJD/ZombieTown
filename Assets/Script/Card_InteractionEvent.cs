using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class Card_InteractionEvent : MonoBehaviour
{
    public CardEvent card;
    Card_DatabaseManager databaseManager;

    void Start()
    {
        databaseManager = GetComponent<Card_DatabaseManager>();
    }

    public Card[] GetCard() //어떤 카드를 뽑을지 결정하고 해당 값을 데이터베이스 시스템에 전송하는 스크립트
    {
        card.cardLeftID = databaseManager.overlapList[Random.Range(0, databaseManager.overlapList.Count)];
        card.cardRightID = databaseManager.overlapList[Random.Range(0, databaseManager.overlapList.Count)];
        card.cardCenterID = databaseManager.overlapList[Random.Range(0, databaseManager.overlapList.Count)];
        card.cards = Card_DatabaseManager.instance.GetCards(card.cardLeftID, card.cardCenterID, card.cardRightID);

        return card.cards;
    }
}
