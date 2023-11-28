using UnityEngine;
using UnityEngine.UI;

public class Player : Photon.MonoBehaviour
{
    public PhotonView photon;
    public Rigidbody2D rb;
    public Animator animator;
    public GameObject playerCamera;
    public SpriteRenderer sr;
    public Text playerNameText;

    public bool isGrounded = false;
    public float moveSpeed;
    public float jumpForce;

    private int halfScreenWidth;

    private void Awake()
    {
        if (photon.isMine)
        {
            playerCamera.SetActive(true);
            Camera.main.GetComponent<AudioListener>().enabled = false;

            playerNameText.text = PhotonNetwork.playerName;
        }
        else
        {
            playerNameText.text = photonView.owner.name;
            playerNameText.color = Color.cyan;
        }

        halfScreenWidth = Screen.width / 2;
    }

    private void Update()
    {
        if (photon.isMine)
            CheckInput();
    }

    private void CheckInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.position.x >= halfScreenWidth)
                transform.position += moveSpeed * Time.deltaTime * Vector3.right;
            else if (touch.position.x < halfScreenWidth)
                transform.position += moveSpeed * Time.deltaTime * Vector3.left;

            if (touch.position.x < halfScreenWidth)
                photonView.RPC("FlipTrue", PhotonTargets.AllBuffered);

            if (touch.position.x >= halfScreenWidth)
                photonView.RPC("FlipFalse", PhotonTargets.AllBuffered);

            if ((touch.position.x < halfScreenWidth && touch.phase != TouchPhase.Ended) || (touch.position.x >= halfScreenWidth && touch.phase != TouchPhase.Ended))
                animator.SetBool("isRunning", true);
            else
                animator.SetBool("isRunning", false);
        }
        else if (Application.isMobilePlatform == false)
        {
            var move = new Vector3(Input.GetAxisRaw("Horizontal"), 0);
            transform.position += moveSpeed * Time.deltaTime * move;

            if (Input.GetKeyDown(KeyCode.A))
                photonView.RPC("FlipTrue", PhotonTargets.AllBuffered);
            else if (Input.GetKeyDown(KeyCode.D))
                photonView.RPC("FlipFalse", PhotonTargets.AllBuffered);

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                animator.SetBool("isRunning", true);
            else
                animator.SetBool("isRunning", false);
        }
    }

    [PunRPC] private void FlipTrue() => sr.flipX = true;

    [PunRPC] private void FlipFalse() => sr.flipX = false;
}
