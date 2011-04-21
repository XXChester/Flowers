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
	public class Flower : IRenderable {
		public enum FlowerType {
			None,
			Rose,
			Daisy
		}
		#region Class variables
		private Animated2DSprite activeSprite;
		private Animated2DSprite dyingSprite;
		private FlowerType type;
		private int index;
		#endregion Class variables

		#region Class propeties

		#endregion Class properties

		#region Constructor
		public Flower(ContentManager content, int index, Vector2 position) {
			this.type = FlowerType.None;
			this.index = index;
			Animated2DSpriteParams parms = new Animated2DSpriteParams();
			parms.AnimationState = AnimationManager.AnimationState.PlayForwardOnce;
			parms.Content = content;
			parms.FrameRate = 150f;
			parms.FramesHeight = 96;
			parms.FramesStartHeight = 0;
			parms.FramesStartWidth = 0;
			parms.FramesWidth = 96;
			parms.LoadingType = Animated2DSprite.LoadingType.CustomizedSheetDefineFrames;
			parms.Position = position;
			parms.SpaceBetweenFrames = 0;
			parms.TotalFrameCount = 5;
			parms.Origin = new Vector2(40f,80f);
			this.activeSprite = new Animated2DSprite(parms);
#if WINDOWS
#if DEBUG
			
#endif
#endif
		}
		#endregion Constructor

		#region Support methods
		public void update(float elapsed) {
			if (this.type != FlowerType.None) {
				// tie into state manager to determine what one to update
				this.activeSprite.update(elapsed);
			}
		}

		public void render(SpriteBatch spriteBatch) {
			if (this.type != FlowerType.None) {
				// tie into state manager to determine what one to draw
				this.activeSprite.render(spriteBatch);
			}
		}
		#endregion Support methods

		#region Destructor
		public void dispose() {
			if (this.activeSprite != null) {
				this.activeSprite.dispose();
			}
			if (this.dyingSprite != null) {
				this.dyingSprite.dispose();
			}
		}
		#endregion Destructor
	}
}
