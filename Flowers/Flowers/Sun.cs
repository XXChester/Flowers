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
namespace Flowers {
	public class Sun : IRenderable {
		#region Class variables
		private StaticDrawable2D innerLayer;
		private StaticDrawable2D outterLayer;
		private const float INNER_ROTATION_SPEED = .1f / 1000f;
		private const float OUTER_ROTATION_SPEED = .2f / 1000f;
		#endregion Class variables

		#region Class propeties

		#endregion Class properties

		#region Constructor
		public Sun(ContentManager content) {
			StaticDrawable2DParams parms = new StaticDrawable2DParams();
			Texture2D sunTx = content.Load<Texture2D>("Sun");
			parms.Texture = sunTx;
			parms.Scale = new Vector2(1.5f, 1.5f);
			parms.Origin = new Vector2(48f, 48f);
			parms.Position = new Vector2(70f, 70f);
			this.innerLayer = new StaticDrawable2D(parms);
			parms.Position = new Vector2(70f, 70f);
			parms.Origin = new Vector2(50f, 50f);
			parms.Scale = new Vector2(1.8f, 1.8f);
			parms.SpriteEffect = SpriteEffects.FlipHorizontally;
			parms.LightColour = Color.LightGray;
			this.outterLayer = new StaticDrawable2D(parms);
#if WINDOWS
#if DEBUG
			if (this.innerLayer != null) {
				ScriptManager.getInstance().registerObject(this.innerLayer, "sunInner");
			}
			if (this.outterLayer != null) {
				ScriptManager.getInstance().registerObject(this.outterLayer, "sunOutter");
			}
#endif
#endif
		}
		#endregion Constructor

		#region Support methods
		public void update(float elapsed) {
			if (this.innerLayer != null) {
				this.innerLayer.Rotation = this.innerLayer.Rotation + (INNER_ROTATION_SPEED * elapsed);
			}
			if (this.outterLayer != null) {
				this.outterLayer.Rotation = this.outterLayer.Rotation - (OUTER_ROTATION_SPEED * elapsed);
			}
		}

		public void render(SpriteBatch spriteBatch) {
			if (this.outterLayer != null) {
				this.outterLayer.render(spriteBatch);
			}
			if (this.innerLayer != null) {
				this.innerLayer.render(spriteBatch);
			}
		}
		#endregion Support methods

		#region Destructor
		public void dispose() {
			if (this.innerLayer != null) {
				this.innerLayer.dispose();
			}
			if (this.outterLayer != null) {
				this.outterLayer.dispose();
			}
		}
		#endregion Destructor
	}
}
