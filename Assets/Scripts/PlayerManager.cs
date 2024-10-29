using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
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
		if(_pv.IsMine)
		{
			CreateController();
		}
	}

	private static void CreateController()
	{
		PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), Vector3.zero, Quaternion.identity);
	}

	public static PlayerManager Find(Player player)
	{
		return FindObjectsOfType<PlayerManager>().SingleOrDefault(x => Equals(x._pv.Owner, player));
	}
}