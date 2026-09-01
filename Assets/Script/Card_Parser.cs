using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Parser : MonoBehaviour
{
    public Card[] Parse(string _CSVFilename)
    {
        List<Card> cardList = new List<Card>(); //전체 카드 리스트 생성
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFilename); //csv 파일 가져오기

        string[] data = csvData.text.Split(new char[] { '\n' });
        for(int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });

            Card card = new Card(); //카드 리스트 생성
            card.cardID = row[1];
            card.cardName = row[2];
            card.cardTier = (Card.Tier)System.Enum.Parse(typeof(Card.Tier), row[3]);
            card.cardType = (Card.Type)System.Enum.Parse(typeof(Card.Type), row[4]);
            card.cardInfo = row[5].Replace("&", ",");
            card.cardRef_a = float.Parse(row[6]);
            card.cardRef_b = float.Parse(row[7]);
            card.cardRef_c = float.Parse(row[8]);

            cardList.Add(card);
        }

        return cardList.ToArray();
    }
}
