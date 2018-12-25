using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float gravity;
    public float jumpSpeed;
    public Vector2 moveVerctor;
    public bool isOriginRight;
    public bool isGrounded;
    public float raycastDistance;
    public LayerMask groundedLayerMask;

    protected readonly int m_HashHorizontalSpeedPara = Animator.StringToHash("HorizontalSpeed");
    protected readonly int m_HashGroundedPara = Animator.StringToHash("Grounded");
    protected Animator m_Animator;
    protected CapsuleCollider2D m_Capsule;
    protected Rigidbody2D m_Rigidbody2D;
    public SpriteRenderer spriteRenderer;

    Vector2 m_PreviousPosition;
    Vector2 m_CurrentPosition;
    Vector2 m_NextMovement;
    ContactFilter2D contactFilter;

    public PlayerInput playerInput;

    // Start is called before the first frame update
    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_Capsule = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        contactFilter.layerMask = groundedLayerMask;
        contactFilter.useLayerMask = true;
        contactFilter.useTriggers = false;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateFacing();
        moveVerctor.x = PlayerInput.Instance.Horizontal.Value * moveSpeed;
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            moveVerctor += new Vector2(0, jumpSpeed);
        }
        CheckCapsuleEndCollisions(true);
        m_Animator.SetFloat(m_HashHorizontalSpeedPara, moveVerctor.x);
        m_Animator.SetBool(m_HashGroundedPara, isGrounded);
    }

    private void FixedUpdate()
    {
        if (!isGrounded)
        {
            moveVerctor += new Vector2(0, gravity);
        }
        else if (moveVerctor.y < 0)
        {
            moveVerctor.y = 0;
        }
        m_NextMovement = moveVerctor * Time.deltaTime;
        m_PreviousPosition = m_Rigidbody2D.position;
        m_CurrentPosition = m_PreviousPosition + m_NextMovement;
        m_NextMovement = Vector2.zero;
        m_Rigidbody2D.MovePosition(m_CurrentPosition);
    }

    public void CheckCapsuleEndCollisions(bool bottom = true)
    {
        Vector2 raycastDirection = Vector2.down;
        Vector2 raycastStart;
        raycastStart = m_Rigidbody2D.position;
        RaycastHit2D[] raycastHit = new RaycastHit2D[12];
        if (bottom)
        {
            Debug.DrawRay(raycastStart, raycastDirection);
            if (Physics2D.Raycast(raycastStart, raycastDirection, contactFilter, raycastHit, raycastDistance) > 0)
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
        else
        {

        }
    }

    public void UpdateFacing()
    {
        bool faceLeft = PlayerInput.Instance.Horizontal.Value < 0f;
        bool faceRight = PlayerInput.Instance.Horizontal.Value > 0f;

        if (faceLeft)
        {
            spriteRenderer.flipX = isOriginRight;
        }
        else if (faceRight)
        {
            spriteRenderer.flipX = !isOriginRight;
        }
    }
}
