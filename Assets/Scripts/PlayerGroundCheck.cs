using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
	private PlayerController _playerController;

	private void Awake()
	{
		_playerController = GetComponentInParent<PlayerController>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject == _playerController.gameObject)
			return;

		_playerController.SetGroundedState(true);
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.gameObject == _playerController.gameObject)
			return;

		_playerController.SetGroundedState(false);
	}

	private void OnTriggerStay(Collider other)
	{
		if(other.gameObject == _playerController.gameObject)
			return;

		_playerController.SetGroundedState(true);
	}
}