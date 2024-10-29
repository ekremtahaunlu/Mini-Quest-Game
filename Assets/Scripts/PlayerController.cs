using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviourPunCallbacks
{
	[SerializeField] private GameObject cameraHolder;

	[SerializeField] private float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;

	private float _verticalLookRotation;
	private bool _isGrounded;
	private Vector3 _smoothMoveVelocity;
	private Vector3 _moveAmount;

	private Rigidbody _rb;
	
	private PhotonView _pv;
	private PlayerManager _playerManager;

	private void Awake()
	{
		_rb = GetComponent<Rigidbody>();
		_pv = GetComponent<PhotonView>();
	}

	private void Start()
	{
		if (_pv.IsMine)
		{
			Destroy(GetComponentInChildren<Camera>().gameObject);
			Destroy(_rb);
		}
	}

	private void Update()
	{
		if(!_pv.IsMine)
			return;

		Look();
		Move();
		Jump();
	}

	private void Look()
	{
		transform.Rotate(Vector3.up * (Input.GetAxisRaw("Mouse X") * mouseSensitivity));

		_verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
		_verticalLookRotation = Mathf.Clamp(_verticalLookRotation, -90f, 90f);

		cameraHolder.transform.localEulerAngles = Vector3.left * _verticalLookRotation;
	}

	private void Move()
	{
		Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

		_moveAmount = Vector3.SmoothDamp(_moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref _smoothMoveVelocity, smoothTime);
	}

	private void Jump()
	{
		if(Input.GetKeyDown(KeyCode.Space) && _isGrounded)
		{
			_rb.AddForce(transform.up * jumpForce);
		}
	}

	public void SetGroundedState(bool _grounded)
	{
		_isGrounded = _grounded;
	}

	private void FixedUpdate()
	{
		if(!_pv.IsMine)
			return;

		_rb.MovePosition(_rb.position + transform.TransformDirection(_moveAmount) * Time.fixedDeltaTime);
	}
}