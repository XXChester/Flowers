using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flowers {
	public static class LogicUtils {
		private const int CHECKS_WIDTH = 3;
		private const int CHECKS_HEIGHT = 8;
		private readonly static int[,] BOARD_LINE_CHECKS = new int[CHECKS_HEIGHT, CHECKS_WIDTH] {
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

		public static bool checkBoardsState(Flower[] board, Flower.FlowerType flowerType, out int[] winningIndexes) {
			bool foundMatch = false;
			winningIndexes = null;
			for (int i = 0; i < CHECKS_HEIGHT; i++) {
				if (board[BOARD_LINE_CHECKS[i, 0]].Type == flowerType && board[BOARD_LINE_CHECKS[i, 1]].Type == flowerType && board[BOARD_LINE_CHECKS[i, 2]].Type == flowerType) {
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

		public static bool isGameOver(Flower[] board, out Flower.FlowerType winningType, out int[] winningIndexes) {
			bool gameOver = false;
			// first check if anyone has won
			if (checkBoardsState(board, Flower.FlowerType.Daisy, out winningIndexes)) {
				gameOver = true;
				winningType = Flower.FlowerType.Daisy;
			} else if (checkBoardsState(board, Flower.FlowerType.Rose, out winningIndexes)) {
				gameOver = true;
				winningType = Flower.FlowerType.Rose;
			} else {
				winningType = Flower.FlowerType.None;
				winningIndexes = null;
			}

			// if we haven't found a win check for cats game
			if (!gameOver) {
				gameOver = true;
				for (int i = 0; i < board.Length; i++) {
					if (board[i].Type == Flower.FlowerType.None) {
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
	}
}
