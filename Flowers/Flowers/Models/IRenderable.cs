using Microsoft.Xna.Framework.Graphics;
namespace Flowers {
	interface IRenderable {
		void update(float elapsed);
		void render(SpriteBatch spriteBatch);
		void dispose();
	}
}
