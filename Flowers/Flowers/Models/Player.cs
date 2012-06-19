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
		private Animated2DSprite myTurnSprite;
		private Animated2DSprite notMyTurnSprite;
		private Animated2DSprite activeSprite;
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

			this.myTurnSprite = FlowerBuilder.getFlowerSprite(content, turnSpritePosition, this.AliveTexture, AnimationState.PlayForwardOnce);
			this.notMyTurnSprite = FlowerBuilder.getFlowerSprite(content, turnSpritePosition, this.DyingTexture, AnimationState.PlayForwardOnce);
		}
		#endregion Constructor

		#region Support methods
		public void updateColour(float transitionTime) {
			if (StateManager.getInstance().CurrentTransitionState == StateManager.TransitionState.TransitionIn) {
				this.Text.LightColour = TransitionUtils.fadeIn(ResourceManager.getInstance().TextColour, Display.TRANSITION_TIME, transitionTime);
				this.myTurnSprite.LightColour = TransitionUtils.fadeIn(Color.White, Display.TRANSITION_TIME, transitionTime);
				this.notMyTurnSprite.LightColour = TransitionUtils.fadeIn(Color.White, Display.TRANSITION_TIME, transitionTime);
			} else if (StateManager.getInstance().CurrentTransitionState == StateManager.TransitionState.TransitionOut) {
				this.Text.LightColour = TransitionUtils.fadeOut(ResourceManager.getInstance().TextColour, Display.TRANSITION_TIME, transitionTime);
				this.myTurnSprite.LightColour = TransitionUtils.fadeOut(Color.White, Display.TRANSITION_TIME, transitionTime);
				this.notMyTurnSprite.LightColour = TransitionUtils.fadeOut(Color.White, Display.TRANSITION_TIME, transitionTime);
			}
		}

		public void update(float elapsed) {
			this.Text.WrittenText = name + ": " + this.Score;

			if (LogicUtils.translateTurnToFlowerType(StateManager.getInstance().WhosTurnIsIt) == this.FlowerType) {
				this.activeSprite = this.myTurnSprite;
				this.myTurnSprite.update(elapsed);
				this.notMyTurnSprite.reset();
				this.notMyTurnSprite.AnimationManager.State = AnimationState.PlayForwardOnce;
			} else {
				this.activeSprite = this.notMyTurnSprite;
				this.notMyTurnSprite.update(elapsed);
				this.myTurnSprite.reset();
				this.myTurnSprite.AnimationManager.State = AnimationState.PlayForwardOnce;
			}
		}

		public void render(SpriteBatch spriteBatch) {
			this.Text.render(spriteBatch);
			this.activeSprite.render(spriteBatch);
		}
		#endregion Support methods

		#region Destructor
		public void dispose() {
			if (this.myTurnSprite != null) {
				this.myTurnSprite.dispose();
			}
			if (this.notMyTurnSprite != null) {
				this.notMyTurnSprite.dispose();
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
