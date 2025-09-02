using UnityEngine;
using UnityEngine.InputSystem;

namespace Sabih.Common.Components
{
    [DisallowMultipleComponent]
    public class InputDrivenRotator : MonoBehaviour
    {
        [Header("Input")]
        [Tooltip("Vector2 action (e.g. Look). Bind to <Mouse>/delta, <Gamepad>/rightStick, or a 2D Vector.")]
        public InputActionReference lookAction;

        [Header("Rotation Settings")]
        [Tooltip("Horizontal sensitivity (yaw).")]
        public float sensitivityX = 150f;

        [Tooltip("Vertical sensitivity (pitch).")]
        public float sensitivityY = 150f;

        [Header("Clamping")]
        [Tooltip("Clamp pitch (X axis).")]
        public float minPitch = -50f;
        public float maxPitch = 80f;

        [Tooltip("Clamp yaw (Y axis).")]
        public float minYaw = -90f;
        public float maxYaw = 90f;

        [Header("Mouse")]
        [Tooltip("Scales mouse delta only. Gamepad/keys remain unscaled.")]
        public float mouseSensitivity = 0.05f;

        private float yaw;   // rotation around Y (left/right)
        private float pitch; // rotation around X (up/down)

        private InputAction _action;

        void OnEnable()
        {
            if (lookAction == null || lookAction.action == null)
            {
                Debug.LogWarning($"{nameof(InputDrivenRotator)}: No InputActionReference assigned.");
                return;
            }

            _action = lookAction.action;
            _action.Enable();
            _action.performed += OnLook;
        }

        void OnDisable()
        {
            if (_action == null) return;
            _action.performed -= OnLook;
            _action.Disable();
            _action = null;
        }

        private void OnLook(InputAction.CallbackContext ctx)
        {
            Vector2 input = ctx.ReadValue<Vector2>();

            // Apply rotation deltas
            yaw += input.x * sensitivityX * Time.deltaTime;
            pitch -= input.y * sensitivityY * Time.deltaTime; // minus = natural FPS-style look

            // Clamp both axes
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
            yaw = Mathf.Clamp(yaw, minYaw, maxYaw);

            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }
    }

}
