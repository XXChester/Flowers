using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GWNorthEngine.Model;
using GWNorthEngine.Model.Params;
using GWNorthEngine.Scripting;
using GWNorthEngine.Utils;
namespace Flowers {
	public class Fence : IRenderable {
		#region Class variables
		private StaticDrawable2D[] pieces;
		#endregion Class variables

		#region Class propeties
		
		#endregion Class properties

		#region Constructor
		public Fence(ContentManager content) {
			int size = 9;
			this.pieces = new StaticDrawable2D[size];
			Texture2D fenceTexture = LoadingUtils.load<Texture2D>(content, "Fence");
			StaticDrawable2DParams parms = new StaticDrawable2DParams();
			parms.Texture = fenceTexture;
			parms.Scale = new Vector2(1f, 1.5f);
			int textureWidth = fenceTexture.Width;
			for (int i = 0; i < size; i++) {
				parms.Position = new Vector2(i * textureWidth, 330f);
				this.pieces[i] = new StaticDrawable2D(parms);
			}

#if WINDOWS
#if DEBUG
			if (this.pieces != null) {
				for (int i = 0; i < this.pieces.Length; i++) {
					ScriptManager.getInstance().registerObject(this.pieces[i], "fence" + i);
				}
			}
#endif
#endif
		}
		#endregion Constructor

		#region Support methods
		public void update(float elapsed) {

		}

		public void render(SpriteBatch spriteBatch) {
			if (this.pieces != null) {
				foreach (StaticDrawable2D piece in this.pieces) {
					piece.render(spriteBatch);
				}
			}
		}
		#endregion Support methods

		#region Destructor
		public void dispose() {
			if (this.pieces != null) {
				foreach (StaticDrawable2D piece in this.pieces) {
					piece.dispose();
				}
			}
		}
		#endregion Destructor
	}
}
