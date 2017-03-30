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

		public static Color32 shallow_yellow = new Color32(241, 196, 15, 255);
		public static Color32 dark_yellow = new Color32(243, 156, 18, 255);
		//rgb(241, 196, 15)
		
	}
}