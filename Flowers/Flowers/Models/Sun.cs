using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GWNorthEngine.Model;
using GWNorthEngine.Model.Params;
using GWNorthEngine.Model.Effects;
using GWNorthEngine.Model.Effects.Params;
using GWNorthEngine.Scripting;
using GWNorthEngine.Utils;
namespace Flowers {
	public class Sun : IRenderable {
		#region Class variables
		private StaticDrawable2D innerLayer;
		private StaticDrawable2D outterLayer;
		private const float START_SCALE = 1.2f;
		private const float END_SCALE = 1.25f;
		#endregion Class variables

		#region Class propeties

		#endregion Class properties

		#region Constructor
		public Sun(ContentManager content) {
			StaticDrawable2DParams parms = new StaticDrawable2DParams();
			Texture2D sunTx = LoadingUtils.load<Texture2D>(content, "SunLayer1");
			parms.Texture = sunTx;
			parms.Scale = new Vector2(START_SCALE, START_SCALE);
			parms.Origin = new Vector2(48f, 48f);
			parms.Position = new Vector2(70f, 70f);
			this.innerLayer = new StaticDrawable2D(parms);
			parms.Texture = LoadingUtils.load<Texture2D>(content, "SunLayer2");
			this.outterLayer = new StaticDrawable2D(parms);

			PulseEffectParams effectParms = new PulseEffectParams {
				Reference = this.outterLayer,
				ScaleBy = 5f / 1000f,
				ScaleDownTo = START_SCALE,
				ScaleUpTo = END_SCALE
			};
			this.outterLayer.addEffect(new PulseEffect(effectParms));
			
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
				this.outterLayer.update(elapsed);
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
