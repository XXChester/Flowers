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
using Engine.Utils;
namespace Flowers {
	public class GameDisplay : IRenderable {
		#region Class variables
		private SpriteFont font;
		private Flower[] flowers;
		private Line2D[] lines;
		private int playersScore;
		private int computersScore;
		private MouseState previousMouseState;
		private Texture2D playerOnesAliveTexture;
		private Texture2D playerOnesDyingTexture;
		private Texture2D playerTwosAliveTexture;
		private Texture2D playerTwosDyingTexture;
		#endregion Class variables

		#region Class propeties

		#endregion Class properties

		#region Constructor
		public GameDisplay(GraphicsDevice device, ContentManager content) {
			// create the boards lines
			Texture2D linesTexture = TextureUtils.create2DColouredTexture(device, 5, 5, Color.White);
			Line2DParams lineParams = new Line2DParams();
			lineParams.Texture = linesTexture;
			lineParams.LightColour = Color.Black;
			this.lines = new Line2D[8];
			lineParams.StartPosition = new Vector2(350f,525f);
			lineParams.EndPosition = new Vector2(900f, 525f);
			this.lines[0] = new Line2D(lineParams);
			lineParams.StartPosition = new Vector2(350f,612f);
			lineParams.EndPosition = new Vector2(900f, 612f);
			this.lines[1] = new Line2D(lineParams);
			lineParams.StartPosition = new Vector2(515f,450f);
			lineParams.EndPosition = new Vector2(515f, 700f);
			this.lines[2] = new Line2D(lineParams);
			lineParams.StartPosition = new Vector2(715f,450f);
			lineParams.EndPosition = new Vector2(715f, 700f);
			this.lines[3] = new Line2D(lineParams);
			lineParams.StartPosition = new Vector2(350f, 450f);
			lineParams.EndPosition = new Vector2(350f, 700f);
			this.lines[4] = new Line2D(lineParams);
			lineParams.StartPosition = new Vector2(900f, 450f);
			lineParams.EndPosition = new Vector2(900f, 700f);
			this.lines[5] = new Line2D(lineParams);
			lineParams.StartPosition = new Vector2(350f, 450f);
			lineParams.EndPosition = new Vector2(905f, 450f);
			this.lines[6] = new Line2D(lineParams);
			lineParams.StartPosition = new Vector2(350f, 700f);
			lineParams.EndPosition = new Vector2(900f, 700f);
			this.lines[7] = new Line2D(lineParams);

			// create the flowers
			this.flowers = new Flower[9];
			for (int i = 0; i < this.flowers.Length; i++) {
				this.flowers[i] = new Flower(content, i);
			}
			this.playerOnesAliveTexture = content.Load<Texture2D>("Flower1");
			this.playerTwosAliveTexture = content.Load<Texture2D>("Flower2");
#if WINDOWS
#if DEBUG
			if (this.lines != null) {
				for (int i = 0; i < this.lines.Length; i++) {
					ScriptManager.getInstance().registerObject(this.lines[i], "line" + i);
				}
			}
#endif
#endif
			reset(true);
		}
		#endregion Constructor

		#region Support methods
		public void reset(bool fullReset) {
			foreach (Flower flower in this.flowers) {
				flower.Type = Flower.FlowerType.None;
			}
			if (fullReset) {
				this.computersScore = 0;
				this.playersScore = 0;
			}
		}

		public void update(float elapsed) {
			if (this.flowers != null) {
				foreach (Flower flower in this.flowers) {
					flower.update(elapsed);
				}
			}
			MouseState currentState = Mouse.GetState();
			if (currentState.LeftButton == ButtonState.Pressed && this.previousMouseState.LeftButton == ButtonState.Released) {// first press
				// find the tile we clicked
				Vector2 mousePos = new Vector2(currentState.X, currentState.Y);
				Flower flower = null;
				for (int i = 0; i < this.flowers.Length; i++) {
					flower = this.flowers[i];
					if (PickingUtils.pickRectangle(mousePos, SpritePositioner.getInstance().getPositionsRectangle(flower.Index))) {
						//TODO: Need to tie into the state to see which texture we are going to use
						if (StateManager.getInstance().WhosTurnIsIt == StateManager.TurnType.PlayerOnes) {
							flower.initSprites(Flower.FlowerType.Rose, this.playerOnesAliveTexture, this.playerOnesDyingTexture);
							StateManager.getInstance().WhosTurnIsIt = StateManager.TurnType.PlayerTwos;
						} else {
							flower.initSprites(Flower.FlowerType.Daisy, this.playerTwosAliveTexture, this.playerTwosDyingTexture);
							StateManager.getInstance().WhosTurnIsIt = StateManager.TurnType.PlayerOnes;
						}
						break;
					}
				}
			}
			this.previousMouseState = Mouse.GetState();
		}

		public void render(SpriteBatch spriteBatch) {
			if (this.lines != null) {
				foreach (Line2D line in this.lines) {
					line.render(spriteBatch);
				}
			}
			if (this.flowers != null) {
				foreach (Flower flower in this.flowers) {
					flower.render(spriteBatch);
				}
			}
		}
		#endregion Support methods

		#region Destructor
		public void dispose() {
			if (this.playerOnesAliveTexture != null) {
				this.playerOnesAliveTexture.Dispose();
			}
			if (this.playerOnesDyingTexture != null) {
				this.playerOnesDyingTexture.Dispose();
			}
			if (this.playerTwosAliveTexture != null) {
				this.playerTwosAliveTexture.Dispose();
			}
			if (this.playerTwosDyingTexture != null) {
				this.playerTwosDyingTexture.Dispose();
			}
			if (this.lines != null) {
				foreach (Line2D line in this.lines) {
					line.dispose();
				}
			}
			if (this.flowers != null) {
				foreach (Flower flower in this.flowers) {
					flower.dispose();
				}
			}
		}
		#endregion Destructor
	}
}
