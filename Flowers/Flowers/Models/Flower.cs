using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GWNorthEngine.Model;
using GWNorthEngine.Logic;
using GWNorthEngine.Logic.Params;
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
			this.aliveSprite = FlowerBuilder.getFlowerSprite(content, index);
			this.dyingSprite = FlowerBuilder.getFlowerSprite(content, index);
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
			this.aliveSprite.AnimationManager.State = AnimationState.PlayForwardOnce;
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
						this.dyingSprite.AnimationManager.State = AnimationState.PlayForwardOnce;
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
