using Microsoft.Xna.Framework.Input;

namespace StardustDefender.Core.Components
{
    /// <summary>
    /// Static wrapper of engine input information.
    /// </summary>
    public static class SInput
    {
        /// <summary>
        /// Current state the player's mouse is in.
        /// </summary>
        public static MouseState Mouse => _mouseState;

        /// <summary>
        /// Current state the player's keyboard is in.
        /// </summary>
        public static KeyboardState Keyboard => _keyboardState;

        public static MouseState PreviousMouse => _previousMouseState;
        public static KeyboardState PreviousKeyboard => _previousKeyboardState;

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
        public static int GetDeltaScrollWheel()
        {
            return _previousMouseState.ScrollWheelValue - _mouseState.ScrollWheelValue;
        }

        public static bool Started(Keys key)
        {
            return !PreviousKeyboard.IsKeyDown(key) &&
                Keyboard.IsKeyDown(key);
        }

        public static bool Performed(Keys key)
        {
            return PreviousKeyboard.IsKeyDown(key) &&
                Keyboard.IsKeyDown(key);
        }

        public static bool Canceled(Keys key)
        {
            return PreviousKeyboard.IsKeyDown(key) &&
                !Keyboard.IsKeyDown(key);
        }
    }
}
