
namespace Flowers {
	public class StateManager {
		#region Enums
		public enum GameState {
			MainMenu,
			InGameMenu,
			InitEasyGame,
			InitModerateGame,
			InitHardGame,
			InitImpossibleGame,
			Active,
			InitGameOver,
			GameOver,
			ShutDown,
		};

		public enum TransitionState {
			None,
			TransitionIn,
			TransitionOut
		}

		public enum TurnType {
			None,
			Players,
			Computers
		};
		#endregion Enums

		#region Class variables
		// singleton instance
		private static StateManager instance = new StateManager();
		private TurnType turnType;
		private Difficulty activeDifficulty;
		private GameState currentState;
		#endregion Class variables

		#region Class properties
		public TurnType WhosTurnIsIt { get { return this.turnType; } set { this.turnType = value; } }
		public Difficulty ActiveDifficulty { get { return this.activeDifficulty; } set { this.activeDifficulty = value; } }
		public TransitionState CurrentTransitionState { get; set; }
		public TransitionState PreviousTransitionState { get; set; }
		public GameState PreviousState { get; set; }
		public GameState CurrentState {
			get { return this.currentState; }
			set {
				this.PreviousState = this.currentState;
				if (value == GameState.Active) {
					this.Winner = new Winner();
				}
				if (value == GameState.InitEasyGame) {
					this.activeDifficulty = new EasyDifficulty();
					this.currentState = GameState.Active;
					this.PreviousState = GameState.MainMenu;
				} else if (value == GameState.InitModerateGame) {
					this.activeDifficulty = new ModerateDifficulty();
					this.PreviousState = GameState.MainMenu;
					this.currentState = GameState.Active;
				} else if (value == GameState.InitHardGame) {
					this.activeDifficulty = new HardDifficulty();
					this.PreviousState = GameState.MainMenu;
					this.currentState = GameState.Active;
				} else if (value == GameState.InitImpossibleGame) {
					this.activeDifficulty = new ImpossibleDifficulty();
					this.PreviousState = GameState.MainMenu;
					this.currentState = GameState.Active;
				} else {
					this.currentState = value;
				}
			}
		}
		public Winner Winner { get; set; }
		#endregion Class properties

		#region Constructor
		public StateManager() {
			this.CurrentState = GameState.MainMenu;
			this.CurrentTransitionState = TransitionState.TransitionIn;
		}
		#endregion Constructor

		#region Support methods
		public static StateManager getInstance() {
			return instance;
		}
		#endregion support methods
	}
}
