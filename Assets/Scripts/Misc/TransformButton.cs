using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace XRay
{
	public class TransformButton
	{
		public Dictionary<string, TransformButton> ChildButtons = new Dictionary<string, TransformButton>();
		public Texture BtnTexture;
		public bool Active = true;
		public bool Enable = false;
		public KeyCode JoystickButton;
		public KeyCode KeyboardButton;
		public Vector2 Position;

		public float Spacing = 100f;
		public float VerticalOffset = 80f;
		public float TransitionSpeed = 3f;

		public delegate void ButtonDelegate(string Name);
		public event ButtonDelegate OnPress;

		protected string name;

		public TransformButton this[string i]{
			get { return ChildButtons[i]; }
			set { ChildButtons[i] = value; }
		}

		public bool HasChild {
			get { return ChildButtons.Count != 0; }
		}

		public TransformButton (){
			this.Position = new Vector2();
			this.name = "";
		}

		public void Init() {
			foreach (var btn in ChildButtons) {
				btn.Value.name = btn.Key;
				btn.Value.Position = Position;
				btn.Value.OnPress += (Name) => {
					if (OnPress != null)
						OnPress((name != "" ? name + "." : "") + Name);
				};
				btn.Value.Init();
			}
		}

		protected void Draw (Vector2 position) {
			this.Position = Vector2.Lerp(this.Position, position, Time.deltaTime * TransitionSpeed);
			DrawButtonTree(new Vector2(this.Position.x + BtnTexture.width / 2, this.Position.y));
			GUI.DrawTexture(new Rect(this.Position.x, this.Position.y, BtnTexture.width, BtnTexture.height), BtnTexture);
		}

		public void DrawButtonTree (Vector2 position) {
			var activeBtns = ChildButtons.Where(b => b.Value.Active);
			if (activeBtns.Count() == 0) return;

			var i = 0;
			TransformButton previous = null;
			var width = activeBtns.Sum(b => b.Value.BtnTexture.width) + Spacing * (activeBtns.Count() - 1);
			foreach (var btn in activeBtns) {
				if (Enable) {
					btn.Value.Draw(new Vector2(previous != null ? previous.Position.x + previous.BtnTexture.width + Spacing : position.x - width / 2, position.y - VerticalOffset));
					i++;
					previous = btn.Value;
				} else {
					btn.Value.Draw(this.Position);
				}
			}
		}

		/// <summary>
		/// Disables the child buttons plus itself.
		/// </summary>
		public void DisableTree() {
			Enable = false;
			DisableChilds();
		}

		/// <summary>
		/// Disables the child buttons.
		/// </summary>
		public void DisableChilds() {
			ChildButtons.ToList().ForEach(b => b.Value.DisableTree());
		}

		public void Update() {
			if (Input.anyKeyDown) {
				ChildButtons.ToList().ForEach(b => b.Value.GetKeys());
			}
		}

		protected bool GetKeys() {
			if (!Active) return false;
			if (Input.GetKeyDown(JoystickButton) || Input.GetKeyDown(KeyboardButton)) {
				if (HasChild)
					Enable = !Enable;
				else
					Enable = false;
				if (OnPress != null)
					OnPress(name);
			} else {
				if (Enable)
					Enable = ChildButtons.Where(b => b.Value.GetKeys()).Any();
			}
			return Enable;
		}
	}
}

