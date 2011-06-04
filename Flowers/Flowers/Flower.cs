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
	public class Flower : IRenderable {
		public enum FlowerType {
			None,
			Rose = -1,
			Daisy = 1
		}
		#region Class variables
		private Animated2DSprite aliveSprite;
		private Animated2DSprite dyingSprite;
		private Animated2DSprite activeSprite;
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
			BaseAnimationManagerParams animationParams = new BaseAnimationManagerParams();
			animationParams.FrameRate = 125f;
			animationParams.TotalFrameCount = 5;
			Animated2DSpriteParams spriteParams = new Animated2DSpriteParams();
			spriteParams.Content = content;
			spriteParams.FramesHeight = 96;
			spriteParams.FramesStartHeight = 0;
			spriteParams.FramesStartWidth = 0;
			spriteParams.FramesWidth = 96;
			spriteParams.LoadingType = Animated2DSprite.LoadingType.CustomizedSheetDefineFrames;
			spriteParams.Position = SpritePositioner.getInstance().getPosition(index);
			spriteParams.SpaceBetweenFrames = 0;
			spriteParams.Origin = new Vector2(40f,80f);
			spriteParams.AnimationParams = animationParams;
			this.aliveSprite = new Animated2DSprite(spriteParams);
			this.dyingSprite = new Animated2DSprite(spriteParams);
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
			this.activeSprite = this.aliveSprite;
			this.dyingSprite.Texture = player.DyingTexture;
		}

		public void reset() {
			this.type = FlowerType.None;
			this.aliveSprite.reset();
			this.dyingSprite.reset();
		}

		public void updateColour(float transitionTime) {
			if (this.activeSprite != null) {
				if (StateManager.getInstance().CurrentTransitionState == StateManager.TransitionState.TransitionIn) {
					this.activeSprite.LightColour = TransitionUtils.fadeIn(Color.White, Display.TRANSITION_TIME, transitionTime);
				} else if (StateManager.getInstance().CurrentTransitionState == StateManager.TransitionState.TransitionOut) {
					this.activeSprite.LightColour = TransitionUtils.fadeOut(Color.White, Display.TRANSITION_TIME, transitionTime);
				}
			}
		}

		public void update(float elapsed) {
			if (this.type != FlowerType.None) {
				if (StateManager.getInstance().CurrentState == StateManager.GameState.InitGameOver) {
					if (StateManager.getInstance().Winner.winningType != this.type || !StateManager.getInstance().Winner.winningIndexes.Contains(this.index)) {
						this.dyingSprite.AnimationManager.State = AnimationManager.AnimationState.PlayForwardOnce;
						this.dyingSprite.reset();
						this.activeSprite = this.dyingSprite;
					}
				}

				this.activeSprite.update(elapsed);
			}
		}

		public void render(SpriteBatch spriteBatch) {
			if (this.type != FlowerType.None) {
				this.activeSprite.render(spriteBatch);
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
			if (this.activeSprite != null) {
				this.activeSprite.dispose();
			}
		}
		#endregion Destructor
	}
}
