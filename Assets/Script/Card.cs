using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card //카드의 파라미터가 어떤 종류로 구성되었는지 명시하는 스크립트.
{
    public enum Tier { common, rare, epic, legend, immortal };
    public enum Type { player, resource, weapon, upgrade, building, drone, spawner, timer }
    public Tier cardTier;
    public Type cardType;
    public string cardName;
    public string cardInfo;
    public string cardID;
    public float cardRef_a;
    public float cardRef_b;
    public float cardRef_c;
}

[System.Serializable]
public class CardEvent //뽑으려 하는 카드의 ID 값과, 해당 ID를 가진 카드의 파라미터 정보를 명시함
{
    public int cardLeftID;
    public int cardCenterID;
    public int cardRightID;
    public Card[] cards;
}