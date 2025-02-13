using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class InputManager : MonoBehaviour
{
    
    [SerializeField] private DynamicJoystick joystick;
    
    #region INPUT 
    
    public void OnTouch(InputValue value)
    {
        joystick.TouchState = value.Get<TouchState>();
        //print("Touch");
    }
    
    #endregion
}
