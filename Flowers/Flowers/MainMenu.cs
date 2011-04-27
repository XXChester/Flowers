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
	public class MainMenu : Display {
		#region Class variables
		private Button[] buttons;
		private const int BUTTON_ID_EASY = 1;
		private const int BUTTON_ID_MODERATE = 2;
		private const int BUTTON_ID_HARD = 3;
		private const int BUTTON_ID_EXIT = 4;
		#endregion Class variables

		#region Class propeties

		#endregion Class properties

		#region Constructor
		public MainMenu(ContentManager content) {
			this.buttons = new ColouredButton[4];
			const float textXDiff = 50f;
			ColouredButtonParams buttonParams = new ColouredButtonParams();
			buttonParams.Font = ResourceManager.getInstance().Font;
			buttonParams.LinesTexture = ResourceManager.getInstance().ButtonsLineTexture;
			buttonParams.Height = ResourceManager.getInstance().ButtonsHeight;
			buttonParams.Width = 150;
			buttonParams.MouseOverColour = ResourceManager.getInstance().ButtonsMouseOverColour;
			buttonParams.RegularColour = ResourceManager.getInstance().ButtonsRegularColour;

			buttonParams.ID = BUTTON_ID_EASY;
			buttonParams.StartX = ResourceManager.getInstance().ButtonsStartX;
			buttonParams.StartY = 425;
			buttonParams.Text = "Easy";
			buttonParams.TextsPosition = new Vector2(ResourceManager.getInstance().ButtonsStartX + textXDiff, buttonParams.StartY + ResourceManager.getInstance().ButtonsTextYDifference);
			this.buttons[0] = new ColouredButton(buttonParams);

			buttonParams.ID = BUTTON_ID_MODERATE;
			buttonParams.StartX = ResourceManager.getInstance().ButtonsStartX;
			buttonParams.StartY = 500;
			buttonParams.Text = "Moderate";
			buttonParams.TextsPosition = new Vector2(ResourceManager.getInstance().ButtonsStartX + (textXDiff - 15f), buttonParams.StartY + ResourceManager.getInstance().ButtonsTextYDifference);
			this.buttons[1] = new ColouredButton(buttonParams);

			buttonParams.ID = BUTTON_ID_HARD;
			buttonParams.StartX = ResourceManager.getInstance().ButtonsStartX;
			buttonParams.StartY = 575;
			buttonParams.Text = "Hard";
			buttonParams.TextsPosition = new Vector2(ResourceManager.getInstance().ButtonsStartX + textXDiff, buttonParams.StartY + ResourceManager.getInstance().ButtonsTextYDifference);
			this.buttons[2] = new ColouredButton(buttonParams);

			buttonParams.ID = BUTTON_ID_EXIT;
			buttonParams.StartX = ResourceManager.getInstance().ButtonsStartX;
			buttonParams.StartY = 650;
			buttonParams.Text = "Exit";
			buttonParams.TextsPosition = new Vector2(ResourceManager.getInstance().ButtonsStartX + textXDiff, buttonParams.StartY + ResourceManager.getInstance().ButtonsTextYDifference);
			this.buttons[3] = new ColouredButton(buttonParams);
#if WINDOWS
#if DEBUG
			if (this.buttons != null) {
				for (int i = 0; i < this.buttons.Length; i++) {
					ScriptManager.getInstance().registerObject(this.buttons[i], "mmButton" + i);
				}
			}
#endif
#endif
		}
		#endregion Constructor

		#region Support methods
		public override void update(float elapsed) {
			if (this.buttons != null) {
				Vector2 mousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
				MouseState currentState = Mouse.GetState();
				foreach (Button button in this.buttons) {
					button.processActorsMovement(mousePos);
					if (button.isActorOver(mousePos)) {
						//was a button clicked
						if (currentState.LeftButton == ButtonState.Pressed && base.previousMouseState.LeftButton == ButtonState.Released) {
							if (BUTTON_ID_EASY == button.ID) {
								StateManager.getInstance().CurrentState = StateManager.GameState.InitEasyGame;
							} else if (BUTTON_ID_MODERATE == button.ID) {
								StateManager.getInstance().CurrentState = StateManager.GameState.InitModerateGame;
							} else if (BUTTON_ID_HARD == button.ID) {
								StateManager.getInstance().CurrentState = StateManager.GameState.InitHardGame;
							} else if (BUTTON_ID_EXIT == button.ID) {
								StateManager.getInstance().CurrentState = StateManager.GameState.ShutDown;
							}
						}
					}
				}
			}
			base.update(elapsed);
		}

		public override void render(SpriteBatch spriteBatch) {
			if (this.buttons != null) {
				foreach (Button button in this.buttons) {
					button.render(spriteBatch);
				}
			}
		}
		#endregion Support methods

		#region Destructor
		public override void dispose() {
			if (this.buttons != null) {
				foreach (Button button in this.buttons) {
					button.dispose();
				}
			}
		}
		#endregion Destructor
	}
}
