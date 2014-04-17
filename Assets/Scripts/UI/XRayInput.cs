using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace XRay.UI
{
	public enum XRayKeyCode {
		JoystickLeftArrow,
		JoystickRightArrow,
		JoystickUpArrow,
		JoystickDownArrow
	}

	public static class XRayInput
	{
		private static Dictionary<XRayKeyCode, bool> keyLastState = new Dictionary<XRayKeyCode, bool>();
		private static Dictionary<XRayKeyCode, bool> keyState = new Dictionary<XRayKeyCode, bool>();

		public static bool AnyKeyDown {
			get {
				return keyState.Any(code => keyLastState.ContainsKey(code.Key) && !keyLastState[code.Key] && code.Value);
			}
		}

		public static bool AnyKeyUp {
			get {
				return keyState.Any(code => keyLastState.ContainsKey(code.Key) && keyLastState[code.Key] && !code.Value);
			}
		}

		public static bool AnyKey {
			get {
				return keyState.Any(code => code.Value);
			}
		}

		public static bool GetKeyDown(XRayKeyCode code) {
			return keyLastState.ContainsKey(code) && !keyLastState[code] &&
				keyState.ContainsKey(code) && keyState[code];
		}

		public static bool GetKeyUp(XRayKeyCode code) {
			return keyLastState.ContainsKey(code) && keyLastState[code] &&
				keyState.ContainsKey(code) && !keyState[code];
		}

		public static bool GetKey(XRayKeyCode code) {
			var test = false;
			
			switch(code) {
			case XRayKeyCode.JoystickLeftArrow:
				test = Input.GetAxis("Arrows H") == -1f;
				break;
			case XRayKeyCode.JoystickRightArrow:
				test = Input.GetAxis("Arrows H") == 1f;
				break;
			case XRayKeyCode.JoystickUpArrow:
				test = Input.GetAxis("Arrows V") == -1f;
				break;
			case XRayKeyCode.JoystickDownArrow:
				test = Input.GetAxis("Arrows V") == 1f;
				break;
			}
			
			return test;
		}

		public static void Update() {
			keyLastState = keyState;
			keyState.Clear();
			for (var i = XRayKeyCode.JoystickLeftArrow ; i != XRayKeyCode.JoystickDownArrow ; i++) {
				keyState.Add(i, GetKey(i));
			}
		}
	}
}

