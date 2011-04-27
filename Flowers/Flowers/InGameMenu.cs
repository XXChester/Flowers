﻿using System;
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
	public class InGameMenu : Display {
		#region Class variables
		private Button[] buttons;
		private const int BUTTON_ID_RETURN_TO_GAME = 1;
		private const int BUTTON_ID_EXIT_TO_MAIN_MENU = 2;
		#endregion Class variables

		#region Class propeties

		#endregion Class properties

		#region Constructor
		public InGameMenu() {
			this.buttons = new ColouredButton[2];
			int startX = ResourceManager.BUTTONS_START_X - 25;
			ColouredButtonParams buttonParams = new ColouredButtonParams();
			buttonParams.Font = ResourceManager.getInstance().Font;
			buttonParams.LinesTexture = ResourceManager.getInstance().ButtonsLineTexture;
			buttonParams.Height = ResourceManager.BUTTONS_HEIGHT;
			buttonParams.Width = ResourceManager.BUTTONS_WIDTH + 75;// These buttons are a little wider
			buttonParams.MouseOverColour = ResourceManager.getInstance().ButtonsMouseOverColour;
			buttonParams.RegularColour = ResourceManager.getInstance().ButtonsRegularColour;

			buttonParams.ID = BUTTON_ID_RETURN_TO_GAME;
			buttonParams.StartX = startX;
			buttonParams.StartY = 500;
			buttonParams.Text = "Return to Game";
			buttonParams.TextsPosition = new Vector2(startX + 35f, buttonParams.StartY + ResourceManager.BUTTONS_TEXT_Y_DIFFERENCE);
			this.buttons[0] = new ColouredButton(buttonParams);

			buttonParams.ID = BUTTON_ID_EXIT_TO_MAIN_MENU;
			buttonParams.StartX = startX;
			buttonParams.StartY = 575;
			buttonParams.Text = "Exit to Main Menu";
			buttonParams.TextsPosition = new Vector2(startX + 20f, buttonParams.StartY + ResourceManager.BUTTONS_TEXT_Y_DIFFERENCE);
			this.buttons[1] = new ColouredButton(buttonParams);
#if WINDOWS
#if DEBUG
			if (this.buttons != null) {
				for (int i = 0; i < this.buttons.Length; i++) {
					ScriptManager.getInstance().registerObject(this.buttons[i], "igmButton" + i);
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
							if (BUTTON_ID_EXIT_TO_MAIN_MENU == button.ID) {
								StateManager.getInstance().CurrentState = StateManager.GameState.ReturnToMainMenu;
							} else if (BUTTON_ID_RETURN_TO_GAME == button.ID) {
								StateManager.getInstance().CurrentState = StateManager.GameState.ReturnToGame;
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
