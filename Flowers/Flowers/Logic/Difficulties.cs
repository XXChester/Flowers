using System;
using GWNorthEngine.Utils;
namespace Flowers {
	#region Base difficulty class
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
	#endregion Base difficulty class

	#region Easy difficulty
	public class EasyDifficulty : Difficulty {
		#region Support methods
		public override int getMove(Flower[] board) {
			return base.generateMove(board);
		}
		#endregion Support methods
	}
	#endregion Easy difficulty

	#region Moderate difficulty
	public class ModerateDifficulty : Difficulty {
		#region Support methods
		public override int getMove(Flower[] board) {
			int result = -1;
			// is there a win possible?
			if (!LogicUtils.isWinOrBlockAvailable(board, LogicUtils.COMPUTERS_TYPE, ref result)) {
				//didn't find anything so generate a move
				result = base.generateMove(board);
			}
			return result;
		}
		#endregion Support methods
	}
	#endregion Moderate difficulty

	#region Hard difficulty
	public class HardDifficulty : Difficulty {
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
	#endregion Hard difficulty

	#region Impossible difficulty
	public class ImpossibleDifficulty : Difficulty {
		#region Support methods
		//Alpha: a score the computer knows with certanty it can achieve
		//Beta: a score the human knows with certanty it can achieve
		//If beta becomes <= alpha, further investigation is useless
		private int miniMax(Flower.FlowerType[] types, Flower.FlowerType turn, int alpha, int beta) {
			int bestMove;
			int move;
			Winner winner;
			if (LogicUtils.isGameOver(types, out winner)) {
				if (winner.winningType == LogicUtils.COMPUTERS_TYPE) {
					return 1;
				} else if (winner.winningType == LogicUtils.PLAYERS_TYPE) {
					return -1;
				} else {
					return 0;
				}
			} else {
				if (LogicUtils.COMPUTERS_TYPE.Equals(turn)) {
					bestMove = alpha;
				} else {
					bestMove = beta;
				}
				Flower.FlowerType[] cloned = null;
				Flower.FlowerType inversedTurn = EnumUtils.inverseValue<Flower.FlowerType>(turn);
				for (int i = 0; i < types.Length; i++) {
					if (types[i] == Flower.FlowerType.None) {// get valid moves
						cloned = LogicUtils.cloneFlowerTypes(types);
						// get our result
						cloned[i] = turn;
						move = miniMax(cloned, inversedTurn, alpha, beta);
						// interpret our result
						if (LogicUtils.COMPUTERS_TYPE == turn) {
							if (move > alpha) {
								alpha = move;
							}
							if (alpha >= beta) {
								return alpha;
							}
						} else {
							if (move < beta) {
								beta = move;
							}
							if (alpha >= beta) {
								return beta;
							}
						}
					}
				}
			}
			if (turn == LogicUtils.COMPUTERS_TYPE) {
				return alpha;
			} else {
				return beta;
			}
		}

		public override int getMove(Flower[] board) {
			const int INFINITY = 10000;
			int result = -INFINITY;
			Flower.FlowerType[] types = LogicUtils.getFlowerTypes(board);
			// figure out if this is our first move
			bool firstMove = true;
			for (int i = 0; i < types.Length; i++) {
				if (types[i] != Flower.FlowerType.None) {
					firstMove = false;
					break;
				}
			}
			if (firstMove) {
				result = base.generateMove(board);
			} else {
				int move;
				int bestMove = -INFINITY;
				int alpha = -INFINITY;
				int beta = INFINITY;
				Flower.FlowerType[] cloned;
				Winner winner;
				for (int i = 0; i < types.Length; i++) {
					if (types[i] == Flower.FlowerType.None) {// only process empty nodes
						cloned = LogicUtils.cloneFlowerTypes(types);// clone our board
						cloned[i] = LogicUtils.COMPUTERS_TYPE;// change this piece to the computers piece
						if (LogicUtils.isGameOver(cloned, out winner)) {// check if this move puts the game into game over state
							if (winner.winningType == LogicUtils.COMPUTERS_TYPE) {// check if the computer wins
								result = i;
								break;// we found a win so no need to continue looping
							}
						}
						move = miniMax(cloned, LogicUtils.PLAYERS_TYPE, alpha, beta);//run the algorithm
						if (move > bestMove) {// interpret the results
							result = i;
							bestMove = move;
						}
					}
				}
			}
			return result;
		}
		#endregion Support methods
	}
	#endregion Impossible difficulty
}
