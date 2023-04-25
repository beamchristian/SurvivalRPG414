using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif
using System;

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        public bool interact;
        public bool inventory;
        public InventoryUI inventoryUI; // <--- add this line
        public bool buildMode; // <-- Add this line

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        public static event Action InteractPressed;
        public static event Action InventoryPressed;
        public static event Action BuildModePressed; // <-- Add this line
        public float scroll;
        public static event Action<float> ScrollWheelChanged;

        void Update()
        {
            if (interact)
            {
                InteractPressed?.Invoke();
                interact = false;
            }


            if (inventory)
            {
                InventoryPressed?.Invoke();
                inventory = false;
            }

            if (buildMode)
            {
                Debug.Log("Build mode key pressed."); // Add this line
                BuildModePressed?.Invoke();
                buildMode = false;
            }
        }

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            if (cursorInputForLook)
            {
                LookInput(value.Get<Vector2>());
            }
        }

        public void OnJump(InputValue value)
        {
            JumpInput(value.isPressed);
        }

        public void OnSprint(InputValue value)
        {
            SprintInput(value.isPressed);
        }

        public void OnInteract(InputValue value)
        {
            InteractInput(value.isPressed);
        }
        public void OnInventory(InputValue value)
        {
            InventoryInput(value.isPressed);
        }

        public void OnBuildMode(InputValue value) // <-- Add this method
        {
            BuildModeInput(value.isPressed);
        }

        public void OnScrollWheel(InputValue value)
        {
            ScrollWheelInput(value.Get<float>());
        }
#endif

        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void JumpInput(bool newJumpState)
        {
            jump = newJumpState;
        }

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }

        public void InteractInput(bool newInteractState)
        {
            interact = newInteractState;
        }

        public void InventoryInput(bool newInventoryState)
        {
            inventory = newInventoryState;
        }

        public void BuildModeInput(bool newBuildModeState) // <-- Add this method
        {
            buildMode = newBuildModeState;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }

        public void ScrollWheelInput(float newScrollValue)
        {
            scroll = newScrollValue;
            ScrollWheelChanged?.Invoke(scroll);
        }

    }
}

