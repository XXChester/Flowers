using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flowers {
	public abstract class Difficulty {
		#region Class variables
		protected Random random;
		#endregion Class variables

		#region Constructor
		public Difficulty() {
			this.random = new Random();
		}
		#endregion Constructor

		#region Support methods
		protected int generateMove(Flower[] board) {
			int result = -1;
			int move = -1;
			while (result == -1) {
				move = this.random.Next(9);
				if (board[move].Type == Flower.FlowerType.None) {
					result = move;
					break;
				}
			}
			return result;
		}

		public abstract int getMove(Flower[] board);
		#endregion Support methods
	}

	public class EasyDifficulty : Difficulty {
		#region Support methods
		public override int getMove(Flower[] board) {
			return base.generateMove(board);
		}
		#endregion Support methods
	}

	public class ModerateDifficulty : Difficulty {
		#region Support methods
		public override int getMove(Flower[] board) {
			int result = -1;
			// is there a win possible?
			if (!LogicUtils.isWinOrBlockAvailable(board, LogicUtils.COMPUTERS_TYPE, ref result)) {
				// is there a block available?
				if (!LogicUtils.isWinOrBlockAvailable(board, LogicUtils.PLAYERS_TYPE, ref result)) {
					//didn't find anything so generate a move
					result = base.generateMove(board);
				}
			}
			return result;
		}
		#endregion Support methods
	}

	public class HardDifficulty : Difficulty {
		#region Support methods
		public override int getMove(Flower[] board) {
			int result = -1;

			return result;
		}
		#endregion Support methods
	}
}
