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
	public class GameDisplay : IRenderable {
		#region Class variables
		private SpriteFont font;
		private Flower[] flowers;
		private Line2D[] lines;
		private int playersScore;
		private int computersScore;
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
			this.lines = new Line2D[4];
			lineParams.StartPosition = new Vector2(350f,515f);
			lineParams.EndPosition = new Vector2(900f, 515f);
			this.lines[0] = new Line2D(lineParams);
			lineParams.StartPosition = new Vector2(350f,615f);
			lineParams.EndPosition = new Vector2(900f, 615f);
			this.lines[1] = new Line2D(lineParams);
			lineParams.StartPosition = new Vector2(515f,450f);
			lineParams.EndPosition = new Vector2(515f, 700f);
			this.lines[2] = new Line2D(lineParams);
			lineParams.StartPosition = new Vector2(715f,450f);
			lineParams.EndPosition = new Vector2(715f, 700f);
			this.lines[3] = new Line2D(lineParams);

			// create the flowers
			this.flowers = new Flower[9];
			this.flowers[0] = new Flower(content, 0, new Vector2(415f, 480));
			this.flowers[1] = new Flower(content, 1, new Vector2(600f, 480f));
			this.flowers[2] = new Flower(content, 2, new Vector2(800f, 480f));
			this.flowers[3] = new Flower(content, 3, new Vector2(415f, 580f));
			this.flowers[4] = new Flower(content, 4, new Vector2(600f, 580f));
			this.flowers[5] = new Flower(content, 5, new Vector2(800f, 580f));
			this.flowers[6] = new Flower(content, 6, new Vector2(415f, 670f));
			this.flowers[7] = new Flower(content, 7, new Vector2(600f, 670f));
			this.flowers[8] = new Flower(content, 8, new Vector2(800f, 670f));
#if WINDOWS
#if DEBUG
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
			if (this.flowers != null) {
				foreach (Flower flower in this.flowers) {
					flower.update(elapsed);
				}
			}
		}

		public void render(SpriteBatch spriteBatch) {
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
		}
		#endregion Support methods

		#region Destructor
		public void dispose() {
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
		}
		#endregion Destructor
	}
}
