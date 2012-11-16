using Microsoft.Xna.Framework;
using GWNorthEngine.Model;
using GWNorthEngine.Model.Effects;
using GWNorthEngine.Model.Params;
namespace Flowers {
	public class SmokeParticle : BaseParticle2D {
		#region Class variables
		#endregion Class variables

		#region Constructor
		public SmokeParticle(BaseParticle2DParams parms)
			: base(parms) {
		}
		#endregion Constructor

		#region Support methods
		public override void update(float elapsed) {
			base.update(elapsed);
			base.position += (base.direction * elapsed);
		}
		#endregion Support methods
	}
}
