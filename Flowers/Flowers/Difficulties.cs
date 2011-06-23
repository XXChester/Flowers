using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
		#region Class variables
		private const int INFINITY = 10000;
		private const int MASTER_ALPHA = -2;
		private const int MASTER_BETA = 2;
		#endregion Class variables

		#region Support methods
		private int max(int a, int b) {
			return (a > b ? a : b);//if a is more than b, return a, else return b
		}

		private int min(int a, int b) {
			return (a < b ? a : b);// if a is less than b, return a, else return b
		}

		private int alphaBeta(Flower.FlowerType[] board, StateManager.TurnType turn, int alpha, int beta) {
			Winner winner;
			if (LogicUtils.isGameOver(board, out winner)) {
				if (winner.winningType == LogicUtils.COMPUTERS_TYPE) {
					return 1;
				} else if (winner.winningType == LogicUtils.PLAYERS_TYPE) {
					return -1;
				} else {
					return 0;
				}
			}
			int score;
			Flower.FlowerType[] cloned;
			for (int i = 0; i < board.Length; i++) {
				if (board[i] == Flower.FlowerType.None) {// get valid moves
					cloned = LogicUtils.cloneFlowerTypes(board);
					if (StateManager.TurnType.Computers == turn) {
						cloned[i] = LogicUtils.COMPUTERS_TYPE;
						score = alphaBeta(cloned, StateManager.TurnType.Players, alpha, beta);
						if (score > alpha) {
							alpha = score;
						}
						if (alpha >= beta) {
							return alpha;
						}
					} else {
						cloned[i] = LogicUtils.PLAYERS_TYPE;
						score = alphaBeta(cloned, StateManager.TurnType.Computers, alpha, beta);
						if (score < beta) {
							beta = score;
						}
						if (alpha >= beta) {
							return beta;
						}
					}
				}
			}
			if (turn == StateManager.TurnType.Computers) {
				return alpha;
			} else {
				return beta;
			}
		}
		//Alpha: a score the computer knows with certanty it can achieve
		//Beta: a score the human knows with certanty it can achieve
		//If beta becomes <= alpha, further investigation is useless
		private int miniMax(Flower.FlowerType[] types, Flower.FlowerType turn, ref int numberOfMoves) {
			int bestMove;
			int move;
			numberOfMoves++;
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
					bestMove = -INFINITY;
				} else {
					bestMove = INFINITY;
				}
				Flower.FlowerType[] cloned = null;
				Flower.FlowerType inversedTurn = EnumUtils.inverseValue<Flower.FlowerType>(turn);
				for (int i = 0; i < types.Length; i++) {
					if (types[i] == Flower.FlowerType.None) {// get valid moves
						cloned = LogicUtils.cloneFlowerTypes(types);
						// get our result
						cloned[i] = turn;
						move = miniMax(cloned, inversedTurn, ref numberOfMoves);
						// interpret our result
						if (LogicUtils.COMPUTERS_TYPE.Equals(turn) && move > bestMove) {
							bestMove = move;
						} else if (LogicUtils.PLAYERS_TYPE.Equals(turn) && move < bestMove) {
							bestMove = move;
						}
					}
				}
			}
			return bestMove;
		}

		public override int getMove(Flower[] board) {
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
				int bestMoveCount = INFINITY;
				int numberOfMoves;
				Flower.FlowerType[] cloned;
				for (int i = 0; i < types.Length; i++) {
					if (types[i] == Flower.FlowerType.None) {// only process empty nodes
						numberOfMoves = 0;
						cloned = LogicUtils.cloneFlowerTypes(types);// clone our board
						cloned[i] = LogicUtils.COMPUTERS_TYPE;// change this piece to the computers piece
						move = miniMax(cloned, LogicUtils.PLAYERS_TYPE, ref numberOfMoves);//run the algorithm
						if (move > bestMove || (move == bestMove && numberOfMoves < bestMoveCount)) {// interpret the results
							result = i;
							bestMove = move;
							bestMoveCount = numberOfMoves;
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
