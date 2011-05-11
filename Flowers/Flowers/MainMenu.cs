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
	public class MainMenu : Display {
		#region Class variables
		private Button[] buttons;
		private const int BUTTON_ID_EASY = 1;
		private const int BUTTON_ID_MODERATE = 2;
		private const int BUTTON_ID_IMPOSSIBLE = 3;
		private const int BUTTON_ID_EXIT = 4;
		#endregion Class variables

		#region Class propeties

		#endregion Class properties

		#region Constructor
		public MainMenu(ContentManager content) {
			this.buttons = new ColouredButton[4];
			const float textXDiff = 50f;
			int startX = 1000;
			ColouredButtonParams buttonParams = new ColouredButtonParams();
			buttonParams.Font = ResourceManager.getInstance().Font;
			buttonParams.LinesTexture = ResourceManager.getInstance().ButtonsLineTexture;
			buttonParams.Height = ResourceManager.BUTTONS_HEIGHT;
			buttonParams.Width = ResourceManager.BUTTONS_WIDTH;
			buttonParams.MouseOverColour = ResourceManager.getInstance().ButtonsMouseOverColour;
			buttonParams.RegularColour = ResourceManager.getInstance().TextColour;

			buttonParams.ID = BUTTON_ID_EASY;
			buttonParams.StartX = startX;
			buttonParams.StartY = 425;
			buttonParams.Text = "Easy";
			buttonParams.TextsPosition = new Vector2(startX + textXDiff, buttonParams.StartY + ResourceManager.BUTTONS_TEXT_Y_DIFFERENCE);
			this.buttons[0] = new ColouredButton(buttonParams);

			buttonParams.ID = BUTTON_ID_MODERATE;
			buttonParams.StartX = startX;
			buttonParams.StartY = 500;
			buttonParams.Text = "Moderate";
			buttonParams.TextsPosition = new Vector2(startX + (textXDiff - 15f), buttonParams.StartY + ResourceManager.BUTTONS_TEXT_Y_DIFFERENCE);
			this.buttons[1] = new ColouredButton(buttonParams);

			buttonParams.ID = BUTTON_ID_IMPOSSIBLE;
			buttonParams.StartX = startX;
			buttonParams.StartY = 575;
			buttonParams.Text = "Impossible";
			buttonParams.TextsPosition = new Vector2(startX + (textXDiff - 30f), buttonParams.StartY + ResourceManager.BUTTONS_TEXT_Y_DIFFERENCE);
			this.buttons[2] = new ColouredButton(buttonParams);

			buttonParams.ID = BUTTON_ID_EXIT;
			buttonParams.StartX = startX;
			buttonParams.StartY = 650;
			buttonParams.Text = "Exit";
			buttonParams.TextsPosition = new Vector2(startX + textXDiff, buttonParams.StartY + ResourceManager.BUTTONS_TEXT_Y_DIFFERENCE);
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
					if (StateManager.getInstance().CurrentTransitionState == StateManager.TransitionState.None) {
						button.processActorsMovement(mousePos);
						if (button.isActorOver(mousePos)) {
							//was a button clicked
							if (currentState.LeftButton == ButtonState.Pressed && base.previousMouseState.LeftButton == ButtonState.Released) {
								if (BUTTON_ID_EASY == button.ID) {
									StateManager.getInstance().CurrentState = StateManager.GameState.InitEasyGame;
									StateManager.getInstance().CurrentTransitionState = StateManager.TransitionState.TransitionOut;
								} else if (BUTTON_ID_MODERATE == button.ID) {
									StateManager.getInstance().CurrentState = StateManager.GameState.InitModerateGame;
									StateManager.getInstance().CurrentTransitionState = StateManager.TransitionState.TransitionOut;
								} else if (BUTTON_ID_IMPOSSIBLE == button.ID) {
									StateManager.getInstance().CurrentState = StateManager.GameState.InitHardGame;
									StateManager.getInstance().CurrentTransitionState = StateManager.TransitionState.TransitionOut;
								} else if (BUTTON_ID_EXIT == button.ID) {
									StateManager.getInstance().CurrentState = StateManager.GameState.ShutDown;
								}
							}
						}
					} else if (StateManager.getInstance().CurrentTransitionState == StateManager.TransitionState.TransitionIn) {
						if (button.isActorOver(mousePos)) {
							((ColouredButton)button).updateColours(base.fadeIn(ResourceManager.getInstance().ButtonsMouseOverColour));
						} else {
							((ColouredButton)button).updateColours(base.fadeIn(ResourceManager.getInstance().TextColour));
						}
					} else 	if (StateManager.getInstance().CurrentTransitionState == StateManager.TransitionState.TransitionOut) {
						if (button.isActorOver(mousePos)) {
							((ColouredButton)button).updateColours(base.fadeOut(ResourceManager.getInstance().ButtonsMouseOverColour));
						} else {
							((ColouredButton)button).updateColours(base.fadeOut(ResourceManager.getInstance().TextColour));
						}
					}
				}
				// if our transition time is up change our state
				if (base.transitionTimeElapsed()) {
					if (StateManager.getInstance().CurrentTransitionState == StateManager.TransitionState.TransitionIn) {
						StateManager.getInstance().CurrentTransitionState = StateManager.TransitionState.None;
					} else if (StateManager.getInstance().CurrentTransitionState == StateManager.TransitionState.TransitionOut) {
						// if our transition out is done we need to transition in our next set
						StateManager.getInstance().CurrentTransitionState = StateManager.TransitionState.TransitionIn;
						StateManager.getInstance().CurrentState = StateManager.GameState.Active;
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
