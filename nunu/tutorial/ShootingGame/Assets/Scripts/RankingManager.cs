using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class RankingManager : MonoBehaviour {

	public GameObject rankingContainer;
	public RectTransform guiRankingNode;
	private List<RankingDataNode> rankingDataList = new List<RankingDataNode>();

	// Use this for initialization
	void Start () {
		TextAsset rankingDataText = Resources.Load ("rankingData") as TextAsset;
		
		//外部データ（ソート済みjsonファイル）からランキングデータList作成
		SetRankingData (rankingDataText.text);

		//Listを表示
		foreach (RankingDataNode node in rankingDataList) {
			var item = GameObject.Instantiate (guiRankingNode) as RectTransform;
			item.SetParent (rankingContainer.transform, false);

			var texts = item.GetComponentsInChildren<Text> ();
			texts[0].text = node.Rank.ToString ();
			texts[1].text = node.Name;
			texts[2].text = node.Score.ToString();
		}
	}
	


	//Jsonデータをパースしてセット///////////////////////////////////////////////
	public bool SetRankingData (string dataText) {

		Debug.Log (dataText);	//DEBUG: wwwの内容一覧表示
		var json = Json.Deserialize (dataText);

		List<object> list;
		if (json is List<object>) {
			list = json as List<object>;
		} else {
			return false;
		}

		int i = 0;
		foreach (Dictionary<string, object> node in list) {
			++i;
			RankingDataNode item = new RankingDataNode (-1, "", -1);
			item.Score = (int) (long) node["score"];
			item.Name = node["name"] as string;
			item.Rank = i;
			rankingDataList.Add (item);
		}
		return true;
	}
	////////////////////////////////////////////////////////////////////////////

	//ランキングデータ要素クラス
	private class RankingDataNode {
		public RankingDataNode (int rank, string name, int score) {
			Rank = rank;
			Score = score;
			Name = name;
		}

		private int rank;
		private int score;
		private string name;
		
		public int Rank {
			get { return rank; }
			set { rank = value; }
		}
		public int Score {
			get { return score; }
			set { score = value; }
		}
		public string Name {
			get { return name; }
			set { name = value; }
		}
	}

	//タイトルに戻る
	public void BackToTitle () {
		Application.LoadLevel ("Stage");
	}
}
