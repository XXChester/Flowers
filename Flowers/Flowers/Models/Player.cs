using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GWNorthEngine.Model;
using GWNorthEngine.Model.Params;
using GWNorthEngine.Logic;
using GWNorthEngine.Logic.Params;
using GWNorthEngine.Utils;
namespace Flowers {
	public class Player : IRenderable {
		#region Class variables
		private string name;
		private Animated2DSprite activeTurnSprite;
		private Animated2DSprite inactiveTurnSprite;
		#endregion Class variables

		#region Class properties
		public Text2D Text { get; set; }
		public Texture2D AliveTexture { get; set; }
		public Texture2D DyingTexture { get; set; }
		public Flower.FlowerType FlowerType { get; set; }
		public int Score { get; set; }
		#endregion Class properties

		#region Constructor
		public Player(ContentManager content, SpriteFont font, string name, Vector2 scorePosition, Vector2 turnSpritePosition, string aliveTexture, string dyingTexture, 
			Flower.FlowerType flowerType) {
			this.name = name;
			this.AliveTexture = LoadingUtils.load<Texture2D>(content, aliveTexture);
			this.DyingTexture = LoadingUtils.load<Texture2D>(content, dyingTexture);
			this.FlowerType = flowerType;
			this.Score = 0;
			Text2DParams textParams = new Text2DParams();
			textParams.Font = font;
			textParams.LightColour = ResourceManager.getInstance().TextColour;
			textParams.Position = scorePosition;
			this.Text = new Text2D(textParams);

			this.activeTurnSprite = FlowerBuilder.getFlowerSprite(content, turnSpritePosition, this.AliveTexture, AnimationState.PlayForwardOnce);
			this.inactiveTurnSprite = FlowerBuilder.getFlowerSprite(content, turnSpritePosition, this.DyingTexture, AnimationState.PlayForwardOnce);
		}
		#endregion Constructor

		#region Support methods
		public void updateColour(float transitionTime) {
			if (StateManager.getInstance().CurrentTransitionState == StateManager.TransitionState.TransitionIn) {
				this.Text.LightColour = TransitionUtils.fadeIn(ResourceManager.getInstance().TextColour, Display.TRANSITION_TIME, transitionTime);
				this.activeTurnSprite.LightColour = TransitionUtils.fadeIn(Color.White, Display.TRANSITION_TIME, transitionTime);
				this.inactiveTurnSprite.LightColour = TransitionUtils.fadeIn(Color.White, Display.TRANSITION_TIME, transitionTime);
			} else if (StateManager.getInstance().CurrentTransitionState == StateManager.TransitionState.TransitionOut) {
				this.Text.LightColour = TransitionUtils.fadeOut(ResourceManager.getInstance().TextColour, Display.TRANSITION_TIME, transitionTime);
				this.activeTurnSprite.LightColour = TransitionUtils.fadeOut(Color.White, Display.TRANSITION_TIME, transitionTime);
				this.inactiveTurnSprite.LightColour = TransitionUtils.fadeOut(Color.White, Display.TRANSITION_TIME, transitionTime);
			}
		}

		public void update(float elapsed) {
			this.Text.WrittenText = name + ": " + this.Score;
			if (LogicUtils.translateTurnToFlowerType(StateManager.getInstance().WhosTurnIsIt) == this.FlowerType) {
				this.activeTurnSprite.update(elapsed);
				this.inactiveTurnSprite.reset();
				this.inactiveTurnSprite.AnimationManager.State = AnimationState.PlayForwardOnce;
			} else {
				this.inactiveTurnSprite.update(elapsed);
				this.activeTurnSprite.reset();
				this.activeTurnSprite.AnimationManager.State = AnimationState.PlayForwardOnce;
			}
		}

		public void render(SpriteBatch spriteBatch) {
			this.Text.render(spriteBatch);
			if (LogicUtils.translateTurnToFlowerType(StateManager.getInstance().WhosTurnIsIt) == this.FlowerType) {
				this.activeTurnSprite.render(spriteBatch);
			} else {
				this.inactiveTurnSprite.render(spriteBatch);
			}
		}
		#endregion Support methods

		#region Destructor
		public void dispose() {
			if (this.activeTurnSprite != null) {
				this.activeTurnSprite.dispose();
			}
			if (this.inactiveTurnSprite != null) {
				this.inactiveTurnSprite.dispose();
			}
			if (this.AliveTexture != null) {
				this.AliveTexture.Dispose();
			}
			if (this.DyingTexture != null) {
				this.DyingTexture.Dispose();
			}
		}
		#endregion Destructor
	}
}
