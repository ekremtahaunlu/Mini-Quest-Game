using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private GameObject _cameraHolder;
    [SerializeField] private float _mouseSensitivity, _sprintSpeed, _walkSpeed, _jumpForce, _smoothTime;
    [SerializeField] private float _collectRange = 2f;

    private float _verticalLookRotation;
    private bool _grounded;
    private Vector3 _smoothMoveVelocity;
    private Vector3 _moveAmount;

    private Rigidbody _rigidbody;
    private PhotonView _photonView;
    private GameObject _lastCollectedObject;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        _photonView = GetComponent<PhotonView>();
    }

    private void Start() {
        if (_photonView.IsMine) return;
        Destroy(GetComponentInChildren<Camera>().gameObject);
        Destroy(_rigidbody);
    }

    private void Update() {
        if (!_photonView.IsMine) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            Collect(); // E tuşu ile toplama
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            DropLastCollected(); // R tuşu ile bırakma
        }

        Look();
        Move();
        Jump();
    }

    private void Look() {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * _mouseSensitivity);

        _verticalLookRotation += Input.GetAxisRaw("Mouse Y") * _mouseSensitivity;
        _verticalLookRotation = Mathf.Clamp(_verticalLookRotation, -90f, 90f);

        _cameraHolder.transform.localEulerAngles = Vector3.left * _verticalLookRotation;
    }

    private void Move() {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        _moveAmount = Vector3.SmoothDamp(_moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? _sprintSpeed : _walkSpeed), ref _smoothMoveVelocity, _smoothTime);
    }

    private void Jump() {
        if (Input.GetKeyDown(KeyCode.Space) && _grounded) {
            _rigidbody.AddForce(transform.up * _jumpForce);
        }
    }

    private void Collect() {
        if (_lastCollectedObject != null) {
            Debug.Log("Zaten bir obje topladınız.");
            return;
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _collectRange);
        foreach (Collider hitCollider in hitColliders) {
            if (hitCollider.CompareTag("Collectible")) {
                _lastCollectedObject = hitCollider.gameObject;
                _lastCollectedObject.SetActive(false); // Objeyi devre dışı bırak
                Debug.Log("Obje toplandı: " + _lastCollectedObject.name);
                return; // İlk uygun objeyi bulunca işlemi sonlandır
            }
        }

        Debug.Log("Toplanacak obje yok.");
    }

    private void DropLastCollected() {
        if (_lastCollectedObject == null) {
            Debug.Log("Bırakacak bir obje yok.");
            return;
        }

        Vector3 dropPosition = transform.position + transform.forward * 2f; // Objeyi oyuncunun önüne bırak
        _lastCollectedObject.transform.position = dropPosition;
        _lastCollectedObject.SetActive(true); // Objeyi tekrar aktif et

        Rigidbody rb = _lastCollectedObject.GetComponent<Rigidbody>();
        if (rb == null) {
            rb = _lastCollectedObject.AddComponent<Rigidbody>();
        }
        rb.isKinematic = false;
        rb.useGravity = true;

        Debug.Log("Obje bırakıldı: " + _lastCollectedObject.name);
        _lastCollectedObject = null; // Referansı sıfırla
    }

    public void SetGroundedState(bool grounded) {
        _grounded = grounded;
    }

    private void FixedUpdate() {
        if (!_photonView.IsMine) {
            return;
        }

        _rigidbody.MovePosition(_rigidbody.position + transform.TransformDirection(_moveAmount) * Time.fixedDeltaTime);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _collectRange);
    }
}
