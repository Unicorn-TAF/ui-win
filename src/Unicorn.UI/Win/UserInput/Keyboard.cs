using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unicorn.UI.Win.UserInput.WindowsApi;

namespace Unicorn.UI.Win.UserInput
{
    // BUG: KeysConverter

    /// <summary>
    /// Represents Keyboard attachment to the machine.
    /// </summary>
    public class Keyboard
    {
        private readonly List<SpecialKeys> _scanCodeDependent = 
            new List<SpecialKeys>
            {
                SpecialKeys.RightAlt,
                SpecialKeys.Insert,
                SpecialKeys.Delete,
                SpecialKeys.Left,
                SpecialKeys.Home,
                SpecialKeys.End,
                SpecialKeys.Up,
                SpecialKeys.Down,
                SpecialKeys.PageUp,
                SpecialKeys.PageDown,
                SpecialKeys.Right,
                SpecialKeys.LeftWin,
                SpecialKeys.RightWin
            };

        private readonly List<int> _keysHeld = new List<int>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Keyboard"/> class.
        /// </summary>
        private Keyboard()
        {
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        /// <summary>
        /// See <a href="http://pinvoke.net/default.aspx/user32/SendInput.html">SendInput usage (user32)</a><para/>
        /// See <a href="http://delphi.about.com/od/objectpascalide/l/blvkc.htm">Virtual key codes</a>
        /// </summary>
        public enum SpecialKeys
        {
            Backspace = 0x08,
            Tab = 0x09,
            Enter = 0x0D,
            Shift = 0x10,
            Control = 0x11,
            Alt = 0x12,
            Pause = 0x13,
            CapsLock = 0x14,
            Escape = 0x1B,
            Space = 0x20,
            PageUp = 0x21,
            PageDown = 0x22,
            End = 0x23,
            Home = 0x24,
            Left = 0x25,
            Up = 0x26,
            Right = 0x27,
            Down = 0x28,
            Print = 0x2A,
            PrintScreen = 0x2C,
            Insert = 0x2D,
            Delete = 0x2E,
            LeftWin = 0x5B,
            RightWin = 0x5C,
            ContextMenu = 0x5D,
            F1 = 0x70,
            F2 = 0x71,
            F3 = 0x72,
            F4 = 0x73,
            F5 = 0x74,
            F6 = 0x75,
            F7 = 0x76,
            F8 = 0x77,
            F9 = 0x78,
            F10 = 0x79,
            F11 = 0x7A,
            F12 = 0x7B,
            NumLock = 0x90,
            ScrollLock = 0x91,
            LeftAlt = 0xA4,
            RightAlt = 0xA5
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Gets keyboard instance.
        /// Use Window.Keyboard method to get handle to the Keyboard. Keyboard instance got using this method would not wait while the application
        /// is busy.
        /// </summary>
        public static Keyboard Instance = new Keyboard();

        /// <summary>
        /// Gets or sets a value indicating whether Caps Lock mode is ON
        /// </summary>
        public bool CapsLockOn
        {
            get
            {
                ushort state = NativeMethods.GetKeyState((uint)SpecialKeys.CapsLock);
                return state != 0;
            }

            set
            {
                if (CapsLockOn != value)
                {
                    Send(SpecialKeys.CapsLock, true);
                }
            }
        }

        /// <summary>
        /// Gets help special keys.
        /// </summary>
        public List<SpecialKeys> HeldKeys { get; } = new List<SpecialKeys>();

        /// <summary>
        /// press sequence of keys
        /// </summary>
        /// <param name="keysToType">keys to type</param>
        /// <returns>keyboard instance</returns>
        public Keyboard Type(string keysToType)
        {
            CapsLockOn = false;

            foreach (char c in keysToType)
            {
                short key = NativeMethods.VkKeyScan(c);
                if (c.Equals('\r'))
                {
                    continue;
                }

                if (ShiftKeyIsNeeded(key))
                {
                    SendKeyDown((short)SpecialKeys.Shift, false);
                }
                    
                Press(key, false);

                if (ShiftKeyIsNeeded(key))
                {
                    SendKeyUp((short)SpecialKeys.Shift, false);
                }
            }

            return this;
        }

        /// <summary>
        /// Presses one of <see cref="SpecialKeys"/>
        /// </summary>
        /// <param name="key">special key to press</param>
        /// <returns>keyboard instance</returns>
        public Keyboard PressSpecialKey(SpecialKeys key)
        {
            Send(key, true);
            return this;
        }

        /// <summary>
        /// Holds one of <see cref="SpecialKeys"/>
        /// </summary>
        /// <param name="key">special key to hold</param>
        /// <returns>keyboard instance</returns>
        public Keyboard HoldKey(SpecialKeys key)
        {
            SendKeyDown((short)key, true);
            HeldKeys.Add(key);
            return this;
        }

        /// <summary>
        /// Leaves specific <see cref="SpecialKeys"/>
        /// </summary>
        /// <param name="key">special key to leave</param>
        /// <returns>keyboard instance</returns>
        public Keyboard LeaveKey(SpecialKeys key)
        {
            SendKeyUp((short)key, true);
            HeldKeys.Remove(key);
            return this;
        }

        /// <summary>
        /// Leaves all <see cref="SpecialKeys"/>
        /// </summary>
        /// <returns>keyboard instance</returns>
        public Keyboard LeaveAllKeys()
        {
            new List<SpecialKeys>(HeldKeys).ForEach(LeaveSingleKey);
            return this;
        }

        private static bool ShiftKeyIsNeeded(short key) =>
            ((key >> 8) & 1) == 1;

        private void LeaveSingleKey(SpecialKeys key)
        {
            SendKeyUp((short)key, true);
            HeldKeys.Remove(key);
        }

        private KeyUpDown GetSpecialKeyCode(bool specialKey, KeyUpDown key)
        {
            if (specialKey && _scanCodeDependent.Contains((SpecialKeys)key))
            {
                key |= KeyUpDown.KEYEVENTF_EXTENDEDKEY;
            }

            return key;
        }

        private void SendInput(Input input) =>
            NativeMethods.SendInput(1, ref input, Marshal.SizeOf(typeof(Input)));

        private Input GetInputFor(short character, KeyUpDown keyUpOrDown) =>
            Input.Keyboard(new KeyboardInput(character, keyUpOrDown, NativeMethods.GetMessageExtraInfo()));

        private void Press(short key, bool specialKey)
        {
            SendKeyDown(key, specialKey);
            SendKeyUp(key, specialKey);
        }

        private void Send(SpecialKeys key, bool specialKey) =>
            Press((short)key, specialKey);

        private void SendKeyUp(short b, bool specialKey)
        {
            if (!_keysHeld.Contains(b))
            {
                throw new InvalidOperationException($"Cannot press the key {b} as its already pressed");
            }

            _keysHeld.Remove(b);
            KeyUpDown keyUpDown = GetSpecialKeyCode(specialKey, KeyUpDown.KEYEVENTF_KEYUP);
            SendInput(GetInputFor(b, keyUpDown));
        }

        private void SendKeyDown(short b, bool specialKey)
        {
            if (_keysHeld.Contains(b))
            {
                throw new InvalidOperationException($"Cannot press the key {b} as its already pressed");
            }

            _keysHeld.Add(b);
            KeyUpDown keyUpDown = GetSpecialKeyCode(specialKey, KeyUpDown.KEYEVENTF_KEYDOWN);
            SendInput(GetInputFor(b, keyUpDown));
        }
    }
}
