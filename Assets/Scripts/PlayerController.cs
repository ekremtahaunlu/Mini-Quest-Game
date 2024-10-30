using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour 

    { 
        [SerializeField] private GameObject _cameraHolder; 
        [SerializeField] private float _mouseSensitivity, _sprintSpeed, _walkSpeed, _jumpForce, _smoothTime;
        
        private float _verticalLookRotation;
        private bool _grounded;
        private Vector3 _smoothMoveVelocity;
        private Vector3 _moveAmount;
        
        private Rigidbody _rigidbody;
        private PhotonView _photonView;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _photonView = GetComponent<PhotonView>();
        }
        
        private void Start()
        {
            if (_photonView.IsMine) return;
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(_rigidbody);
        }

        private void Update()
        {
            if (!_photonView.IsMine)
            {
                return;
            }
            
            Look();

            Move();

            Jump();
            
        }

        private void Look()
        {
            transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * _mouseSensitivity);

            _verticalLookRotation += Input.GetAxisRaw("Mouse Y") * _mouseSensitivity;
            _verticalLookRotation = Mathf.Clamp(_verticalLookRotation, -90f, 90f);

            _cameraHolder.transform.localEulerAngles = Vector3.left * _verticalLookRotation;
        }
        
        private void Move()
        {
            Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0 , Input.GetAxisRaw("Vertical")).normalized;

            _moveAmount = Vector3.SmoothDamp(_moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? _sprintSpeed : _walkSpeed), ref _smoothMoveVelocity, _smoothTime);
        }

        private void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && _grounded)
            {
                _rigidbody.AddForce(transform.up * _jumpForce);
            }
        }

        public void SetGroundedState(bool grounded)
        {
            _grounded = grounded;
            
        }

        private void FixedUpdate()
        { 
            if (!_photonView.IsMine)
            {
                return;
            }
            
            _rigidbody.MovePosition(_rigidbody.position + transform.TransformDirection(_moveAmount) * Time.fixedDeltaTime);
        }
    }