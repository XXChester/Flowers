using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Utils;
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
	#endregion Moderate difficulty

	#region Hard difficulty
	#region MinMax psuedo
	//int minMax(Node, depth) {
	//	if node is a terminal node or depth <= 0
	//		return the hueristic value of the node
	//	a = -INFINITY
	//	foreach child in node.children
	//		a = max(a, -minMax(child, depth - 1);
	//			//max is the greatest a value of all the children
	//	return a
	#endregion MinMax psudeo
	public class HardDifficulty : Difficulty {
		#region Class variables
		private const int INFINITY = 10000;
		private const int MASTER_ALPHA = -2;
		private const int MASTER_BETA = 2;
		#endregion Class variables

		#region Support methods
		private bool areNodesTerminal(Flower.FlowerType[] types) {
			bool terminal = true;
			// if the 3 types are not the same there is no win
			Flower.FlowerType previousType = types[0];
			for (int i = 1; i < types.Length; i++) {
				if (types[i] != previousType) {
					terminal = false;
					break;
				}
			}
			return terminal;
		}

		private int max(int a, int b) {
			return (a > b ? a : b);//if a is more than b, return a, else return b
		}

		private int min(int a, int b) {
			return (a < b ? a : b);// if a is less than b, return a, else return b
		}

		private int isGameOver(Flower.FlowerType[] types) {
			int value = -INFINITY;
			int[] dummy;
			Flower.FlowerType winningType;
			if (LogicUtils.isGameOver(types, out winningType, out dummy)) {
				if (LogicUtils.COMPUTERS_TYPE.Equals(winningType)) {
					value = 1;// maximizer
				} else if (LogicUtils.PLAYERS_TYPE.Equals(winningType)) {
					value = -1;// minamizer
				}
				value = 0;
			}
			return value;
		}

		private int maxValue(Flower.FlowerType[] types, int alpha, int beta) {
			int value = isGameOver(types);
			if (value == -INFINITY) {
				value = MASTER_ALPHA;
				Flower.FlowerType[] clonedTypes;
				for (int i = 0; i < types.Length; i++) {
					clonedTypes = LogicUtils.cloneFlowerTypes(types);
					// if the type is none than process minimax
					if (clonedTypes[i] == Flower.FlowerType.None) {
						clonedTypes[i] = LogicUtils.COMPUTERS_TYPE;	// maximizer
						value = max(value, minValue(clonedTypes, alpha, beta));
						if (value >= beta) {
							break;
						}
						alpha = max(alpha, value);
						//types[i] = Flower.FlowerType.None;
					}
				}
			}
			return value;
		}

		private int minValue(Flower.FlowerType[] types, int alpha, int beta) {
			int value = isGameOver(types);
			if (value == -INFINITY) {
				value = MASTER_BETA;
				Flower.FlowerType[] clonedTypes;
				for (int i = 0; i < types.Length; i++) {
					clonedTypes = LogicUtils.cloneFlowerTypes(types);
					// if the type is none than process minimax
					if (clonedTypes[i] == Flower.FlowerType.None) {
						clonedTypes[i] = LogicUtils.PLAYERS_TYPE;	// minamizer
						value = min(value, maxValue(clonedTypes, alpha, beta));
						if (value <= alpha) {
							break;
						}
						beta = min(beta, value);
						//types[i] = Flower.FlowerType.None;
					}
				}
			}
			return value;
		}
		#region Tests
		private int miniMax(Flower.FlowerType[] types,/* StateManager.TurnType turn,*/ int depth) {
			// computer is the maximizer
			int[] dummy;
			Flower.FlowerType winningType;
			if (LogicUtils.isGameOver(types, out winningType, out dummy)) {
				if (winningType == LogicUtils.COMPUTERS_TYPE) {
					return +1;
				} else if (winningType == LogicUtils.PLAYERS_TYPE) {
					return -1;
				}
				return 0;
			} else if (depth <= 0) {
				return 0;
			} else {
				int a = -INFINITY;
				types = LogicUtils.cloneFlowerTypes(types);
				for (int i = 0; i < types.Length; i++) {
					// if the type is none than process minimax
					if (types[i] == Flower.FlowerType.None) {
						types[i] = LogicUtils.COMPUTERS_TYPE;
						a = max(a, miniMax(types, depth - 1));
					}
				}
				return a;
				/*int value = -INFINITY;
				int bestScore = -INFINITY;
				int previousScore = -INFINITY;
				for (int i = 0; i < types.Length; i++) {
					// if the type is none than process minimax
					if (types[i] == Flower.FlowerType.None) {
						if (turn == StateManager.TurnType.Computers) {
							types[i] = LogicUtils.COMPUTERS_TYPE;
							value = (miniMax(types, StateManager.TurnType.Players, depth - 1));
						} else {
							types[i] = LogicUtils.PLAYERS_TYPE;
							value = (miniMax(types, StateManager.TurnType.Computers, depth - 1));
						}

						if (value > previousScore) {
							bestScore = i;
						}
						types[i] = Flower.FlowerType.None;
						previousScore = value;
					}
				}*/
				/*int value = INFINITY;
				int bestScore = INFINITY;
				int previousScore = INFINITY;
				for (int i = 0; i < types.Length; i++) {
					// if the type is none than process minimax
					if (types[i] == Flower.FlowerType.None) {
						types[i] = LogicUtils.COMPUTERS_TYPE;
						value = (miniMax(types, turn, depth -1));
						if (value < previousScore) {
							bestScore = value;
						}
						types[i] = Flower.FlowerType.None;
						previousScore = value;
					}
				}*/
				/*int bestMove;
				if (turn == StateManager.TurnType.Computers) {
					bestScore = -INFINITY;
					bestMove = -INFINITY;
				} else {
					bestScore = INFINITY;
					bestMove = INFINITY;
				}
				int newScore;
				for (int i = 0; i < types.Length; i++) {
					// if the type is none than process minimax
					if (types[i] == Flower.FlowerType.None) {
						if (turn == StateManager.TurnType.Computers) {
							types[i] = LogicUtils.COMPUTERS_TYPE;
							newScore = miniMax(types, StateManager.TurnType.Players);
							if (newScore > bestScore) {
								bestScore = newScore;
								bestMove = i;
							}
						} else {
							types[i] = LogicUtils.PLAYERS_TYPE;
							newScore = miniMax(types, StateManager.TurnType.Computers);
							if (newScore < bestScore) {
								bestScore = newScore;
								bestMove = i;
							}
						}
						// reset the type
						types[i] = Flower.FlowerType.None;
					}
				}
				return bestMove;*/
				//return bestScore;
			}
		}

		/*private int minMax(Flower.FlowerType[] types, int depth) {
			int bestMove = +INFINITY;
			int value;
			int index = 0;
			int[] bestMoves = new int[9];
			for (int i = 0; i < types.Length; i++) {
				// if the type is none
				if (types[i] == Flower.FlowerType.None) {
					types[i] = LogicUtils.COMPUTERS_TYPE;
					value = maxSearch(types);
					if (value < bestMove) {
						bestMove = value;
						index = 0;
						bestMoves[index] = i;
					} else if (value == bestMove) {
						bestMoves[index++] = i;
					}
					// set the type back to none
					types[i] = Flower.FlowerType.None;
				}
			}
			if (index > 0) {
				index = random.Next() % index;
			}
			//return bestMove;
			return bestMoves[index];
		}

		private int minSearch(Flower.FlowerType[] types) {
			int bestMove = +INFINITY;
			int value;
			if (LogicUtils.isGameOver(types)) {
				bestMove = 0;
			} else {
				for (int i = 0; i < types.Length; i++) {
					// if the type is none
					if (types[i] == Flower.FlowerType.None) {
						types[i] = LogicUtils.COMPUTERS_TYPE;
						value = maxSearch(types);
						if (value < bestMove) {
							bestMove = value;
						}
						// reset this type back to none
						types[i] = Flower.FlowerType.None;
					}
				}
			}
			return bestMove;
		}

		private int maxSearch(Flower.FlowerType[] types) {
			int bestMove = -INFINITY;
			int value;
			if (LogicUtils.isGameOver(types)) {
				bestMove = 0;
			} else {
				for (int i = 0; i < types.Length; i++) {
					// if the type is none
					if (types[i] == Flower.FlowerType.None) {
						types[i] = LogicUtils.PLAYERS_TYPE;
						value = minSearch(types);
						if (value > bestMove) {
							bestMove = value;
						}
						// reset this type back to none
						types[i] = Flower.FlowerType.None;
					}
				}
			}
			return bestMove;
		}*/
		
		/*private Flower.FlowerType[] getOpenSpaces(Flower.FlowerType[] types) {
			List<Flower.FlowerType> openSpaces = new List<Flower.FlowerType>();
			for (int i = 0; i < types.Length; i++) {
				if (types[i] == Flower.FlowerType.None) {
					openSpaces.Add(types[i]);
				}
			}
			return openSpaces.ToArray<Flower.FlowerType>();
		}*/

		/*private int miniMax(Flower.FlowerType[] types, Flower.FlowerType turn) {
			int move = -INFINITY;
			//Flower.FlowerType[] openSpaces = getOpenSpaces(types);
			Flower.FlowerType[] newTypes;
			int value;
			int index = -INFINITY;
			for (int i = 0; i < types.Length; i++) {
				if (types[i] == Flower.FlowerType.None) {
					newTypes = LogicUtils.cloneFlowerTypes(types);
					newTypes[i] = turn;
					value = isGameOver(newTypes);
					if (value == -INFINITY) {
						int temp = miniMax(newTypes, EnumUtils.numberToEnum<Flower.FlowerType>(-(int)turn));
						value = temp;
					}
					if (LogicUtils.PLAYERS_TYPE.Equals(turn) && value < move ||
						LogicUtils.COMPUTERS_TYPE.Equals(turn) && value > move) {
						move = value;
						index = i;
						//move = i;
					}
				}
			}
			return index;
		}*/
		
		private int alphaBeta(Flower.FlowerType[] board, StateManager.TurnType turn, int alpha, int beta) {
			/*int gameOver = isGameOver(board);
			if (gameOver != -INFINITY) {
				return gameOver;
			}*/
			int[] dummy;
			Flower.FlowerType winner;
			if (LogicUtils.isGameOver(board, out winner, out dummy)) {
				if (winner == LogicUtils.COMPUTERS_TYPE) {
					return 1;
				} else if (winner == LogicUtils.PLAYERS_TYPE) {
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
					/*if (StateManager.TurnType.Computers == turn) {
						cloned[i] = LogicUtils.PLAYERS_TYPE;
						score = alphaBeta(cloned, StateManager.TurnType.Computers, alpha, beta);
						if (score > alpha) {
							alpha = score;
						}
						if (alpha >= beta) {
							return alpha;
						}
					} else {
						cloned[i] = LogicUtils.COMPUTERS_TYPE;
						score = alphaBeta(cloned, StateManager.TurnType.Players, alpha, beta);
						if (score < beta) {
							beta = score;
						}
						if (alpha >= beta) {
							return beta;
						}
					}*/
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

		private int miniMax(Flower.FlowerType[] types, Flower.FlowerType turn) {
			int bestMove;
			int move;
			int[] dummy;
			Flower.FlowerType winner;
			if (LogicUtils.isGameOver(types, out winner, out dummy)) {
				if (winner == LogicUtils.COMPUTERS_TYPE) {
					return 1;
				} else if (winner == LogicUtils.PLAYERS_TYPE) {
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
				for (int i = 0; i < types.Length; i++) {
					if (types[i] == Flower.FlowerType.None) {// get valid moves
						cloned = LogicUtils.cloneFlowerTypes(types);
						if (LogicUtils.COMPUTERS_TYPE.Equals(turn)) {
							cloned[i] = LogicUtils.COMPUTERS_TYPE;
							move = miniMax(cloned, LogicUtils.PLAYERS_TYPE);
						} else {
							cloned[i] = LogicUtils.PLAYERS_TYPE;
							move = miniMax(cloned, LogicUtils.COMPUTERS_TYPE);
						}
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
		#endregion Tests
		/*private struct temp {
			public int index;
			public int score;

			public override string ToString() {
				return (string.Format("Index: {0} Score: {1}", index, score));
			}
		}*/
		public override int getMove(Flower[] board) {
			//int result = -1;
			int result = -2;
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
				//List<temp> possibilities = new List<temp>();
				Flower.FlowerType[] cloned;
				for (int i = 0; i < types.Length; i++) {
					if (types[i] == Flower.FlowerType.None) {
						cloned = LogicUtils.cloneFlowerTypes(types);
						cloned[i] = LogicUtils.COMPUTERS_TYPE;
						move = miniMax(cloned, LogicUtils.PLAYERS_TYPE);
							/*temp test = new temp();
							test.index = i;
							test.score = move;
							possibilities.Add(test);*/
						if (move > bestMove) {
							result = i;
							bestMove = move;
						}
					}
				}
				//int asdt23ra = possibilities.Count;
			}
			return result;
		}
		#endregion Support methods
	}
	#endregion Hard difficulty
}
