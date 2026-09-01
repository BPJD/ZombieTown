using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControllerMove : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform lever;
    public RectTransform rectTransform;

    public Button upgradeButton;

    public Canvas mainCanvas;

    [SerializeField, Range(1, 150)]
    public float leverRange;

    Vector2 inputDirection;
    bool isInput;

    [SerializeField]
    Player_Move player;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        player.StopOrder();
        ControlJoystickLever(eventData);
        isInput = true;
        upgradeButton.enabled = false;
        player.SendMessage("MoveAniPlay", SendMessageOptions.DontRequireReceiver);
    }

    public void OnDrag(PointerEventData eventData)
    {
        ControlJoystickLever(eventData);
    }

    void ControlJoystickLever(PointerEventData eventData)
    {

        var scaledAnchoredPosition = rectTransform.anchoredPosition * mainCanvas.transform.localScale.x;
        var inputPos = eventData.position - scaledAnchoredPosition;
        var inputVector = inputPos.magnitude < leverRange ? inputPos : inputPos.normalized * leverRange;
        lever.anchoredPosition = inputVector;
        inputDirection = inputVector / leverRange;


        //var inputPos = eventData.position - rectTransform.anchoredPosition;
        //var inputVector = inputPos.magnitude < leverRange ? inputPos : inputPos.normalized * leverRange;
        //lever.anchoredPosition = inputVector;
        //inputDirection = inputVector / leverRange;
        //Debug.Log(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;
        isInput = false;
        player.StopOrder();
        upgradeButton.enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isInput)
        {
            InputControlVector();
        }
    }

    void InputControlVector()
    {
        player.Move(inputDirection);
    }

}
