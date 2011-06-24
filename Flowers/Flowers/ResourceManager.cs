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
using GWNorthEngine.Utils;
namespace Flowers {
	public class ResourceManager {
		#region Class variables
		//singleton variable
		private static ResourceManager instance = new ResourceManager();
		public const int BUTTONS_WIDTH = 150;
		public const int BUTTONS_HEIGHT = 50;
		public const float BUTTONS_TEXT_Y_DIFFERENCE = 10f;
		#endregion Class variables

		#region Class propeties
		public SpriteFont Font { get; set; }
		public Texture2D ButtonsLineTexture { get; set; }
		public Texture2D CloudTexture1 { get; set; }
		public Texture2D CloudTexture2 { get; set; }
		public Texture2D ShrubTexture { get; set; }
		public Color ButtonsMouseOverColour { get; set; }
		public Color TextColour { get; set; }
		#endregion Class properties

		#region Constructor

		#endregion Constructor

		#region Support methods
		public static ResourceManager getInstance() {
			return instance;
		}

		public void loadResources(GraphicsDevice device, ContentManager content) {
			this.Font = LoadingUtils.loadSpriteFont(content, "HUDFont");
			this.CloudTexture1 = LoadingUtils.loadTexture2D(content, "Cloud1");
			this.CloudTexture2 = LoadingUtils.loadTexture2D(content, "Cloud2");
			this.ShrubTexture = LoadingUtils.loadTexture2D(content, "Shrub");
			this.ButtonsLineTexture = TextureUtils.create2DColouredTexture(device, 1, 1, Color.White);
			this.ButtonsMouseOverColour = Color.SkyBlue;
			this.TextColour = Color.White;
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
			if (this.CloudTexture1 != null) {
				this.CloudTexture1.Dispose();
			}
			if (this.CloudTexture2 != null) {
				this.CloudTexture2.Dispose();
			}
			if (this.ShrubTexture != null) {
				this.ShrubTexture.Dispose();
			}
		}
		#endregion Destructor
	}
}
