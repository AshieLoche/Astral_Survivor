using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerController controller;
    private static InputManager _instance;
    public static InputManager Instance {  get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } 
        else
        {
            _instance = this;
        }

        controller = new PlayerController();
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        controller.Enable();
    }

    private void OnDisable()
    {
        controller.Disable();
    }

    public Vector2 GetMovement()
    {
        return controller.Player.Movement.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta()
    {
        return controller.Player.Look.ReadValue<Vector2>();
    }

    public bool GetJumpStatus()
    {
        return controller.Player.Jump.triggered;
    }

    public bool GetRunStatus()
    {
        return controller.Player.Run.IsInProgress();
    }

    public bool GetMovementStatus()
    {
        return controller.Player.Movement.IsInProgress();
    }
}
