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
using GWNorthEngine.Model;
using GWNorthEngine.Model.Params;
using GWNorthEngine.Scripting;
using GWNorthEngine.Utils;
namespace Flowers {
	public class House : IRenderable {
		#region Class variables
		private StaticDrawable2D house;
		private SmokeEmitter chimneySmokeEmitter;
		#endregion Class variables

		#region Class propeties

		#endregion Class properties

		#region Constructor
		public House(ContentManager content) {
			Texture2D houseTexture = content.Load<Texture2D>("House");
			StaticDrawable2DParams houseParams = new StaticDrawable2DParams();
			houseParams.Texture = houseTexture;
			houseParams.Position = new Vector2(1000f, 100f);
			this.house = new StaticDrawable2D(houseParams);
			this.chimneySmokeEmitter = new SmokeEmitter(content);
		}
		#endregion Constructor

		#region Support methods

		public void update(float elapsed) {
			this.chimneySmokeEmitter.update(elapsed);
		}

		public void render(SpriteBatch spriteBatch) {
			if (this.house != null) {
				this.house.render(spriteBatch);
			}
			this.chimneySmokeEmitter.render(spriteBatch);
		}
		#endregion Support methods

		#region Destructor
		public void dispose() {
			if (this.house != null) {
				this.house.dispose();
			}
			if (this.chimneySmokeEmitter != null) {
				this.chimneySmokeEmitter.dispose();
			}
		}
		#endregion Destructor
	}
}
