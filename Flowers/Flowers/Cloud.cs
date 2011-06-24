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
using GWNorthEngine.Model;
using GWNorthEngine.Model.Params;
using GWNorthEngine.Scripting;
using GWNorthEngine.Utils;
namespace Flowers {
	public class Cloud : IRenderable {
		#region Class variables
		private StaticDrawable2D drawable;
		private const float SPEED = 10f / 1000f;// float a second
		private const float RESET_X = 1400f;
		#endregion Class variables

		#region Class propeties

		#endregion Class properties

		#region Constructor
		public Cloud(Vector2 position, Texture2D texture) 
		: this(position, texture, SpriteEffects.None) {
		}

		public Cloud(Vector2 position, Texture2D texture, SpriteEffects spriteEffect) {
			StaticDrawable2DParams parms = new StaticDrawable2DParams();
			parms.Texture = texture;
			parms.Position = position;
			parms.SpriteEffect = spriteEffect;
			this.drawable = new StaticDrawable2D(parms);
		}
		#endregion Constructor

		#region Support methods
		public void update(float elapsed) {
			if (this.drawable != null) {
				Vector2 newPosition = new Vector2(this.drawable.Position.X - (SPEED * elapsed), this.drawable.Position.Y);
				if (newPosition.X < -100f) {
					newPosition.X = RESET_X;
				}
				this.drawable.Position = newPosition;
			}
		}

		public void render(SpriteBatch spriteBatch) {
			if (this.drawable != null) {
				this.drawable.render(spriteBatch);
			}
		}
		#endregion Support methods

		#region Destructor
		public void dispose() {
			if (this.drawable != null) {
				this.drawable.dispose();
			}
		}
		#endregion Destructor
	}
}
