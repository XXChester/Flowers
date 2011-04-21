using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flowers {
	public interface IDifficulty {
		int getMove(Flower[] board);
	}
}
