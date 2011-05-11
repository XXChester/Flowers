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
		private Player player;
		private Player computer;
		private Button replayButton;
		private float currentDelay;
		private const float DELAY = 750f;//Time the conmputer takes to place a move
		#endregion Class variables

		#region Class propeties

		#endregion Class properties

		#region Constructor
		public GameDisplay(ContentManager content) {
			// create the flowers
			this.flowers = new Flower[9];
			for (int i = 0; i < this.flowers.Length; i++) {
				this.flowers[i] = new Flower(content, i);
			}

			// create our players
			float textY = (Game1.RESOLUTION.Y - 50f);
			this.player = new Player(content, ResourceManager.getInstance().Font, "Player", new Vector2(100f, textY), "DaisyAlive", "DaisyDying", LogicUtils.PLAYERS_TYPE);
			this.computer = new Player(content, ResourceManager.getInstance().Font, "Computer", new Vector2(Game1.RESOLUTION.X - 250f, textY), "RoseAlive", "RoseDying", LogicUtils.COMPUTERS_TYPE);

			// Replay button
			int startX = 1000;
			ColouredButtonParams buttonParams = new ColouredButtonParams();
			buttonParams.Font = ResourceManager.getInstance().Font;
			buttonParams.LinesTexture = ResourceManager.getInstance().ButtonsLineTexture;
			buttonParams.Height = ResourceManager.BUTTONS_HEIGHT;
			buttonParams.Width = ResourceManager.BUTTONS_WIDTH;
			buttonParams.MouseOverColour = ResourceManager.getInstance().ButtonsMouseOverColour;
			buttonParams.RegularColour = ResourceManager.getInstance().TextColour;
			buttonParams.StartX = startX;
			buttonParams.StartY = 550;
			buttonParams.Text = "Replay";
			buttonParams.TextsPosition = new Vector2(startX + 35f, buttonParams.StartY + ResourceManager.BUTTONS_TEXT_Y_DIFFERENCE);
			this.replayButton = new ColouredButton(buttonParams);
#if WINDOWS
#if DEBUG
			
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
					flower.updateColour(base.currentTransitionTime);
				}
				if (StateManager.getInstance().CurrentState == StateManager.GameState.InitGameOver) {
					StateManager.getInstance().CurrentState = StateManager.GameState.GameOver;
				}
			}

			if (this.computer != null) {
				this.computer.update(elapsed);
				this.computer.updateColour(base.currentTransitionTime);
			}
			if (this.player != null) {
				this.player.update(elapsed);
				this.player.updateColour(base.currentTransitionTime);
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
							if (flower.Type == Flower.FlowerType.None) {
								if (PickingUtils.pickRectangle(mousePos, SpritePositioner.getInstance().getPositionsRectangle(flower.Index))) {
									if (StateManager.getInstance().WhosTurnIsIt == StateManager.TurnType.Players) {
										flower.initSprites(this.player);
										StateManager.getInstance().WhosTurnIsIt = StateManager.TurnType.Computers;
										this.currentDelay = 0f;
									} /*else {// player two if we implement it
									flower.initSprites(Flower.FlowerType.Daisy, this.computersAliveTexture, this.computersDyingTexture);
									StateManager.getInstance().WhosTurnIsIt = StateManager.TurnType.Players;
								}*/
									break;
								}
							}
						}
					}
				} else if (StateManager.getInstance().WhosTurnIsIt == StateManager.TurnType.Computers) {
					if (this.currentDelay >= DELAY) {
						int move = StateManager.getInstance().ActiveDifficulty.getMove(this.flowers);
						this.flowers[move].initSprites(this.computer);
						StateManager.getInstance().WhosTurnIsIt = StateManager.TurnType.Players;
					}
				}
				Winner winner;
				if (LogicUtils.isGameOver(this.flowers, out winner)) {
					StateManager.getInstance().CurrentState = StateManager.GameState.InitGameOver;
					StateManager.getInstance().Winner = winner;
					if (winner.winningType == LogicUtils.COMPUTERS_TYPE) {
						this.computer.Score++;
					} else if (winner.winningType == LogicUtils.PLAYERS_TYPE) {
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

			if (StateManager.getInstance().CurrentTransitionState != StateManager.TransitionState.None) {
				Vector2 mousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
				if (StateManager.getInstance().CurrentTransitionState == StateManager.TransitionState.TransitionIn) {
					if (this.replayButton.isActorOver(mousePos)) {
						((ColouredButton)this.replayButton).updateColours(base.fadeIn(ResourceManager.getInstance().ButtonsMouseOverColour));
					} else {
						((ColouredButton)this.replayButton).updateColours(base.fadeIn(ResourceManager.getInstance().TextColour));
					}
				} else if (StateManager.getInstance().CurrentTransitionState == StateManager.TransitionState.TransitionOut) {
					if (this.replayButton.isActorOver(mousePos)) {
						((ColouredButton)this.replayButton).updateColours(base.fadeOut(ResourceManager.getInstance().ButtonsMouseOverColour));
					} else {
						((ColouredButton)this.replayButton).updateColours(base.fadeOut(ResourceManager.getInstance().TextColour));
					}
				}

				// if the fade ins/outs are complete we change the state
				if (base.transitionTimeElapsed()) {
					if (StateManager.getInstance().CurrentTransitionState == StateManager.TransitionState.TransitionIn) {
						StateManager.getInstance().CurrentTransitionState = StateManager.TransitionState.None;
					} else if (StateManager.getInstance().CurrentTransitionState == StateManager.TransitionState.TransitionOut) {
						// we need to transition in our in game menu screen
						StateManager.getInstance().CurrentTransitionState = StateManager.TransitionState.TransitionIn;
					}
				}
			}
			// At any time if we press escape we need to go to the in game menu
			if (Keyboard.GetState().IsKeyDown(Keys.Escape) && base.previousKeyboardState.IsKeyUp(Keys.Escape)) {
				StateManager.getInstance().CurrentState = StateManager.GameState.InGameMenu;
				StateManager.getInstance().CurrentTransitionState = StateManager.TransitionState.TransitionOut;
			}
			this.currentDelay += elapsed;
			base.update(elapsed);
		}

		public override void render(SpriteBatch spriteBatch) {
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
			if (this.replayButton != null && StateManager.getInstance().CurrentState == StateManager.GameState.GameOver ||
				(StateManager.getInstance().PreviousState == StateManager.GameState.GameOver && 
				StateManager.getInstance().CurrentTransitionState == StateManager.TransitionState.TransitionOut)) {
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
