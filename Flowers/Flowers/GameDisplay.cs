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
	public class GameDisplay : Display {
		#region Class variables
		private Flower[] flowers;
		private Line2D[] lines;
		private Player player;
		private Player computer;
		private Button replayButton;
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
			lineParams.Scale = new Vector2(5f, 5f);
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

			/********************** TOD: REMOVE ME ONCE I GET DYING SPRITES **********************/
			const string TEMP_DYING_NAME = "Flower1";

			// create our players
			float textY = (Game1.RESOLUTION.Y - 50f);
			this.player = new Player(content, ResourceManager.getInstance().Font, "Player", new Vector2(100f, textY), "Flower1", TEMP_DYING_NAME, LogicUtils.PLAYERS_TYPE);
			this.computer = new Player(content, ResourceManager.getInstance().Font, "Computer", new Vector2(Game1.RESOLUTION.X - 250f, textY), "flower2", TEMP_DYING_NAME, LogicUtils.COMPUTERS_TYPE);

			// Replay button
			int startX = 1000;
			ColouredButtonParams buttonParams = new ColouredButtonParams();
			buttonParams.Font = ResourceManager.getInstance().Font;
			buttonParams.LinesTexture = ResourceManager.getInstance().ButtonsLineTexture;
			buttonParams.Height = ResourceManager.BUTTONS_HEIGHT;
			buttonParams.Width = ResourceManager.BUTTONS_WIDTH;
			buttonParams.MouseOverColour = ResourceManager.getInstance().ButtonsMouseOverColour;
			buttonParams.RegularColour = ResourceManager.getInstance().ButtonsRegularColour;
			buttonParams.StartX = startX;
			buttonParams.StartY = 550;
			buttonParams.Text = "Replay";
			buttonParams.TextsPosition = new Vector2(startX + 35f, buttonParams.StartY + ResourceManager.BUTTONS_TEXT_Y_DIFFERENCE);
			this.replayButton = new ColouredButton(buttonParams);
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
				flower.reset();
			}
			if (fullReset) {
				this.computer.Score = 0;
				this.player.Score = 0;
				Random rand = new Random();
				int turn = rand.Next(2);
				if (turn == 0) {
					StateManager.getInstance().WhosTurnIsIt = StateManager.TurnType.Computers;
				} else {
					StateManager.getInstance().WhosTurnIsIt = StateManager.TurnType.Players;
				}
			}
			// if we just finished a game we need to re-activate the game
			if (StateManager.getInstance().CurrentState == StateManager.GameState.GameOver) {
				StateManager.getInstance().CurrentState = StateManager.GameState.Active;
			}
		}

		public override void update(float elapsed) {
			if (this.flowers != null) {
				foreach (Flower flower in this.flowers) {
					flower.update(elapsed);
				}
			}
			if (this.computer != null) {
				this.computer.update(elapsed);
			}
			if (this.player != null) {
				this.player.update(elapsed);
			}
			MouseState currentState = Mouse.GetState();
			// accept input to the tiles if the game is running
			if (StateManager.getInstance().CurrentState == StateManager.GameState.Active) {
				if (StateManager.getInstance().WhosTurnIsIt == StateManager.TurnType.Players) {
					if (currentState.LeftButton == ButtonState.Pressed && base.previousMouseState.LeftButton == ButtonState.Released) {// first press
						// find the tile we clicked
						Vector2 mousePos = new Vector2(currentState.X, currentState.Y);
						Flower flower = null;
						for (int i = 0; i < this.flowers.Length; i++) {
							flower = this.flowers[i];
							if (PickingUtils.pickRectangle(mousePos, SpritePositioner.getInstance().getPositionsRectangle(flower.Index))) {
								if (StateManager.getInstance().WhosTurnIsIt == StateManager.TurnType.Players) {
									flower.initSprites(this.player);
									StateManager.getInstance().WhosTurnIsIt = StateManager.TurnType.Computers;
								} /*else {// player two if we implement it
									flower.initSprites(Flower.FlowerType.Daisy, this.computersAliveTexture, this.computersDyingTexture);
									StateManager.getInstance().WhosTurnIsIt = StateManager.TurnType.Players;
								}*/
								break;
							}
						}
					}
				} else if (StateManager.getInstance().WhosTurnIsIt == StateManager.TurnType.Computers) {
					int move = StateManager.getInstance().ActiveDifficulty.getMove(this.flowers);
					this.flowers[move].initSprites(this.computer);
					StateManager.getInstance().WhosTurnIsIt = StateManager.TurnType.Players;
				}
				int[] winningIndexes;
				Flower.FlowerType winningType;
				if (LogicUtils.isGameOver(this.flowers, out winningType, out winningIndexes)) {
					StateManager.getInstance().CurrentState = StateManager.GameState.GameOver;
					if (winningType == LogicUtils.COMPUTERS_TYPE) {
						this.computer.Score++;
					} else if (winningType == LogicUtils.PLAYERS_TYPE) {
						this.player.Score++;
					}
				}
			} else if (StateManager.getInstance().CurrentState == StateManager.GameState.GameOver) {
				Vector2 mousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
				this.replayButton.processActorsMovement(mousePos);
				if (this.replayButton.isActorOver(mousePos)) {
					if (currentState.LeftButton == ButtonState.Pressed && base.previousMouseState.LeftButton == ButtonState.Released) {// first press
						reset(false);
					}
				}
			}
			// At any time if we press escape we need to go to the in game menu
			if (Keyboard.GetState().IsKeyDown(Keys.Escape) && base.previousKeyboardState.IsKeyUp(Keys.Escape)) {
				StateManager.getInstance().CurrentState = StateManager.GameState.InGameMenu;
			}
			// Did we just return from the In Game Menu? If so determine the games state
			if (StateManager.getInstance().CurrentState == StateManager.GameState.ReturnToGame) {
				Flower.FlowerType dummy;
				int[] dummyIndexes;
				if (LogicUtils.isGameOver(this.flowers, out dummy, out dummyIndexes)) {
					StateManager.getInstance().CurrentState = StateManager.GameState.GameOver;
				} else {
					StateManager.getInstance().CurrentState = StateManager.GameState.Active;
				}
			}
			base.update(elapsed);
		}

		public override void render(SpriteBatch spriteBatch) {
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
			if (this.computer != null) {
				this.computer.render(spriteBatch);
			}
			if (this.player != null) {
				this.player.render(spriteBatch);
			}
			if (this.replayButton != null && StateManager.getInstance().CurrentState == StateManager.GameState.GameOver) {
				this.replayButton.render(spriteBatch);
			}
		}
		#endregion Support methods

		#region Destructor
		public override void dispose() {
			if (this.computer != null) {
				this.computer.dispose();
			}
			if (this.player != null) {
				this.player.dispose();
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
			if (this.replayButton != null) {
				this.replayButton.dispose();
			}
		}
		#endregion Destructor
	}
}
