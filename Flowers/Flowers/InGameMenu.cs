using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GWNorthEngine.Model;
using GWNorthEngine.Model.Params;
using GWNorthEngine.Scripting;
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
			int startX = 1000 - 25;
			ColouredButtonParams buttonParams = new ColouredButtonParams();
			buttonParams.Font = ResourceManager.getInstance().Font;
			buttonParams.LinesTexture = ResourceManager.getInstance().ButtonsLineTexture;
			buttonParams.Height = ResourceManager.BUTTONS_HEIGHT;
			buttonParams.Width = ResourceManager.BUTTONS_WIDTH + 75;// These buttons are a little wider
			buttonParams.MouseOverColour = ResourceManager.getInstance().ButtonsMouseOverColour;
			buttonParams.RegularColour = ResourceManager.getInstance().TextColour;

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
					if (StateManager.getInstance().CurrentTransitionState == StateManager.TransitionState.None) {
						if (button.isActorOver(mousePos)) {
							//was a button clicked
							if (currentState.LeftButton == ButtonState.Pressed && base.previousMouseState.LeftButton == ButtonState.Released) {
								if (BUTTON_ID_EXIT_TO_MAIN_MENU == button.ID) {
									StateManager.getInstance().CurrentState = StateManager.GameState.MainMenu;
									StateManager.getInstance().CurrentTransitionState = StateManager.TransitionState.TransitionOut;
								} else if (BUTTON_ID_RETURN_TO_GAME == button.ID) {
									StateManager.getInstance().CurrentState = StateManager.getInstance().PreviousState;
									StateManager.getInstance().CurrentTransitionState = StateManager.TransitionState.TransitionOut;
								}
							}
						}
					} else if (StateManager.getInstance().CurrentTransitionState == StateManager.TransitionState.TransitionIn) {
						if (button.isActorOver(mousePos)) {
							((ColouredButton)button).updateColours(base.fadeIn(ResourceManager.getInstance().ButtonsMouseOverColour));
						} else {
							((ColouredButton)button).updateColours(base.fadeIn(ResourceManager.getInstance().TextColour));
						}
					} else if (StateManager.getInstance().CurrentTransitionState == StateManager.TransitionState.TransitionOut) {
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
