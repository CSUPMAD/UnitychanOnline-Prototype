using UnityEngine;
using System.Collections;

public class NetworkManager : Photon.MonoBehaviour {
	void Awake () {
		// Server接続
		PhotonNetwork.ConnectUsingSettings("v0.1");
		if (string.IsNullOrEmpty(PhotonNetwork.playerName)) {    
			PhotonNetwork.playerName = "Guest" + Random.Range(1, 20);      
		}       

	}
	// Lobby参加OK時
	void OnJoinedLobby() {
		// ランダムにRoom参加
		PhotonNetwork.JoinRandomRoom();
	}
	// Room参加NG時
	void OnPhotonRandomJoinFailed() {
		Debug.Log("Room参加失敗！");
		// 名前なしRoom作成
		PhotonNetwork.CreateRoom(null);
	}
	// Room参加OK時
	void OnJoinedRoom() {
		Debug.Log("Room参加成功！");
		//プレイヤーをインスタンス化
		Vector3 spawnPosition = new Vector3 (Random.Range (-2, 2), 5, 0); //生成位置
		var player = PhotonNetwork.Instantiate ("unitychanPrefab", spawnPosition, Quaternion.identity, 0);
		//player.GetComponent<myThirdPersonController>().isControllable = true;
		player.GetComponent<ThirdPersonController>().isControllable = true;
		//player.GetComponent<ThirdPersonCamera2>().enabled = true;
		player.GetComponent<ThirdPersonCamera>().enabled = true;
		//player.GetComponent<FollowCamera>().enabled = true;
	}

	// GUI表示
	void OnGUI() {
		// Photon接続状態
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
		GUILayout.Label(PhotonNetwork.playerName);
	}
}
