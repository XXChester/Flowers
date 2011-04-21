using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Engine.Model;
using Engine.Model.Params;
using Engine.Scripting;
namespace Flowers {
	public class InGameMenu : IRenderable {
		#region Class variables
		private Button[] buttons;
		#endregion Class variables

		#region Class propeties

		#endregion Class properties

		#region Constructor
		public InGameMenu() {
			this.buttons = new TexturedButton[2];
#if WINDOWS
#if DEBUG
			if (this.buttons != null) {
				for (int i = 0; i < this.buttons.Length; i++) {
					ScriptManager.getInstance().registerObject(this.buttons[i], "igmButton" + i);
				}
			}
#endif
#endif
		}
		#endregion Constructor

		#region Support methods
		public void update(float elapsed) {

		}

		public void render(SpriteBatch spriteBatch) {
			if (this.buttons != null) {
				foreach (Button button in this.buttons) {
					button.render(spriteBatch);
				}
			}
		}
		#endregion Support methods

		#region Destructor
		public void dispose() {
			if (this.buttons != null) {
				foreach (Button button in this.buttons) {
					button.dispose();
				}
			}
		}
		#endregion Destructor
	}
}
