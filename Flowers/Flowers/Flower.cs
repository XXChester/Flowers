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
		private Animated2DSprite aliveSprite;
		private Animated2DSprite dyingSprite;
		private FlowerType type;
		private int index;
		#endregion Class variables

		#region Class propeties
		public FlowerType Type { get { return this.type; } set { this.type = value; } }
		public int Index { get { return this.index; } }
		#endregion Class properties

		#region Constructor
		public Flower(ContentManager content, int index) {
			this.index = index;
			Animated2DSpriteParams parms = new Animated2DSpriteParams();
			parms.AnimationState = AnimationManager.AnimationState.Paused;
			parms.Content = content;
			parms.FrameRate = 150f;
			parms.FramesHeight = 96;
			parms.FramesStartHeight = 0;
			parms.FramesStartWidth = 0;
			parms.FramesWidth = 96;
			parms.LoadingType = Animated2DSprite.LoadingType.CustomizedSheetDefineFrames;
			parms.Position = SpritePositioner.getInstance().getPosition(index);
			parms.SpaceBetweenFrames = 0;
			parms.TotalFrameCount = 5;
			parms.Origin = new Vector2(40f,80f);
			this.aliveSprite = new Animated2DSprite(parms);
			//TOD: NEED TO UPDATE THIS LATER
			this.dyingSprite = new Animated2DSprite(parms);
			reset();
#if WINDOWS
#if DEBUG

#endif
#endif
		}
		#endregion Constructor

		#region Support methods
		public void initSprites(Player player) {
			this.type = player.FlowerType;
			this.aliveSprite.Texture = player.AliveTexture;
			this.aliveSprite.AnimationManager.State = AnimationManager.AnimationState.PlayForwardOnce;
			this.dyingSprite.Texture = player.DyingTexture;
		}

		public void reset() {
			this.type = FlowerType.None;
			this.aliveSprite.reset();
			this.dyingSprite.reset();
		}

		public void update(float elapsed) {
			if (this.type != FlowerType.None) {
				// tie into state manager to determine what one to update
				this.aliveSprite.update(elapsed);
			}
		}

		public void render(SpriteBatch spriteBatch) {
			if (this.type != FlowerType.None) {
				// tie into state manager to determine what one to draw
				this.aliveSprite.render(spriteBatch);
			}
		}
		#endregion Support methods

		#region Destructor
		public void dispose() {
			if (this.aliveSprite != null) {
				this.aliveSprite.dispose();
			}
			if (this.dyingSprite != null) {
				this.dyingSprite.dispose();
			}
		}
		#endregion Destructor
	}
}
