using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
	public GameObject LevelCamera = null;
	public bool offlineMode = false;
	SpawnPoint[] spawnPoints = null;

	void Start()
	{
		spawnPoints = GameObject.FindObjectsOfType<SpawnPoint>();
		Connect();
	}

	void Connect()
	{
		if(offlineMode)
		{
			PhotonNetwork.offlineMode = true;
			OnJoinedLobby();
		}
		else
		{
			PhotonNetwork.ConnectUsingSettings("DimensionBusters 0.0.1");
		}
	}

	void OnGUI()
	{
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
	}

	void OnJoinedLobby()
	{
		Debug.Log("Joined Lobby");
		PhotonNetwork.JoinRandomRoom();
	}

	void OnPhotonRandomJoinFailed()
	{
		Debug.Log("Creating Room");
		PhotonNetwork.CreateRoom(null);
	}

	void OnJoinedRoom()
	{
		SpawnMyPlayer();
	}

	void SpawnMyPlayer()
	{
		// find spawn point
		if(spawnPoints == null)
		{
			Debug.LogError("No spawnpoints in level");
		}
		SpawnPoint spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

		// instantiate local player and enable relevant components
		GameObject player = (GameObject)PhotonNetwork.Instantiate("PlayerController", spawnPoint.transform.position, spawnPoint.transform.rotation, 0);
		((MonoBehaviour)player.GetComponent("CharacterMotor")).enabled = true;
		((MonoBehaviour)player.GetComponent("FPSInputController")).enabled = true;
		((MonoBehaviour)player.GetComponent("MouseLook")).enabled = true;
		((MonoBehaviour)player.GetComponent("PlayerShooting")).enabled = true;
		player.transform.FindChild("Camera").gameObject.SetActive(true);

		//disable level camera
		LevelCamera.SetActive(false);
	}
}
