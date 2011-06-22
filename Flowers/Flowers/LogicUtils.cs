using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flowers {
	public static class LogicUtils {
		private const int CHECKS_WIDTH = 3;
		private const int CHECKS_HEIGHT = 8;
		public readonly static int[,] BOARD_LINE_CHECKS = new int[CHECKS_HEIGHT, CHECKS_WIDTH] {
				{0,1,2},// top row
				{0,3,6},// first column
				{0,4,8},// top left to bottom right
				{2,4,6},// top right to bottom left
				{3,4,5},// middle row
				{6,7,8},// bottom row
				{1,4,7},// second column
				{2,5,8},// last column
		};
		public const Flower.FlowerType PLAYERS_TYPE = Flower.FlowerType.Daisy;
		public const Flower.FlowerType COMPUTERS_TYPE = Flower.FlowerType.Rose;

		public static bool checkBoardsState(Flower.FlowerType[] board, Flower.FlowerType flowerType, out int[] winningIndexes) {
			bool foundMatch = false;
			winningIndexes = null;
			for (int i = 0; i < CHECKS_HEIGHT; i++) {
				if (board[BOARD_LINE_CHECKS[i, 0]] == flowerType && board[BOARD_LINE_CHECKS[i, 1]] == flowerType && board[BOARD_LINE_CHECKS[i, 2]] == flowerType) {
					foundMatch = true;
					winningIndexes = new int[CHECKS_WIDTH];
					for (int j = 0; j < CHECKS_WIDTH; j++) {
						winningIndexes[j] = BOARD_LINE_CHECKS[i, j];
					}
					break;
				}
			}
			return foundMatch;
		}

		public static bool isGameOver(Flower[] board, out Winner winner) {
			return isGameOver(getFlowerTypes(board), out winner);
		}

		public static bool isGameOver(Flower.FlowerType[] board, out Winner winner) {
			bool gameOver = false;
			// first check if anyone has won
			if (checkBoardsState(board, Flower.FlowerType.Daisy, out winner.winningIndexes)) {
				gameOver = true;
				winner.winningType = Flower.FlowerType.Daisy;
			} else if (checkBoardsState(board, Flower.FlowerType.Rose, out winner.winningIndexes)) {
				gameOver = true;
				winner.winningType = Flower.FlowerType.Rose;
			} else {
				winner.winningType = Flower.FlowerType.None;
				winner.winningIndexes = null;
			}

			// if we haven't found a win check for cats game
			if (!gameOver) {
				gameOver = true;
				for (int i = 0; i < board.Length; i++) {
					if (board[i] == Flower.FlowerType.None) {
						gameOver = false;
						break;
					}
				}
			}
			return gameOver;
		}

		public static bool isWinOrBlockAvailable(Flower[] board, Flower.FlowerType searchFor, ref int move) {
			bool canWin = false;
			int positives;
			int neutrals;
			int neutralsMove = -1;
			for (int y = 0; y < CHECKS_HEIGHT; y++) {
				positives = 0;
				neutrals = 0;
				for (int x = 0; x < CHECKS_WIDTH; x++) {
					if (board[BOARD_LINE_CHECKS[y, x]].Type == searchFor) {
						positives++;
					} else if (board[BOARD_LINE_CHECKS[y, x]].Type == Flower.FlowerType.None) {
						neutralsMove = BOARD_LINE_CHECKS[y, x];
						neutrals++;
					}
				}
				if (positives == 2 && neutrals == 1) {
					move = neutralsMove;
					canWin = true;
					break;
				}
			}
			return canWin;
		}

		public static Flower.FlowerType[] getFlowerTypes(Flower[] board) {
			Flower.FlowerType[] types = new Flower.FlowerType[board.Length];
			for (int i = 0; i < board.Length; i++) {
				types[i] = board[i].Type;
			}
			return types;
		}

		public static Flower.FlowerType[] cloneFlowerTypes(Flower.FlowerType[] oldTypes) {
			Flower.FlowerType[] newTypes = new Flower.FlowerType[oldTypes.Length];
			for (int i = 0; i < oldTypes.Length; i++) {
				newTypes[i] = oldTypes[i];
			}
			return newTypes;
		}

		public static Flower.FlowerType translateTurnToFlowerType(StateManager.TurnType turn) {
			Flower.FlowerType type = Flower.FlowerType.None;
			if (turn == StateManager.TurnType.Computers) {
				type = COMPUTERS_TYPE;
			} else if (turn == StateManager.TurnType.Players) {
				type = PLAYERS_TYPE;
			}
			return type;
		}
	}
}
