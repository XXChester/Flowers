using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Engine.Engine;
using Engine.Model;
using Engine.Model.Params;
using Engine.Scripting;
using Engine.Utils;
namespace Flowers {
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : BaseRenderer {
		#region Class variables
		private IRenderable backGround;
		private Display activeDisplay;
		private Display gameDisplay;
		private Display mainMenu;
		private Display inGameMenu;
		public static readonly Vector2 RESOLUTION = new Vector2(1280f, 720f);
		#endregion Class variables

		#region Cosntructor
		public Game1() {
			BaseRendererParams parms = new BaseRendererParams();
			parms.WindowsText = "Flowers";
			parms.ScreenHeight = (int)RESOLUTION.Y;
			parms.ScreenWidth = (int)RESOLUTION.X;
#if WINDOWS
#if DEBUG
			parms.RunningMode = RunningMode.Debug;
#else
			parms.RunningMode = RunningMode.Release;
#endif
#endif
			base.initialize(parms);
		}
		#endregion Constructor

		#region Support methods

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent() {
			ResourceManager.getInstance().loadResources(GraphicsDevice, Content);
			this.backGround = new BackGround(Content);
			this.mainMenu = new MainMenu(Content);
			this.gameDisplay = new GameDisplay(GraphicsDevice, Content);
			this.inGameMenu = new InGameMenu();
#if WINDOWS
#if DEBUG
			ScriptManager.getInstance().LogFile = "Log.log";
#endif
#endif
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime) {
			if (StateManager.getInstance().CurrentState == StateManager.GameState.ReturnToMainMenu) {
				// if we just came here we need to reset our Game
				((GameDisplay)this.gameDisplay).reset(true);
				StateManager.getInstance().CurrentState = StateManager.GameState.MainMenu;
			}
			if (StateManager.getInstance().CurrentState == StateManager.GameState.MainMenu) {
				this.activeDisplay = this.mainMenu;
			} else if (StateManager.getInstance().CurrentState == StateManager.GameState.InGameMenu) {
				this.activeDisplay = this.inGameMenu;
			} else {
				this.activeDisplay = this.gameDisplay;
			}
			// Allows the game to exit
			if (StateManager.getInstance().CurrentState == StateManager.GameState.MainMenu) {
				if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) {
					this.Exit();
				}
			} else if (StateManager.getInstance().CurrentState == StateManager.GameState.ShutDown) {
				this.Exit();
			}
			float elapsed = gameTime.ElapsedGameTime.Milliseconds;
			this.backGround.update(elapsed);
			this.activeDisplay.update(elapsed);
			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime) {
#if DEBUG
			base.Window.Title = "Flowers...FPS: " + FrameRate.getInstance().calculateFrameRate(gameTime);
#endif
			GraphicsDevice.Clear(Color.White);

			base.spriteBatch.Begin();
			this.backGround.render(spriteBatch);
			this.activeDisplay.render(spriteBatch);
			base.spriteBatch.End();

			base.Draw(gameTime);
		}
		#endregion Support methods

		#region Destructor

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent() {
			if (this.backGround != null) {
				this.backGround.dispose();
			}
			if (this.mainMenu != null) {
				this.mainMenu.dispose();
			}
			if (this.inGameMenu != null) {
				this.inGameMenu.dispose();
			}
			if (this.gameDisplay != null) {
				this.gameDisplay.dispose();
			}
			ResourceManager.getInstance().dispose();
			base.UnloadContent();
		}
		#endregion Destructor
	}
}
