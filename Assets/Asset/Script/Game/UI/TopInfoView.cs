using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using UnityEngine.UI;

public class TopInfoView : MonoBehaviour {
	RectTransform statsHolder { get { return transform.Find("stats").GetComponent<RectTransform>(); } }
	Image avatar { get { return transform.Find("avatar").GetComponent<Image>(); } }

	public Unit currentUnit = null;

	public void SetCharacterInfo( Unit p_unit  ) {
		UtilityMethod.ClearChildObject(statsHolder);

		//p_unit.characterJSON;

		CreateTextTag("", p_unit.mCharacterPrefab._name );
		CreateTextTag("LV", p_unit.characterJSON.GetField("level").num.ToString());
		CreateTextTag("Atk", p_unit.characterJSON.GetField("strength").num.ToString());
		CreateTextTag("Def",  p_unit.characterJSON.GetField("defense").num.ToString() );

		CreateTextTag("Class", p_unit.mCharacterPrefab._class );
		CreateTextTag("Spd",  p_unit.characterJSON.GetField("speed").num.ToString() );
		CreateTextTag("Skl",  p_unit.characterJSON.GetField("skill").num.ToString() );
		CreateTextTag("FS",  p_unit.characterJSON.GetField("foot speed").num.ToString() );

		currentUnit = p_unit;
		avatar.color = new Color(1,1,1,1);
	}

	public void CreateTextTag(string p_title, string p_value) {
		GameObject prefab = Resources.Load<GameObject>("Prefab/UI/Game/Top/infoTag");
		GameObject generatedObject = UtilityMethod.CreateObjectToParent(statsHolder, prefab);

		generatedObject.transform.Find("title").GetComponent<Text>().text = p_title;
		generatedObject.transform.Find("field").GetComponent<Text>().text = p_value;

	}

	public void Close() {
		currentUnit = null;
		avatar.color = new Color(1,1,1,0);
		UtilityMethod.ClearChildObject(statsHolder);
	}

}
