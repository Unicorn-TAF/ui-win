using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Unicorn.UI.Core.UserInput.WindowsApi;

namespace Unicorn.UI.Core.UserInput
{
    // BUG: KeysConverter

    /// <summary>
    /// Represents Keyboard attachment to the machine.
    /// </summary>
    public class Keyboard
    {
        /// <summary>
        /// Use Window.Keyboard method to get handle to the Keyboard. Keyboard instance got using this method would not wait while the application
        /// is busy.
        /// </summary>
        private static Keyboard instance;

        private readonly List<SpecialKeys> ScanCodeDependent = 
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

        private readonly List<SpecialKeys> heldKeys = new List<SpecialKeys>();

        private readonly List<int> keysHeld = new List<int>();

        protected Keyboard()
        {
        }

        public static Keyboard Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Keyboard();
                }

                return instance;
            }
        }

        public virtual bool CapsLockOn
        {
            get
            {
                ushort state = GetKeyState((uint)SpecialKeys.CapsLock);
                return state != 0;
            }

            set
            {
                if (this.CapsLockOn != value)
                {
                    Send(SpecialKeys.CapsLock, true);
                }
            }
        }

        public virtual List<SpecialKeys> HeldKeys => this.heldKeys;

        public virtual void Enter(string keysToType) =>
            Send(keysToType);

        public virtual void Send(string keysToType)
        {
            this.CapsLockOn = false;

            foreach (char c in keysToType)
            {
                short key = VkKeyScan(c);
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
        }

        public virtual void PressSpecialKey(SpecialKeys key) =>
            Send(key, true);

        public virtual void HoldKey(SpecialKeys key)
        {
            SendKeyDown((short)key, true);
            this.heldKeys.Add(key);
        }

        public virtual void LeaveKey(SpecialKeys key)
        {
            SendKeyUp((short)key, true);
            this.heldKeys.Remove(key);
        }

        public virtual void LeaveAllKeys() =>
            new List<SpecialKeys>(this.heldKeys).ForEach(LeaveKey);

        [DllImport("user32", EntryPoint = "SendInput")]
        private static extern int SendInput(uint numberOfInputs, ref Input input, int structSize);

        [DllImport("user32.dll")]
        private static extern IntPtr GetMessageExtraInfo();

        [DllImport("user32.dll")]
        private static extern short VkKeyScan(char ch);

        [DllImport("user32.dll")]
        private static extern ushort GetKeyState(uint virtKey);

        private static bool ShiftKeyIsNeeded(short key) =>
            ((key >> 8) & 1) == 1;

        private KeyUpDown GetSpecialKeyCode(bool specialKey, KeyUpDown key)
        {
            if (specialKey && ScanCodeDependent.Contains((SpecialKeys)key))
            {
                key |= KeyUpDown.KEYEVENTF_EXTENDEDKEY;
            }

            return key;
        }

        private void SendInput(Input input) =>
            SendInput(1, ref input, Marshal.SizeOf(typeof(Input)));

        private Input GetInputFor(short character, KeyUpDown keyUpOrDown) =>
            Input.Keyboard(new KeyboardInput(character, keyUpOrDown, GetMessageExtraInfo()));

        private void Press(short key, bool specialKey)
        {
            SendKeyDown(key, specialKey);
            SendKeyUp(key, specialKey);
        }

        private void Send(SpecialKeys key, bool specialKey) =>
            Press((short)key, specialKey);

        private void SendKeyUp(short b, bool specialKey)
        {
            if (!this.keysHeld.Contains(b))
            {
                throw new InvalidOperationException(string.Format("Cannot press the key {0} as its already pressed", b));
            }

            this.keysHeld.Remove(b);
            KeyUpDown keyUpDown = GetSpecialKeyCode(specialKey, KeyUpDown.KEYEVENTF_KEYUP);
            SendInput(GetInputFor(b, keyUpDown));
        }

        private void SendKeyDown(short b, bool specialKey)
        {
            if (this.keysHeld.Contains(b))
            {
                throw new InvalidOperationException(string.Format("Cannot press the key {0} as its already pressed", b));
            }

            this.keysHeld.Add(b);
            KeyUpDown keyUpDown = GetSpecialKeyCode(specialKey, KeyUpDown.KEYEVENTF_KEYDOWN);
            SendInput(GetInputFor(b, keyUpDown));
        }

        /// <summary>
        /// http://pinvoke.net/default.aspx/user32/SendInput.html <para/>
        /// http://delphi.about.com/od/objectpascalide/l/blvkc.htm
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
    }
}
