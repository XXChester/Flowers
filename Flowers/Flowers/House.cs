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
		private Texture2D particleTexture;
		private BaseParticle2DParams particleParms;
		private List<BaseParticle2D> smokeParticles;
		private Vector2[] positions;
		private int nextPosition;
		private const int MAX_PARTICLES = 15;
		#endregion Class variables

		#region Class propeties

		#endregion Class properties

		#region Constructor
		public House(ContentManager content) {
			Texture2D houseTexture = content.Load<Texture2D>("House");
			this.particleTexture = content.Load<Texture2D>("Particle");
			StaticDrawable2DParams houseParams = new StaticDrawable2DParams();
			houseParams.Texture = houseTexture;
			houseParams.Position = new Vector2(1000f, 100f);
			this.house = new StaticDrawable2D(houseParams);

			this.particleParms = new BaseParticle2DParams();
			this.particleParms.Position = new Vector2(1000f, 200f);
			this.particleParms.Origin = new Vector2(8f, 8f);
			this.particleParms.Texture = this.particleTexture;
			this.particleParms.LightColour = Color.Black;
			this.particleParms.TimeToLive = 15000f;
			this.particleParms.Direction = new Vector2(-4f, -1f);
			this.particleParms.Acceleration = new Vector2(-10f, -10f);
			this.smokeParticles = new List<BaseParticle2D>();

			this.positions = new Vector2[3] {
				new Vector2(1192f,124f),
				new Vector2(1199f,124f),
				new Vector2(1208f,124f)
			};
			this.nextPosition = 0;
		}
		#endregion Constructor

		#region Support methods

		public void update(float elapsed) {
			if (this.smokeParticles.Count < MAX_PARTICLES) {
				if (this.nextPosition == this.positions.Length - 1) {
					this.nextPosition = 0;
				} else {
					this.nextPosition++;
				}
				this.particleParms.Position = this.positions[this.nextPosition];
				this.smokeParticles.Add(new BaseParticle2D(this.particleParms));
			}
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
			/*if (Mouse.GetState().LeftButton == ButtonState.Pressed) {
				Console.WriteLine(Mouse.GetState().X + " " + Mouse.GetState().Y);
			}*/
		}

		public void render(SpriteBatch spriteBatch) {
			if (this.house != null) {
				this.house.render(spriteBatch);
			}
			if (this.smokeParticles != null) {
				foreach (BaseParticle2D smokeParticle in this.smokeParticles) {
					smokeParticle.render(spriteBatch);
				}
			}
		}
		#endregion Support methods

		#region Destructor
		public void dispose() {
			if (this.house != null) {
				this.house.dispose();
			}
			if (this.particleTexture != null) {
				this.particleTexture.Dispose();
			}
		}
		#endregion Destructor
	}
}
