using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
namespace Flowers {
	public abstract class Display {
		#region Class variables
		protected MouseState previousMouseState;
		protected KeyboardState previousKeyboardState;
		protected float currentTransitionTime;
		public const float TRANSITION_TIME = 400f;
		#endregion Class variables

		#region Support methods
		protected Color fadeOut(Color colour) {
			return fadeOut(colour, this.currentTransitionTime);
		}

		public static Color fadeOut(Color colour, float elapsedTransitionTime) {
			float alpha = 1f - (elapsedTransitionTime / Display.TRANSITION_TIME);
			return Color.Lerp(Color.Transparent, colour, alpha);
		}

		protected Color fadeIn(Color colour) {
			return fadeIn(colour, this.currentTransitionTime);
		}

		public static Color fadeIn(Color colour, float elapsedTransitionTime) {
			float alpha = 1f - (elapsedTransitionTime / Display.TRANSITION_TIME);
			return Color.Lerp(colour, Color.Transparent, alpha);
		}

		public static bool resetTransitionTimes() {
			bool reset = false;
			// reset our current transition time in certain scenarios
			if (StateManager.getInstance().CurrentTransitionState == StateManager.TransitionState.None) {
				reset = true;
			} else if (StateManager.getInstance().CurrentTransitionState == StateManager.TransitionState.TransitionIn &&
				StateManager.getInstance().PreviousTransitionState == StateManager.TransitionState.TransitionOut) {
				// if we went straight from a transition out to transition in we need to reset our transition timer as well
				reset = true;
			}
			return reset;
		}

		public bool transitionTimeElapsed() {
			bool elapsed = false;
			if (this.currentTransitionTime >= Display.TRANSITION_TIME) {
				elapsed = true;
			}
			return elapsed;
		}

		public virtual void update(float elapsed) {
			this.previousKeyboardState = Keyboard.GetState();
			this.previousMouseState = Mouse.GetState();

			if (resetTransitionTimes()) {
				this.currentTransitionTime = 0f;
			} else {
				this.currentTransitionTime += elapsed;
			}
			StateManager.getInstance().PreviousTransitionState = StateManager.getInstance().CurrentTransitionState;
		}

		public abstract void render(SpriteBatch spriteBatch);
		public abstract void dispose();
		#endregion Support methods
	}
}
