using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card_CallOut : MonoBehaviour //카드 기능을 호출하는 스크립트 및 게임 내 UI 작동 관리
{
    public GameObject cardUI;
    Card_InteractionEvent cardData;
    Card_Apply cardApply;
    public GameObject[] buttons;
    public Image[] buttonImage;
    public Image[] cardIcons;
    public Sprite[] icons;
    public Text[] cardNames;
    public Text[] cardTiers;
    public Text[] cardInfos;
    string[] tier_texts = { "일반", "고급", "희귀", "전설", "불멸" };
    public Color[] colors;
    Unit_Status playerStat;
    Player_Action playerAction;
    public GameObject buildingUI;
    GameManage manager;


    public GameObject dPad, upgradeButton;

    public GameObject[] activateUI;
    bool[] activateUI_activated = { false, false };


    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponentInParent<GameManage>();
        cardData = GetComponent<Card_InteractionEvent>();
        cardApply = GetComponent<Card_Apply>();
        playerStat = GameObject.FindGameObjectWithTag("Player").GetComponent<Unit_Status>();
        playerAction = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Action>();
    }

    public void CardDraw()
    {
        for (int i = 0; i < activateUI.Length; i++) //UI 관련
        {
            if (activateUI[i].activeSelf) //카드 뽑기가 호출됐을때 원래 켜져있었던 UI는 키고, 꺼져있던 UI는 해당 상태 유지
            {
                activateUI_activated[i] = true;
            }
            else
            {
                activateUI_activated[i] = false;
            }
        }

        if (!manager.isGameOver)
        {
            GameStop(true);
            cardData.GetCard();
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            cardIcons[i].sprite = icons[(int)cardData.card.cards[i].cardType];
            cardNames[i].text = cardData.card.cards[i].cardName;
            cardTiers[i].text = tier_texts[(int)cardData.card.cards[i].cardTier].ToString();
            if (cardData.card.cards[i].cardRef_a.ToString() != "0")
            {
                if(cardData.card.cards[i].cardID == "203" || cardData.card.cards[i].cardID == "500" || cardData.card.cards[i].cardID == "514")
                {
                    cardInfos[i].text = cardData.card.cards[i].cardInfo.Replace("(a)", cardData.card.cards[i].cardRef_a.ToString());
                }
                else
                {
                    cardInfos[i].text = cardData.card.cards[i].cardInfo.Replace("(a)", (cardData.card.cards[i].cardRef_a * 100).ToString() + '%');
                }

                if (cardData.card.cards[i].cardRef_b.ToString() != "0")
                {
                    cardInfos[i].text = cardInfos[i].text.Replace("(b)", (cardData.card.cards[i].cardRef_b * 100).ToString() + '%');
                    if (cardData.card.cards[i].cardRef_c.ToString() != "0")
                    {
                        cardInfos[i].text = cardInfos[i].text.Replace("(c)", (cardData.card.cards[i].cardRef_c * 100).ToString() + '%');
                    }
                }
            }
            else
            {
                cardInfos[i].text = cardData.card.cards[i].cardInfo;
            }

            buttonImage[i].color = colors[(int)cardData.card.cards[i].cardTier];
            //cardIcons[i].color = colors[(int)cardData.card.cards[i].cardTier];
            //cardNames[i].color = colors[(int)cardData.card.cards[i].cardTier];
            //cardTiers[i].color = colors[(int)cardData.card.cards[i].cardTier];
            
        }
        
    }

    public void CardLeftClicked()
    {
        cardApply.ApplyCardData(0);

        GameStop(false);
    }

    public void CardCenterClicked()
    {
        cardApply.ApplyCardData(1);
        GameStop(false);
    }

    public void CardRightClicked()
    {
        cardApply.ApplyCardData(2);
        GameStop(false);
    }

    void GameStop(bool _isStop)
    {
        for (int i = 0; i < activateUI.Length; i++)
        {
            if (activateUI_activated[i])
            {
                activateUI[i].SetActive(!activateUI[i].activeSelf);
            }
        }

        if(playerStat.player_state != Unit_Status.State.InBuilding)
        {
            dPad.SetActive(!_isStop);
        }
        else if(playerStat.player_state == Unit_Status.State.InBuilding)
        {
            buildingUI.SetActive(!_isStop);
        }

        cardUI.SetActive(_isStop);
        upgradeButton.SetActive(!_isStop);
        if (!_isStop)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }
    }
}
