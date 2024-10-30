using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun;
using TMPro;

public class Timer : MonoBehaviour
{
	public TextMeshProUGUI time;
	public bool count;
	public int Time;
	private Hashtable _setTime = new ();

	private void Start()
	{
		count = true;
	}

	private void Update()
	{
		Time = (int)PhotonNetwork.CurrentRoom.CustomProperties["Time"];
		float minutes = Mathf.FloorToInt((int)PhotonNetwork.CurrentRoom.CustomProperties["Time"] / 60);
		float seconds = Mathf.FloorToInt((int)PhotonNetwork.CurrentRoom.CustomProperties["Time"] % 60);
		
		time.text = $"{minutes:00}:{seconds:00}";

		if (PhotonNetwork.IsMasterClient)
		{
			if (count)
			{
				count = false;
				StartCoroutine(timer());
			}
		}
	}

	IEnumerator timer()
	{
		yield return new WaitForSeconds(1f);
		int nextTime = Time -= 1;
		_setTime["Time"] = nextTime;
		PhotonNetwork.CurrentRoom.SetCustomProperties(_setTime);
		//PhotonNetwork.CurrentRoom.CustomProperties["Time"] = nextTime;
		count = true;
	}
}