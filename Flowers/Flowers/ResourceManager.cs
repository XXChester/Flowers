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
using Engine.Utils;
namespace Flowers {
	public class ResourceManager {
		#region Class variables
		//singleton variable
		private static ResourceManager instance = new ResourceManager();
		#endregion Class variables

		#region Class propeties
		public SpriteFont Font { get; set; }
		public Texture2D ButtonsLineTexture { get; set; }
		public Color ButtonsMouseOverColour { get; set; }
		public Color ButtonsRegularColour { get; set; }
		public int ButtonsHeight { get; set; }
		public int ButtonsWidth { get; set; }
		public float ButtonsTextYDifference { get; set; }
		public int ButtonsStartX { get; set; }
		#endregion Class properties

		#region Constructor

		#endregion Constructor

		#region Support methods
		public static ResourceManager getInstance() {
			return instance;
		}

		public void loadResources(GraphicsDevice device, ContentManager content) {
			this.Font = content.Load<SpriteFont>("HUDFont");
			this.ButtonsLineTexture = TextureUtils.create2DColouredTexture(device, 1, 1, Color.White);
			this.ButtonsMouseOverColour = Color.Gray;
			this.ButtonsRegularColour = Color.Black;
			this.ButtonsHeight = 50;
			this.ButtonsWidth = 150;
			this.ButtonsTextYDifference = 10f;
			this.ButtonsStartX = 1000;
		}
		#endregion Support methods

		#region Destructor
		~ResourceManager() {
			dispose();
		}

		public void dispose() {
			if (this.ButtonsLineTexture != null) {
				this.ButtonsLineTexture.Dispose();
			}
		}
		#endregion Destructor
	}
}
