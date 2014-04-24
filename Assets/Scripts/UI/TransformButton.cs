using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace XRay.UI {
    public class TransformButtonException : Exception {
        public TransformButtonException(string message) : base(message) {}
    }

    public class TransformButton {
        public delegate bool TriggerAction();

        public List<TransformButton> ChildButtons = new List<TransformButton>();
        public Texture BtnTexture;
        public bool Active = true;
        public bool Enabled = false;
        public KeyCode JoystickButton;
        public KeyCode KeyboardButton;
        public TriggerAction ButtonTrigger;
        public Vector2 Position;

        public float Spacing = 100f;
        public float VerticalOffset = 80f;
        public float TransitionSpeed = 3f;

        public delegate void ButtonDelegate(string name);

        public event ButtonDelegate OnPress;

        private TransformButton _parent;

        /// <summary>
        /// Get a child button by it's name. 
        /// </summary>
        /// <param name="key">Name of the child button</param>
        /// <returns>The child button that corresponds to the given key.</returns>
        public TransformButton this[string key] {
            get { return ChildButtons.First(b => b.Name == key); }
            set { ChildButtons[ChildButtons.FindIndex(b => b.Name == key)] = value; }
        }

        /// <summary>
        /// Returns if this buttons has child buttons.
        /// </summary>
        public bool HasChild {
            get { return ChildButtons.Count != 0; }
        }

        /// <summary>
        /// Checks if this button tree has one child button that is enabled.
        /// </summary>
        public bool HasOneEnabledChild {
            get { return HasChild ? ChildButtons.Any(b => b.HasOneEnabledChild) : Enabled; }
        }

        /// <summary>
        /// Name of the button.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Give name of this button and all its parents separated by points.
        /// </summary>
        public string FullName {
            get { return ((_parent != null && _parent.FullName != "Root") ? _parent.FullName + "." + Name : Name); }
        }

        public TransformButton(string name) {
            if (name == "")
                throw new TransformButtonException("Invalid button name.");
            Position = new Vector2();
            Name = name;
        }

        /// <summary>
        /// Initialize all buttons within the tree.
        /// </summary>
        /// <param name="path">Path to load the texture.</param>
        /// <returns>Returns the button tree.</returns>
        public TransformButton Init(string path) {
            foreach (var btn in ChildButtons) {
                btn._parent = this;
                btn.Position = Position;
                btn.BtnTexture = Resources.Load<Texture>(path + "/" + btn.FullName);
                if (btn.BtnTexture == null)
                    throw new TransformButtonException("Texture for " + btn.FullName + " not loaded.");
                btn.OnPress += name => {
                    if (OnPress != null)
                        OnPress(name);
                };
                btn.Init(path);
            }
            return this;
        }

        /// <summary>
        /// Draw this button.
        /// </summary>
        /// <param name="position">Position for drawing.</param>
        /// <param name="useLerp">Use lerp to calculate position.</param>
        protected void Draw(Vector2 position, bool useLerp = true) {
            Position = useLerp
                           ? Vector2.Lerp(Position, position, Time.deltaTime*TransitionSpeed)
                           : new Vector2(position.x, position.y);
            DrawButtonTree(new Vector2(Position.x + (BtnTexture != null ? BtnTexture.width/2 : 0), Position.y));
            if (BtnTexture != null)
                GUI.DrawTexture(new Rect(Position.x, Position.y, BtnTexture.width, BtnTexture.height), BtnTexture);
        }

        /// <summary>
        /// Draw all child buttons.
        /// </summary>
        /// <param name="position">Origin position where the button tree will be drawn.</param>
        public void DrawButtonTree(Vector2 position) {
            var btns = ChildButtons.Where(b => b.Active).ToArray();
            if (!btns.Any()) return;

            TransformButton previous = null;
            var width = btns.Sum(b => b.BtnTexture.width) + Spacing*(btns.Count() - 1);
            foreach (var btn in btns) {
                if (Enabled) {
                    btn.Draw(
                        new Vector2(
                            previous != null
                                ? previous.Position.x + previous.BtnTexture.width + Spacing
                                : position.x - width/2, position.y - VerticalOffset));
                    previous = btn;
                }
                else {
                    btn.Draw(Position, false);
                }
            }
        }

        /// <summary>
        /// Disables the child buttons plus itself.
        /// </summary>
        public void DisableTree() {
            Enabled = false;
            DisableChilds();
        }

        /// <summary>
        /// Disables the child buttons.
        /// </summary>
        public void DisableChilds() {
            ChildButtons.ForEach(b => b.DisableTree());
        }

        /// <summary>
        /// Updates the button tree.
        /// </summary>
        public void Update() {
            if (!Input.anyKeyDown && !XRayInput.AnyKeyDown) return;
            try {
                ChildButtons.ForEach(b => b.GetKeys());
            }
            catch (CancelCheck e) {
                DisableChilds();
                e.InvokeCallback();
            }
        }

        /// <summary>
        /// Utility exception for the TransformButton class.
        /// </summary>
        private class CancelCheck : Exception {
            private readonly TransformButton _button;
            private readonly bool _enabled;

            public CancelCheck(TransformButton button, bool enabled) {
                _button = button;
                _enabled = enabled;
            }

            /// <summary>
            /// Set the given button and its parents to the given value.
            /// </summary>
            public void InvokeCallback() {
                _button.Enabled = _enabled;
                _button.SetEnableParents(_enabled);
            }
        }

        /// <summary>
        /// Checks if the key bindings where pressed for this button and its childs.
        /// </summary>
        /// <exception cref="CancelCheck">If a keypress is detected, this exception will be fired.</exception>
        private void GetKeys() {
            if (!Active) return;

            // If key wasn't pressed return
            if (Input.GetKeyDown(JoystickButton) || Input.GetKeyDown(KeyboardButton) ||
                (ButtonTrigger != null && ButtonTrigger())) {
                var enabled = HasChild && !Enabled;

                // Fire press event
                if (OnPress != null)
                    OnPress(FullName);
                throw new CancelCheck(this, enabled);
            }

            if (!Enabled) return;

            foreach (var button in ChildButtons) {
                button.GetKeys();
            }
        }

        /// <summary>
        /// Enable or disable all the parents of this button.
        /// </summary>
        /// <param name="enabled">The value to apply.</param>
        private void SetEnableParents(bool enabled) {
            if (_parent == null || _parent.Name == "Root") return;
            _parent.Enabled = enabled;
            _parent.SetEnableParents(enabled);
        }
    }
}