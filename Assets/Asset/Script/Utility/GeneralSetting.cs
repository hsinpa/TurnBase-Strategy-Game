using UnityEngine;
using System.Collections;


namespace Utility {
	public class GeneralSetting {
		//Tag 

		//Layer Setting
		public static int unitLayer = 1 << 8;
		public static int terrainLayer = 1 << 9;

		//Color
		public static Color32 player_color = new Color32(255, 255, 255, 255);
		public static Color32 enemy_color = new Color32(192, 57, 43, 255);
		public static Color32 friend_color = new Color32(46, 204, 113, 255);

		public static Color32 c_white = Color.white;
		public static Color32 dark_red = new Color32(192, 57, 43, 255);
		public static Color32 shallow_red = new Color32(231, 76, 60, 255);
		//

	}
}