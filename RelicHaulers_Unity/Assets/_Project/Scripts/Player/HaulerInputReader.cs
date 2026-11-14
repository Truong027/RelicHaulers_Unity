using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RelicHaulers.Player
{
    /// <summary>
    /// Reads PC & Gamepad controls for Gate 1 Prototype using Unity Input System.
    /// Hold Left Shift = Sprint (free); Quick Tap Left Shift / V / Mouse4 / East = Dash.
    /// </summary>
    public class HaulerInputReader : MonoBehaviour
    {
        [Header("Dash Tap Threshold")]
        [SerializeField] private float shiftTapMaxDuration = 0.20f;

        private InputActionMap playerMap;
        private InputAction moveAction;
        private InputAction lookAction;
        private InputAction jumpAction;
        private InputAction sprintAction;
        private InputAction dashAction;
        private InputAction interactAction;
        private InputAction attackAction;
        private InputAction aimThrowAction;
        private InputAction resetAction;

        private float shiftPressedAt = -1f;
        private bool jumpBufferedFlag;
        private bool dashTriggeredFlag;
        private bool interactPressedFlag;
        private bool attackPressedFlag;
        private bool resetPressedFlag;

        public Vector2 MoveInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public bool IsJumpHeld { get; private set; }
        public bool IsSprintHeld { get; private set; }
        public bool IsAimThrowHeld { get; private set; }

        public bool ConsumeJumpPressed()
        {
            bool v = jumpBufferedFlag;
            jumpBufferedFlag = false;
            return v;
        }

        public bool ConsumeDashPressed()
        {
            bool v = dashTriggeredFlag;
            dashTriggeredFlag = false;
            return v;
        }

        public bool ConsumeInteractPressed()
        {
            bool v = interactPressedFlag;
            interactPressedFlag = false;
            return v;
        }

        public bool ConsumeAttackPressed()
        {
            bool v = attackPressedFlag;
            attackPressedFlag = false;
            return v;
        }

        public bool ConsumeResetPressed()
        {
            bool v = resetPressedFlag;
            resetPressedFlag = false;
            return v;
        }

        private void Awake()
        {
            BuildInputActions();
        }

        private void OnEnable()
        {
            playerMap?.Enable();
        }

        private void OnDisable()
        {
            playerMap?.Disable();
        }

        private void Update()
        {
            MoveInput = moveAction.ReadValue<Vector2>();
            LookInput = lookAction.ReadValue<Vector2>();
            IsJumpHeld = jumpAction.IsPressed();
            IsSprintHeld = sprintAction.IsPressed();
            IsAimThrowHeld = aimThrowAction.IsPressed();

            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                bool locked = Cursor.lockState == CursorLockMode.Locked;
                Cursor.lockState = locked ? CursorLockMode.None : CursorLockMode.Locked;
                Cursor.visible = locked;
            }
        }

        private void BuildInputActions()
        {
            playerMap = new InputActionMap("Player");

            moveAction = playerMap.AddAction("Move", InputActionType.Value);
            moveAction.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/w")
                .With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a")
                .With("Right", "<Keyboard>/d");
            moveAction.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/upArrow")
                .With("Down", "<Keyboard>/downArrow")
                .With("Left", "<Keyboard>/leftArrow")
                .With("Right", "<Keyboard>/rightArrow");
            moveAction.AddBinding("<Gamepad>/leftStick");

            lookAction = playerMap.AddAction("Look", InputActionType.Value);
            lookAction.AddBinding("<Mouse>/delta");
            lookAction.AddBinding("<Gamepad>/rightStick");

            jumpAction = playerMap.AddAction("Jump", InputActionType.Button);
            jumpAction.AddBinding("<Keyboard>/space");
            jumpAction.AddBinding("<Gamepad>/buttonSouth");
            jumpAction.performed += _ => jumpBufferedFlag = true;

            sprintAction = playerMap.AddAction("Sprint", InputActionType.Button);
            sprintAction.AddBinding("<Keyboard>/leftShift");
            sprintAction.AddBinding("<Gamepad>/leftStickPress");
            sprintAction.started += _ => shiftPressedAt = Time.unscaledTime;
            sprintAction.canceled += _ =>
            {
                if (shiftPressedAt > 0f && (Time.unscaledTime - shiftPressedAt) <= shiftTapMaxDuration)
                {
                    dashTriggeredFlag = true;
                }
                shiftPressedAt = -1f;
            };

            dashAction = playerMap.AddAction("Dash", InputActionType.Button);
            dashAction.AddBinding("<Keyboard>/v");
            dashAction.AddBinding("<Mouse>/forwardButton");
            dashAction.AddBinding("<Gamepad>/buttonEast");
            dashAction.performed += _ => dashTriggeredFlag = true;

            interactAction = playerMap.AddAction("Interact", InputActionType.Button);
            interactAction.AddBinding("<Keyboard>/e");
            interactAction.AddBinding("<Gamepad>/buttonWest");
            interactAction.performed += _ => interactPressedFlag = true;

            attackAction = playerMap.AddAction("Attack", InputActionType.Button);
            attackAction.AddBinding("<Mouse>/leftButton");
            attackAction.AddBinding("<Gamepad>/rightShoulder");
            attackAction.performed += _ => attackPressedFlag = true;

            aimThrowAction = playerMap.AddAction("AimThrow", InputActionType.Button);
            aimThrowAction.AddBinding("<Mouse>/rightButton");
            aimThrowAction.AddBinding("<Gamepad>/rightTrigger");

            resetAction = playerMap.AddAction("ResetPos", InputActionType.Button);
            resetAction.AddBinding("<Keyboard>/r");
            resetAction.performed += _ => resetPressedFlag = true;
        }
    }
}
