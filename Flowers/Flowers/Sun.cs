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
		private Animated2DSprite innerLayer;
		private Animated2DSprite outterLayer;
		#endregion Class variables

		#region Class propeties

		#endregion Class properties

		#region Constructor
		public Sun(ContentManager content) {
			Animated2DSpriteParams parms = new Animated2DSpriteParams();
			parms.AnimationState = AnimationManager.AnimationState.Paused;
			parms.Content = content;
			parms.FrameRate = 200f;
			parms.LoadingType = Animated2DSprite.LoadingType.WholeSheetReadFramesFromFile;
			parms.TexturesName = "Sun";
			parms.TotalFrameCount = 1;
			parms.Position = new Vector2(20f, 20f);
			this.innerLayer = new Animated2DSprite(parms);
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
				this.innerLayer.update(elapsed);
			}
			if (this.outterLayer != null) {
				this.outterLayer.update(elapsed);
			}
		}

		public void render(SpriteBatch spriteBatch) {
			if (this.innerLayer != null) {
				this.innerLayer.render(spriteBatch);
			}
			if (this.outterLayer != null) {
				this.outterLayer.render(spriteBatch);
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
