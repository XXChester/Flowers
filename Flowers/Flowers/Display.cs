using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
namespace Flowers {
	public abstract class Display {
		#region Class variables
		protected MouseState previousMouseState;
		protected KeyboardState previousKeyboardState;
		#endregion Class variables

		#region Support methods
		public virtual void update(float elapsed) {
			this.previousKeyboardState = Keyboard.GetState();
			this.previousMouseState = Mouse.GetState();
		}

		public abstract void render(SpriteBatch spriteBatch);
		public abstract void dispose();
		#endregion Support methods
	}
}
