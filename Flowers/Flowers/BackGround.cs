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
	public class BackGround : IRenderable {
		#region Class variables
		private Sun sun;
		private Fence fence;
		private StaticDrawable2D house;
		private StaticDrawable2D backGround;
		private Line2D[] lines;
		private Cloud[] clouds;
		#endregion Class variables

		#region Class propeties

		#endregion Class properties

		#region Constructor
		public BackGround(ContentManager content) {
			Texture2D houseTexture = content.Load<Texture2D>("House");
			Texture2D backGroundTexture = content.Load<Texture2D>("BackGround");

			StaticDrawable2DParams backGroundParams = new StaticDrawable2DParams();
			backGroundParams.Texture = backGroundTexture;
			this.backGround = new StaticDrawable2D(backGroundParams);

			backGroundParams.Texture = houseTexture;
			backGroundParams.Position = new Vector2(1000f, 100f);
			this.house = new StaticDrawable2D(backGroundParams);
			this.sun = new Sun(content);
			this.fence = new Fence(content);

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
			if (this.fence != null) {
				this.fence.render(spriteBatch);
			}
			if (this.house != null) {
				this.house.render(spriteBatch);
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
		}
		#endregion Destructor
	}
}
