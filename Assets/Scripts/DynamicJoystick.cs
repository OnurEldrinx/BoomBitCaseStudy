using UnityEngine;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.LowLevel;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class DynamicJoystick : OnScreenStick, IPointerUpHandler
{

    #region Private Fields
    
    private RectTransform _canvasRect;
    private Image _stickImage;
    private Image _outerCircleImage;
    private bool _active;
    #endregion

    #region Serialized Fields

    [SerializeField] private RectTransform outerCircle;

    #endregion

    public TouchState TouchState;
    
    private void Awake()
    {
        _canvasRect = transform.root.GetComponent<RectTransform>();
        _stickImage = GetComponent<Image>();
        _outerCircleImage = outerCircle.GetComponent<Image>();
        SetVisibility(0);
    }

    private void Update()
    {
        if (_active) return;
        
        if (TouchState.phase != TouchPhase.Began) return;
        
        MoveStick(TouchState.position);
        SetVisibility(1);
        _active = true;
    }

    private void MoveStick(Vector2 pointerPosition)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, pointerPosition,null, out Vector2 localPoint);
        outerCircle.anchoredPosition = localPoint;
    }
    
    private void SetVisibility(float alpha)
    {
        Color color = _stickImage.color;
        color.a = alpha;
        _stickImage.color = color;
        _outerCircleImage.color = color;
    }

    public new void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        SetVisibility(0);
        _active = false;
    }

    
}
