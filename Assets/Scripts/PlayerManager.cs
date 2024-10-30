using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
	private PhotonView _pv;

	private void Awake()
	{
		_pv = GetComponent<PhotonView>();
	}

	private void Start()
	{
		if (_pv.IsMine)
		{
			CreateController();
		}
	}

	private void CreateController()
	{
		PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), Vector3.up * 1.1f, Quaternion.identity);
	}
}