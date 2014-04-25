using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace XRay.UI {
    public enum XRayKeyCode {
        JoystickLeftArrow,
        JoystickRightArrow,
        JoystickUpArrow,
        JoystickDownArrow,
        JoystickLeftTrigger,
        JoystickRightTrigger
    }

    public static class XRayInput {
        private static Dictionary<XRayKeyCode, bool> _keyLastState = new Dictionary<XRayKeyCode, bool>();
        private static readonly Dictionary<XRayKeyCode, bool> KeyState = new Dictionary<XRayKeyCode, bool>();

        public static bool AnyKeyDown {
            get {
                return
                    KeyState.Any(code => _keyLastState.ContainsKey(code.Key) && !_keyLastState[code.Key] && code.Value);
            }
        }

        public static bool AnyKeyUp {
            get {
                return
                    KeyState.Any(code => _keyLastState.ContainsKey(code.Key) && _keyLastState[code.Key] && !code.Value);
            }
        }

        public static bool AnyKey {
            get { return KeyState.Any(code => code.Value); }
        }

        public static Dictionary<XRayKeyCode, bool> LastKeyStates {
            get {
                var cpy = _keyLastState;
                return cpy;
            }
        }

        public static bool GetKeyDown(XRayKeyCode code) {
            return _keyLastState.ContainsKey(code) && !_keyLastState[code] &&
                   KeyState.ContainsKey(code) && KeyState[code];
        }

        public static bool GetKeyUp(XRayKeyCode code) {
            return _keyLastState.ContainsKey(code) && _keyLastState[code] &&
                   KeyState.ContainsKey(code) && !KeyState[code];
        }

        public static bool GetKey(XRayKeyCode code) {
            var test = false;

            switch (code) {
                case XRayKeyCode.JoystickLeftArrow:
                    test = Input.GetAxis("Arrows H") <= -1f;
                    break;
                case XRayKeyCode.JoystickRightArrow:
                    test = Input.GetAxis("Arrows H") >= 1f;
                    break;
                case XRayKeyCode.JoystickDownArrow:
                    test = Input.GetAxis("Arrows V") <= -1f;
                    break;
                case XRayKeyCode.JoystickUpArrow:
                    test = Input.GetAxis("Arrows V") >= 1f;
                    break;
                case XRayKeyCode.JoystickLeftTrigger:
                    test = Input.GetAxis("Triggers") <= 1f;
                    break;
                case XRayKeyCode.JoystickRightTrigger:
                    test = Input.GetAxis("Triggers") >= 1f;
                    break;
            }

            return test;
        }

        public static void Update() {
            _keyLastState = new Dictionary<XRayKeyCode, bool>(KeyState);
            KeyState.Clear();
            for (var i = XRayKeyCode.JoystickLeftArrow; i != XRayKeyCode.JoystickDownArrow; i++) {
                KeyState.Add(i, GetKey(i));
            }
        }

        public static bool GetKeysDown(params KeyCode[] codes) {
            return codes.All(Input.GetKey) && Input.GetKeyDown(codes.Last());
        }

        /// <summary>
        /// Returns a Vector with both axis of the right stick.
        /// </summary>
        /// <returns></returns>
        public static Vector2 GetRightStick()
        {
            return new Vector2(
                Mathf.Round(!Input.GetAxis("Joy X").Equals(0f)
                                ? Input.GetAxis("Joy X")
                                : Input.GetAxis("PlusMinus")),
                Mathf.Round(!Input.GetAxis("Joy Y").Equals(0f)
                                ? Input.GetAxis("Joy Y")
                                : Input.GetAxis("PlusMinus")));
        }
    }
}