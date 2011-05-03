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
namespace Flowers {
	public class Player : IRenderable {
		#region Class variables
		private string name;
		#endregion Class variables

		#region Class properties
		public Text2D Text { get; set; }
		public Texture2D AliveTexture { get; set; }
		public Texture2D DyingTexture { get; set; }
		public Flower.FlowerType FlowerType { get; set; }
		public int Score { get; set; }
		#endregion Class properties

		#region Constructor
		public Player(ContentManager content, SpriteFont font, string name, Vector2 position, string aliveTexture, string dyingTexture, Flower.FlowerType flowerType) {
			this.name = name;
			this.AliveTexture = content.Load<Texture2D>(aliveTexture);
			this.DyingTexture = content.Load<Texture2D>(dyingTexture);
			this.FlowerType = flowerType;
			this.Score = 0;
			Text2DParams textParams = new Text2DParams();
			textParams.Font = font;
			textParams.LightColour = ResourceManager.getInstance().TextColour;
			textParams.Position = position;
			this.Text = new Text2D(textParams);
		}
		#endregion Constructor

		#region Support methods
		public void update(float elapsed) {
			this.Text.WrittenText = name + ": " + this.Score;
		}

		public void render(SpriteBatch spriteBatch) {
			this.Text.render(spriteBatch);
		}
		#endregion Support methods

		#region Destructor
		public void dispose() {
			if (this.AliveTexture != null) {
				this.AliveTexture.Dispose();
			}
			if (this.DyingTexture != null) {
				this.DyingTexture.Dispose();
			}
		}
		#endregion Destructor
	}
}
