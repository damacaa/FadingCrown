using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput input;
    private InputController inputs;
    private UIManager uiManager;
    float inputX;
    bool floatIng;

    private Player _player;
    private bool inGame = true;

    private void Awake()
    {
        input = this.GetComponent<PlayerInput>();
        inputs = new InputController();
        _player = GameObject.FindObjectOfType<Player>();
        uiManager = GameObject.FindObjectOfType<UIManager>();


        inputs.Player.Move.performed += ctx => Move(ctx.ReadValue<float>());
        inputs.Player.Move.canceled += ctx => KeyMoveReleased(ctx.ReadValue<float>());
        inputs.Player.Attack.performed += ctx => Attack();
        inputs.Player.Dash.performed += ctx => Dash();
        inputs.Player.Down.performed += ctx => Down();
        inputs.Player.Jump.performed += ctx => Jump();
        inputs.Player.Jump.canceled += ctx => KeyJumpReleased();
        inputs.Player.Pause.performed += ctx => Pause();

        inputs.Player.Enable();

    }

    private void Update()
    {
        _player.Move(inputX);

        if (floatIng)
        {
            _player.Float();
        }

    }

    private void Pause()
    {
        uiManager.Paused();
    }

    private void KeyMoveReleased(float direction)
    {
        //Debug.Log("Release de move "+ direction);
        inputX = 0f;
    }

    private void KeyJumpReleased()
    {
        //Debug.Log("release de jump");
        floatIng = false;
    }

    private void Move(float direction)
    {
        if (inGame)
            inputX = direction;        
    }

    private void Attack()
    {

        _player.Attack();
    }

    private void Dash()
    {

        _player.Dash();
    }

    private void Jump()
    {

        //metodo para recoger si se toca el suelo o no
        _player.Jump();
        floatIng = true;
    }

    private void Down()
    {

        if (inGame)
            _player.Down();
    }


    private void OnEnable()
    {
        inputs.Enable();
        inputs.Player.Enable();
    }

    private void OnDisable()
    {
        inputs.Disable();

        inputs.Player.Disable();
    }

    public void SetInGame(bool value)
    {
        inGame = value;
    }

}
