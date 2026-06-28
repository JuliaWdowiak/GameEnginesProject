using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Objects.Actors.Player
{
    [RequireComponent(typeof(ActorController))]
    public class PlayerInputController : MonoBehaviour
    {
        public InputActionReference move;
        public InputActionReference jump;
        public InputActionReference attack;
        public InputActionReference interact;

        public InputActionReference openMenu;
        public GameObject Menu;

        private ActorController _player;

        private void Start()
        {
            _player = GetComponent<ActorController>();
        }

        private void Update()
        {
            if (move.action.ReadValue<Vector2>().x > 0)
            {
                _player.StateMachine.RegisterStateChange(ActorState.Running);
                _player.StateMachine.RegisterStateChange(LookDirection.Right);
            }
            else if (move.action.ReadValue<Vector2>().x < 0)
            {
                _player.StateMachine.RegisterStateChange(ActorState.Running);
                _player.StateMachine.RegisterStateChange(LookDirection.Left);
            }
            else if (move.action.ReadValue<Vector2>().x == 0)
            {
                _player.StateMachine.RegisterStateChange(ActorState.Standing);
            }
        }

        public void OnEnable()
        {
            jump.action.started += Jump;
            jump.action.canceled += StopJump;

            attack.action.started += Attack;
            interact.action.started += Interact;
            openMenu.action.started += OpenMenu;
        }
        public void OnDisable()
        {
            jump.action.started -= Jump;
            jump.action.canceled -= StopJump;

            attack.action.started -= Attack;
            interact.action.started -= Interact;
            openMenu.action.started -= OpenMenu;
        }

        private void StopJump(InputAction.CallbackContext context)
        {
            _player.StateMachine.RegisterStateChange(ActorState.StoppingJump);
        }

        private void Jump(InputAction.CallbackContext context)
        {
            _player.StateMachine.RegisterStateChange(ActorState.Jumping);
        }

        private void Attack(InputAction.CallbackContext context)
        {
            _player.StateMachine.RegisterStateChange(ActorState.StartingAttack);
        }
        private void Interact(InputAction.CallbackContext context)
        {
            _player.StateMachine.RegisterStateChange(ActorState.Interacting);
        }

        private void OpenMenu(InputAction.CallbackContext context)
        {
            if (Menu == null) return;
            Menu.SetActive(!Menu.activeSelf);
        }
    }
}
