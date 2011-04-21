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
		private IRenderable activeDisplay;
		private IRenderable gameDisplay;
		private IRenderable mainMenu;
		private IRenderable inGameMenu;
		private StaticDrawable2D temp;
		#endregion Class variables

		#region Cosntructor
		public Game1() {
			BaseRendererParams parms = new BaseRendererParams();
			parms.WindowsText = "Flowers";
			parms.ScreenHeight = 720;
			parms.ScreenWidth = 1280;
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
			this.backGround = new BackGround(Content);
			this.gameDisplay = new GameDisplay(base.GraphicsDevice, Content);
			
			this.activeDisplay = this.gameDisplay;
#if WINDOWS
#if DEBUG
			Texture2D texture = TextureUtils.create2DColouredTexture(base.GraphicsDevice, 25, 25, Color.Green);
			StaticDrawable2DParams parms = new StaticDrawable2DParams();
			parms.Texture = texture;
			this.temp = new StaticDrawable2D(parms);
			ScriptManager.getInstance().registerObject(this.temp, "temp");
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
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) {
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
			GraphicsDevice.Clear(Color.White);

			base.spriteBatch.Begin();
			this.backGround.render(spriteBatch);
			this.activeDisplay.render(spriteBatch);
			this.temp.render(spriteBatch);
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
			this.temp.dispose();
			base.UnloadContent();
		}
		#endregion Destructor
	}
}
