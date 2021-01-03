using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestInput : MonoBehaviour
{
    [SerializeField] private InputActionAsset Actions;
    private InputAction Move;
    private InputAction Look;
    private InputAction Sprint;
    private InputAction Cro;

    private void Start()
    {
        Move = Actions["Player/Move"];
        Look = Actions["Player/Look"];
        Sprint = Actions["Player/Sprint"];
        Cro = Actions["Player/Cro"];
    }

    public void MMove(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<Vector2>());
    }

    public void LLook(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<Vector2>());
    }

    public void SSprint(InputAction.CallbackContext context)
    {
        Debug.Log(context.performed);
    }

}
