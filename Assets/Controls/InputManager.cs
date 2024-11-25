using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class InputManager : MonoBehaviour, GameInput.IGamePlayActions, GameInput.IUIActions
{
    // Instance of GameInput from the player controler
    private GameInput gameinput;
    private void OnEnable()
    {
        //Creates a new instance of the gameinput Script if it isnt currently asigned
        if (gameinput == null)
        {
            gameinput = new GameInput();

            gameinput.GamePlay.SetCallbacks(this);
            gameinput.UI.SetCallbacks(this);

            SetGamePlay();
            
        }
    }

    private void OnDisable()
    {
        //this disables the inputs when the object is deactivated to stop prefromance isues
        gameinput.GamePlay.Disable();
        gameinput.UI.Disable();
    }
    public void SetGamePlay() 
    {
        gameinput.UI.Disable();
        gameinput.GamePlay.Enable();
    }

    public void SetUI() 
    {
        gameinput.GamePlay.Disable();
        gameinput.UI.Enable();
    }

    //defines event to send information to other scripts
    public event Action<Vector2> MoveEvent;
  
    public event Action JumpEvent;
    public event Action JumpEventCancel;

    public event Action DashEvent;
    public event Action DashEventCancel;

    public event Action PauseEvent;
    public event Action ResumeEvent;

    public void OnMove(InputAction.CallbackContext context)
    {
        //sends vector2 to the moveevent (The question mark is to stop it from sending data if the event is null because that will cause a error)
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }
    public void OnJump(InputAction.CallbackContext context) 
    {
        //Checks if button is jump is being press if the button is released sends a cancel event
        if (context.performed) 
        {
            JumpEvent?.Invoke();
        }
        if (context.canceled) 
        {
            JumpEventCancel?.Invoke();
        }
    }
    public void OnDash(InputAction.CallbackContext context) 
    {
        //Same logic as jump
        if (context.performed)
        {
            DashEvent?.Invoke();
        }
        if (context.canceled)
        {
            DashEventCancel?.Invoke();
        }
    }
    public void OnPause(InputAction.CallbackContext context) 
    {
        PauseEvent?.Invoke();
    }
    public void OnResume(InputAction.CallbackContext context) 
    {
        ResumeEvent?.Invoke();
    }
}
