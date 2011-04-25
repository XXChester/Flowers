using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flowers {
	public class StateManager {
		#region Enums
		public enum GameState {
			
		};

		public enum TurnType {
			None,
			PlayerOnes,
			PlayerTwos
		};

		public enum Difficulty {
			None,
			Easy,
			Moderate,
			Hard
		}
		#endregion Enums

		#region Class variables
		// singleton instance
		private static StateManager instance = new StateManager();
		private TurnType turnType;
		#endregion Class variables

		#region Class properties
		public TurnType WhosTurnIsIt { get { return this.turnType; } set { this.turnType = value; } }
		#endregion Class properties

		#region Constructor
		public StateManager() {
			this.turnType = TurnType.PlayerOnes;
		}
		#endregion Constructor

		#region Support methods
		public static StateManager getInstance() {
			return instance;
		}
		#endregion support methods
	}
}
