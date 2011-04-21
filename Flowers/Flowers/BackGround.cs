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

namespace Flowers {
	public class BackGround : IRenderable {
		#region Class variables
		private Sun sun;
		private Fence fence;
		private StaticDrawable2D house;
		private StaticDrawable2D backGround;
		private StaticDrawable2D[] clouds;
		#endregion Class variables

		#region Class propeties

		#endregion Class properties

		#region Constructor
		public BackGround(ContentManager content) {
			Texture2D houseTexture = content.Load<Texture2D>("House");
			Texture2D backGroundTexture = content.Load<Texture2D>("BackGround");

			StaticDrawable2DParams parms = new StaticDrawable2DParams();
			parms.Texture = backGroundTexture;
			this.backGround = new StaticDrawable2D(parms);

			parms.Texture = houseTexture;
			parms.Position = new Vector2(1000f, 100f);
			this.house = new StaticDrawable2D(parms);
			this.sun = new Sun(content);
			this.fence = new Fence(content);
#if WINDOWS
#if DEBUG
			if (this.house != null) {
				ScriptManager.getInstance().registerObject(this.house, "house");
			}
			if (this.backGround != null) {
				ScriptManager.getInstance().registerObject(this.backGround, "backGround");
			}
			if (this.clouds != null) {
				for (int i = 0; i < this.clouds.Length; i++) {
					ScriptManager.getInstance().registerObject(this.clouds[i], "cloud" + i);
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
				foreach (StaticDrawable2D cloud in this.clouds) {
					cloud.update(elapsed);
				}
			}
		}

		public void render(SpriteBatch spriteBatch) {
			if (this.sun != null) {
				this.sun.render(spriteBatch);
			}
			if (this.clouds != null) {
				foreach (StaticDrawable2D cloud in this.clouds) {
					cloud.render(spriteBatch);
				}
			}
			if (this.backGround != null) {
				this.backGround.render(spriteBatch);
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
			if (this.clouds != null) {
				foreach (StaticDrawable2D cloud in this.clouds) {
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
