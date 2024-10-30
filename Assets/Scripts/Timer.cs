using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun;
using TMPro;

public class Timer : MonoBehaviour
{
	public TextMeshProUGUI timeText;
	private bool _count;
	public int timeLeft;
	private Hashtable _setTime = new();

	private void Start()
	{
		_count = true;
	}

	private void Update()
	{
		timeLeft = (int)PhotonNetwork.CurrentRoom.CustomProperties["Time"];
		float minutes = Mathf.FloorToInt(timeLeft / 60);
		float seconds = Mathf.FloorToInt(timeLeft % 60);

		timeText.text = $"{minutes:00}:{seconds:00}";

		if (PhotonNetwork.IsMasterClient)
		{
			if (_count)
			{
				_count = false;
				StartCoroutine(TimerCountdown());
			}
		}
	}

	IEnumerator TimerCountdown()
	{
		yield return new WaitForSeconds(1f);
		int nextTime = timeLeft - 1;
		if (nextTime >= 0)
		{
			_setTime["Time"] = nextTime;
			PhotonNetwork.CurrentRoom.SetCustomProperties(_setTime);
			_count = true;
		}
		else
		{
			Time.timeScale = 0f;
		}
	}
}