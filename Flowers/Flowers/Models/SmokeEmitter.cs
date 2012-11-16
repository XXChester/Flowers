using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GWNorthEngine.Engine;
using GWNorthEngine.Model;
using GWNorthEngine.Model.Params;
using GWNorthEngine.Model.Effects;
using GWNorthEngine.Model.Effects.Params;
using GWNorthEngine.Utils;
namespace Flowers {
	public class SmokeEmitter : BaseParticle2DEmitter {
		#region Class variables
		private float elapsedEmittingTime;
		private bool smoking;
		private const float TIME_TO_LIVE = 5000f;
		private const float EMITTER_TIME_TO_LIVE = 35000f;//smoke for 35 seconds
		private readonly Vector2 MOVEMENT_SPEED = new Vector2(-5f / 1000f, -16f / 1000);//speed per second
		public const float SPAWN_DELAY = 500f;
		#endregion Class variables

		#region Constructor
		public SmokeEmitter(BaseParticle2DEmitterParams parms)
			:base(parms) {
			this.elapsedEmittingTime = 0f;
			this.smoking = true;
			BaseParticle2DParams particleParams = new BaseParticle2DParams();
			particleParams.Scale = new Vector2(.25f, .25f);
			particleParams.Position = new Vector2(1199f, 122f);
			particleParams.Origin = new Vector2(32f, 32f);
			particleParams.Texture = this.particleTexture;
			particleParams.LightColour = Color.Black;
			particleParams.TimeToLive = TIME_TO_LIVE;
			particleParams.Direction = MOVEMENT_SPEED;
			base.particleParams = particleParams;
		}
		#endregion Constructor

		#region Support methods
		public override void createParticle() {
			SmokeParticle particle = new SmokeParticle(base.particleParams);
			ScaleOverTimeEffectParams scaleEffectParms = new ScaleOverTimeEffectParams {
				Reference = particle,
				ScaleBy = new Vector2(1f / 1000f, .75f / 1000f)
			};
			particle.addEffect(new ScaleOverTimeEffect(scaleEffectParms));
			RotateOverTimeEffectParams rotateEffectParms = new RotateOverTimeEffectParams {
				Reference = particle,
				RotateBy = 20f / 1000f
			};
			particle.addEffect(new RotateOverTimeEffect(rotateEffectParms));
			FadeEffectParams fadeEffectParms = new FadeEffectParams {
				Reference = particle,
				State = FadeEffect.FadeState.Out,
				TotalTransitionTime = TIME_TO_LIVE,
				OriginalColour = Color.Black
			};
			particle.addEffect(new FadeEffect(fadeEffectParms));

			base.particles.Add(particle);
			base.createParticle();
		}
		public override void update(float elapsed) {
			base.update(elapsed);
			this.elapsedEmittingTime += elapsed;

			// add any new particles if required
			if (this.smoking && this.elapsedSpawnTime >= SPAWN_DELAY) {
				createParticle();
			}

			// check if it is time to turn the chimney off
			if (this.elapsedEmittingTime >= EMITTER_TIME_TO_LIVE) {
				this.smoking = !smoking;
				this.elapsedEmittingTime = 0f;
			}
		}
		#endregion Support methods
	}
}
