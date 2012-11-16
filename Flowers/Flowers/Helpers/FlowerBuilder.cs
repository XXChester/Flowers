using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GWNorthEngine.Model;
using GWNorthEngine.Model.Params;
using GWNorthEngine.Logic;
using GWNorthEngine.Logic.Params;
namespace Flowers {
	public class FlowerBuilder {
		public static Animated2DSprite getFlowerSprite(ContentManager content, int index) {
			return getFlowerSprite(content, FlowerBuilder.SpritePositioner.getInstance().getPosition(index), null, AnimationState.Paused);
		}

		public static Animated2DSprite getFlowerSprite(ContentManager content, Vector2 position, Texture2D texture, AnimationState animationState) {
			BaseAnimationManagerParams animationParams = new BaseAnimationManagerParams();
			animationParams.FrameRate = 100f;
			animationParams.TotalFrameCount = 5;
			animationParams.AnimationState = animationState;
			BaseAnimated2DSpriteParams spriteParams = new Animated2DSpriteLoadSingleRow();
			spriteParams.FramesHeight = 96;
			spriteParams.FramesWidth = 96;
			spriteParams.Position = position;
			spriteParams.Origin = new Vector2(40f, 80f);
			spriteParams.Texture = texture;
			spriteParams.AnimationParams = animationParams;
			return new Animated2DSprite(spriteParams);
		}

		public class SpritePositioner {
			#region Class variables
			// singleton instance variable
			private static SpritePositioner instance = new SpritePositioner();
			private Vector2[] positions = new Vector2[9] { 
			new Vector2(430f, 502f), new Vector2(620f, 502f), new Vector2(820f, 502f),
			new Vector2(430f, 590f), new Vector2(620f, 590f), new Vector2(820f, 590f),
			new Vector2(430f, 678f), new Vector2(620f, 678f), new Vector2(820f, 678f),
		};

			private Rectangle[] rectangles = new Rectangle[9]  {
			new Rectangle(354, 448, 161, 72), new Rectangle(519, 448, 198, 72), new Rectangle(719, 448, 182, 72),
			new Rectangle(354, 524, 161, 85), new Rectangle(519,524,198,85), new Rectangle(719, 524, 182, 85),
			new Rectangle(354,615, 161, 85), new Rectangle(519,615,198,85), new Rectangle(719,615,182,85),
		};
			#endregion Class variables
			#region Support methods
			public static SpritePositioner getInstance() {
				return instance;
			}

			public Vector2 getPosition(int index) {
				return (this.positions[index]);
			}

			public Rectangle getPositionsRectangle(int index) {
				return (this.rectangles[index]);
			}
			#endregion Support methods
		}
	}
}
