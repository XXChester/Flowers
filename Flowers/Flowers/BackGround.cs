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
using GWNorthEngine.Model;
using GWNorthEngine.Model.Params;
using GWNorthEngine.Scripting;
using GWNorthEngine.Utils;
namespace Flowers {
	public class BackGround : IRenderable {
		#region Class variables
		private Sun sun;
		private Fence fence;
		private House house;
		private StaticDrawable2D backGround;
		private Line2D[] lines;
		private Cloud[] clouds;
		private StaticDrawable2D[] shrubs;
		private SoundEffect birdChirpSFX;
		private float currentWaitTime;
		private const float CHIRP_WAIT_TIME = 45000f;
		#endregion Class variables

		#region Class propeties

		#endregion Class properties

		#region Constructor
		public BackGround(ContentManager content) {
			this.birdChirpSFX = content.Load<SoundEffect>("BirdChirp");
			Texture2D backGroundTexture = content.Load<Texture2D>("BackGround");
			StaticDrawable2DParams backGroundParams = new StaticDrawable2DParams();
			backGroundParams.Texture = backGroundTexture;
			this.backGround = new StaticDrawable2D(backGroundParams);
			this.sun = new Sun(content);
			this.fence = new Fence(content);
			this.house = new House(content);

			// create the boards lines
			Texture2D linesTexture =  content.Load<Texture2D>("LineColour");
			Line2DParams lineParams = new Line2DParams();
			lineParams.Texture = linesTexture;
			lineParams.LightColour = Color.White;
			lineParams.Scale = new Vector2(5f, 5f);
			this.lines = new Line2D[8];
			lineParams.StartPosition = new Vector2(350f, 525f);
			lineParams.EndPosition = new Vector2(900f, 525f);
			this.lines[0] = new Line2D(lineParams);
			lineParams.StartPosition = new Vector2(350f, 612f);
			lineParams.EndPosition = new Vector2(900f, 612f);
			this.lines[1] = new Line2D(lineParams);
			lineParams.StartPosition = new Vector2(515f, 450f);
			lineParams.EndPosition = new Vector2(515f, 700f);
			this.lines[2] = new Line2D(lineParams);
			lineParams.StartPosition = new Vector2(715f, 450f);
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

			// create our clouds
			this.clouds = new Cloud[9];
			float layer1Y =  20f;
			float layer2Y = 70f;
			float layer3Y = 130f;
			this.clouds[0] = new Cloud(new Vector2(500f, layer1Y));
			this.clouds[1] = new Cloud(new Vector2(1000f, layer1Y));
			this.clouds[2] = new Cloud(new Vector2(1400f, layer1Y));
			this.clouds[3] = new Cloud(new Vector2(200f, layer2Y));
			this.clouds[4] = new Cloud(new Vector2(700f, layer2Y));
			this.clouds[5] = new Cloud(new Vector2(1200f, layer2Y));
			this.clouds[6] = new Cloud(new Vector2(10f, layer3Y));
			this.clouds[7] = new Cloud(new Vector2(900f, layer3Y));
			this.clouds[8] = new Cloud(new Vector2(400f, layer3Y));

			// shrubs
			this.shrubs = new StaticDrawable2D[42];
			StaticDrawable2DParams shrubParams = new StaticDrawable2DParams();
			shrubParams.Origin = new Vector2(48f, 48f);
			shrubParams.Scale = new Vector2(1f, .5f);
			shrubParams.Texture = content.Load<Texture2D>("Shrub");

			float x = 320;
			float y = 430;
			float xCutoff = 48f;
			float yCutoff = 30f;
			int index = 0;
			//shrubs across the top
			for (int i = 1; i <= 12; i++) {
				shrubParams.Position = new Vector2(x + (i * xCutoff), y);
				this.shrubs[index] = new StaticDrawable2D(shrubParams);
				index += 1;
			}

			// shrubs down the left
			x = 324;
			y = 430;
			for (int i = 1; i <= 9; i++) {
				shrubParams.Position = new Vector2(x, y + (i * yCutoff));
				this.shrubs[index] = new StaticDrawable2D(shrubParams);
				index += 1;
			}
			// shrubs down the right
			x = 918f;
			for (int i = 1; i <= 9; i++) {
				shrubParams.Position = new Vector2(x, y + (i * yCutoff));
				this.shrubs[index] = new StaticDrawable2D(shrubParams);
				index += 1;
			}

			// shrubs across the bottom
			y = 718f;
			x = 320f;
			for (int i = 1; i <= 12; i++) {
				shrubParams.Position = new Vector2(x + (i * xCutoff), y);
				this.shrubs[index] = new StaticDrawable2D(shrubParams);
				index += 1;
			}
#if WINDOWS
#if DEBUG
			if (this.house != null) {
				ScriptManager.getInstance().registerObject(this.house, "house");
			}
			if (this.backGround != null) {
				ScriptManager.getInstance().registerObject(this.backGround, "backGround");
			}
			if (this.lines != null) {
				for (int i = 0; i < this.lines.Length; i++) {
					ScriptManager.getInstance().registerObject(this.lines[i], "line" + i);
				}
			}
			if (this.shrubs != null) {
				for (int i = 0; i < this.shrubs.Length; i++) {
					ScriptManager.getInstance().registerObject(this.shrubs[i], "shrub" + i);
				}
			}
#endif
#endif
		}
		#endregion Constructor

		#region Support methods
		public void update(float elapsed) {
			if (this.sun != null) {
				this.sun.update(elapsed);
			}
			if (this.clouds != null) {
				foreach (Cloud cloud in this.clouds) {
					cloud.update(elapsed);
				}
			}
			if (this.house != null) {
				this.house.update(elapsed);
			}
			if (this.currentWaitTime == 0) {
				this.birdChirpSFX.Play(.5f, 0f, 0f);
				this.currentWaitTime += elapsed;
			} else if (this.currentWaitTime >= CHIRP_WAIT_TIME) {
				this.currentWaitTime = 0f;
			} else {
				this.currentWaitTime += elapsed;
			}
		}

		public void render(SpriteBatch spriteBatch) {
			if (this.backGround != null) {
				this.backGround.render(spriteBatch);
			}
			if (this.sun != null) {
				this.sun.render(spriteBatch);
			}
			if (this.clouds != null) {
				foreach (Cloud cloud in this.clouds) {
					cloud.render(spriteBatch);
				}
			}
			if (this.lines != null) {
				foreach (Line2D line in this.lines) {
					line.render(spriteBatch);
				}
			}
			if (this.house != null) {
				this.house.render(spriteBatch);
			}
			if (this.fence != null) {
				this.fence.render(spriteBatch);
			}
			if (this.shrubs != null) {
				foreach (StaticDrawable2D shrub in this.shrubs) {
					shrub.render(spriteBatch);
				}
			}
		}
		#endregion Support methods

		#region Destructor
		public void dispose() {
			if (this.sun != null) {
				this.sun.dispose();
			}
			if (this.fence != null) {
				this.fence.dispose();
			}
			if (this.house != null) {
				this.house.dispose();
			}
			if (this.lines != null) {
				foreach (Line2D line in this.lines) {
					line.dispose();
				}
			}
			if (this.clouds != null) {
				foreach (Cloud cloud in this.clouds) {
					cloud.dispose();
				}
			}
			if (this.backGround != null) {
				this.backGround.dispose();
			}
			if (this.shrubs != null) {
				foreach (StaticDrawable2D shrub in this.shrubs) {
					shrub.dispose();
				}
			}
			if (this.birdChirpSFX != null) {
				this.birdChirpSFX.Dispose();
			}
		}
		#endregion Destructor
	}
}
