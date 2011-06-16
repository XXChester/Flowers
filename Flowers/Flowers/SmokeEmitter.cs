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
	public class SmokeEmitter : IRenderable {
		#region Class variables
		private Texture2D particleTexture;
		private BaseParticle2DParams particleParms;
		private List<SmokeParticle> smokeParticles;
		private float elapsedSpawnTime;
		private const float SPAWN_DELAY = 500f;
		private const float TIME_TO_LIVE = 5000f;
		private readonly Vector2 MOVEMENT_SPEED = new Vector2(-5f / 1000f, -16f / 1000);//speed per second
		#endregion Class variables

		#region Class properties

		#endregion Class properties

		#region Constructor
		public SmokeEmitter(ContentManager content) {
			this.particleTexture = content.Load<Texture2D>("Smoke1");
			this.particleParms = new BaseParticle2DParams();
			this.particleParms.Scale = new Vector2(.25f, .25f);
			this.particleParms.Position = new Vector2(1199f, 122f);
			this.particleParms.Origin = new Vector2(32f, 32f);
			this.particleParms.Texture = this.particleTexture;
			this.particleParms.LightColour = Color.Black;
			this.particleParms.TimeToLive = TIME_TO_LIVE;
			this.particleParms.Direction = MOVEMENT_SPEED;
			this.smokeParticles = new List<SmokeParticle>();
			this.elapsedSpawnTime = SPAWN_DELAY;
		}
		#endregion Constructor

		#region Support methods
		public void update(float elapsed) {
			this.elapsedSpawnTime += elapsed;
			List<int> indexesUpForRemoval = new List<int>();
			BaseParticle2D smokeParticle = null;
			for (int i = 0; i < this.smokeParticles.Count; i++) {
				smokeParticle = this.smokeParticles[i];
				smokeParticle.update(elapsed);
				if (smokeParticle.TimeAlive >= smokeParticle.TimeToLive) {
					// mark the particle for removal to avoid concurrent access violations
					indexesUpForRemoval.Add(i);
				}
			}
			for (int i = 0; i < indexesUpForRemoval.Count; i++) {
				this.smokeParticles.RemoveAt(indexesUpForRemoval[i]);
			}

			// update our existing particles positions
			for (int i = 0; i < this.smokeParticles.Count; i++) {
				smokeParticle = this.smokeParticles[i];
				if (smokeParticle != null) {
					smokeParticle.Position = smokeParticle.Position + (smokeParticle.Direction * elapsed);
				}
			}

			// add any new particles if required
			if (this.elapsedSpawnTime >= SPAWN_DELAY) {
				this.smokeParticles.Add(new SmokeParticle(this.particleParms));
				this.elapsedSpawnTime = 0f;
			}
		}

		public void render(SpriteBatch spriteBatch) {
			if (this.smokeParticles != null) {
				foreach (BaseParticle2D smokeParticle in this.smokeParticles) {
					smokeParticle.render(spriteBatch);
				}
			}
		}
		#endregion Support methods

		#region Destructor
		public void dispose() {
			if (this.particleTexture != null) {
				this.particleTexture.Dispose();
			}
		}
		#endregion Destructor
	}
}
