using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using GWNorthEngine.Audio;
using GWNorthEngine.Audio.Params;
namespace Flowers {
	public class SoundManager {
		#region Class variables
		// singleton variable
		private static SoundManager instance = new SoundManager();
		#endregion Class variables

		#region Class properties
		public SFXEngine SFXEngine { get; set; }
		#endregion Class properties

		#region Constructor
		public SoundManager() {
			SFXEngineParams parms = new SFXEngineParams();
			parms.Muted = false;
			this.SFXEngine = new SFXEngine(parms);
		}
		#endregion Constructor

		#region Support methods
		public static SoundManager getInstance() {
			return instance;
		}

		public void update() {
			this.SFXEngine.update();
		}
		#endregion Support methods

		#region Destructor
		public void dispose() {
			this.SFXEngine.dispose();
		}
		#endregion Destructor
	}
}
