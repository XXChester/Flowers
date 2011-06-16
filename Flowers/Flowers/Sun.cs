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
namespace Flowers {
	public class Sun : IRenderable {
		#region Class variables
		private StaticDrawable2D innerLayer;
		private StaticDrawable2D outterLayer;
		private const float START_SCALE = 1.2f;
		private const float END_SCALE = 1.25f;
		private readonly Vector2 SCALE_SPEED = new Vector2(5f / 1000f, 5f / 1000f);
		#endregion Class variables

		#region Class propeties

		#endregion Class properties

		#region Constructor
		public Sun(ContentManager content) {
			StaticDrawable2DParams parms = new StaticDrawable2DParams();
			Texture2D sunTx = content.Load<Texture2D>("SunLayer1");
			parms.Texture = sunTx;
			parms.Scale = new Vector2(START_SCALE, START_SCALE);
			parms.Origin = new Vector2(48f, 48f);
			parms.Position = new Vector2(70f, 70f);
			this.innerLayer = new StaticDrawable2D(parms);
			parms.Texture = content.Load<Texture2D>("SunLayer2");
			this.outterLayer = new StaticDrawable2D(parms);
#if WINDOWS
#if DEBUG
			if (this.innerLayer != null) {
				ScriptManager.getInstance().registerObject(this.innerLayer, "sunInner");
			}
			if (this.outterLayer != null) {
				ScriptManager.getInstance().registerObject(this.outterLayer, "sunOutter");
			}
#endif
#endif
		}
		#endregion Constructor

		#region Support methods
		public void update(float elapsed) {
			if (this.outterLayer != null) {
				this.outterLayer.scalingPulse(START_SCALE, END_SCALE, SCALE_SPEED);
			}
		}

		public void render(SpriteBatch spriteBatch) {
			if (this.outterLayer != null) {
				this.outterLayer.render(spriteBatch);
			}
			if (this.innerLayer != null) {
				this.innerLayer.render(spriteBatch);
			}
		}
		#endregion Support methods

		#region Destructor
		public void dispose() {
			if (this.innerLayer != null) {
				this.innerLayer.dispose();
			}
			if (this.outterLayer != null) {
				this.outterLayer.dispose();
			}
		}
		#endregion Destructor
	}
}
