using Microsoft.Xna.Framework.Input;

namespace StardustDefender.Engine
{
    /// <summary>
    /// Static wrapper of engine input information.
    /// </summary>
    internal static class SInput
    {
        /// <summary>
        /// Current state the player's mouse is in.
        /// </summary>
        internal static MouseState Mouse => _mouseState;

        /// <summary>
        /// Current state the player's keyboard is in.
        /// </summary>
        internal static KeyboardState Keyboard => _keyboardState;

        internal static MouseState PreviousMouse => _previousMouseState;
        internal static KeyboardState PreviousKeyboard => _previousKeyboardState;

        private static MouseState _previousMouseState;
        private static KeyboardState _previousKeyboardState;

        private static MouseState _mouseState;
        private static KeyboardState _keyboardState;

        /// <summary>
        /// Updates and correctly configures all project inputs.
        /// </summary>
        internal static void Update()
        {
            _previousMouseState = _mouseState;
            _previousKeyboardState = _keyboardState;

            _mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            _keyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
        }

        /// <summary>
        /// Calculates the change in the scroll wheel value between the previous and current states of the mouse input.
        /// </summary>
        /// <returns>The difference in scroll wheel value.</returns>
        internal static int GetDeltaScrollWheel()
        {
            return _previousMouseState.ScrollWheelValue - _mouseState.ScrollWheelValue;
        }

        internal static bool Started(Keys key)
        {
            if (!PreviousKeyboard.IsKeyDown(key) &&
                Keyboard.IsKeyDown(key))
            {
                return true;
            }

            return false;
        }

        internal static bool Performed(Keys key)
        {
            if (PreviousKeyboard.IsKeyDown(key) &&
                Keyboard.IsKeyDown(key))
            {
                return true;
            }

            return false;
        }

        internal static bool Canceled(Keys key)
        {
            if (PreviousKeyboard.IsKeyDown(key) &&
                !Keyboard.IsKeyDown(key))
            {
                return true;
            }

            return false;
        }
    }
}
