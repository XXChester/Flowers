﻿using System;
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
	public class SmokeParticle : BaseParticle2D {
		#region Class variables
		private readonly Vector2 SCALE_BY = new Vector2(.75f / 1000f, .75f / 1000f);//scale per second
		private readonly float ROTATION_SPEED = 10f / 1000f;//rotation per second
		#endregion Class variables

		#region Constructor
		public SmokeParticle(BaseParticle2DParams parms)
			: base(parms) {
		}
		#endregion Constructor

		#region Support methods
		public override void update(float elapsed) {
			base.update(elapsed);
			base.fadeOutAsLifeProgresses();
			base.scaleAsLifeProgresses(SCALE_BY);
			base.rotateAsLifeProgresses(ROTATION_SPEED);
		}
		#endregion Support methods
	}
}