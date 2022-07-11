using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    //Type: 0-Player, 1-PlayerShadow
    public int playerType;

    public bool moveEnabled = true;
    
    private Rigidbody2D rb;
    private CircleCollider2D coll;
    private Animator anim;

    [Header("移动参数")]
    public float speed = 8f;
    public float crouchSpeedDivisor = 3f;   //蹲伏移速除数
    public float skiingSpeedMultiplier = 2f;

    [Header("跳跃参数")]
    public float jumpForce = 6.3f;  //跳跃力
    public float jumpHoldForce = 1.9f;  //长按跳跃力
    public float jumpHoldDuration = 0.1f;   //长按跳跃持续时间
    //public float crouchJumpBoost = 2.5f;    //蹲伏跳跃加成
    //public float hangingJumpForce = 15f;    //悬挂跳跃力

    float jumpTime; //配合Duration


    [Header("状态")]
    public bool isCrouch;
    public bool isOnGround;
    public bool isJump;
    public bool isHeadBlocked;
    public bool isPushing;
    public bool isPulling;
    public bool isInShelter;
    public bool isSkiing;

    [Header("环境检测")]
    //跳跃和触地的射线检测所需参数
    public float footOffset = 0.4f; //脚的偏移
    public float headClearance = 0.5f;  //头顶空隙
    public float groundDistance = 0.2f; //到地面的距离
    //推箱子
    public float interactDistance = 0.4f;
    public float interactHeight = 0.5f;


    public LayerMask groundLayer;
    public LayerMask slopeLayer;

    float xVelocity;

    //按键设置
    bool jumpPressed;   //单次按下跳跃
     bool jumpHeld;  //长按跳跃
    bool crouchHeld;    //长按下蹲
    bool crouchPressed; //单次按下下蹲
    private bool interactPressed;
    private bool interactHeld;
    private bool interactRelease;

    //碰撞体各状态的尺寸和位置
    Vector2 colliderStandSize;  //站立尺寸
    Vector2 colliderStandOffset;    //站立位置
    Vector2 colliderCrouchSize; //蹲伏尺寸
    Vector2 colliderCrouchOffset;   //蹲伏位置
    
    //交互
    public GameObject box;
    
    //复活点
    [field:SerializeField]
    private UnityEvent OnRespawnRequired { get; set; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CircleCollider2D>();
        //anim = GetComponentInParent<Animator>();

    }

    void Update()
    {
        //加这个主要是解决有时候update和fixedupdate之间的gap导致无法jump, 后面moveenabled是为了解决转换之后自动跳一下
        if (ControlFreak2.CF2Input.GetButtonDown("Jump") && moveEnabled)
        {
            jumpPressed = true;
        }
        
        jumpHeld = ControlFreak2.CF2Input.GetButton("Jump"); 
        crouchHeld = ControlFreak2.CF2Input.GetButton("Crouch");
        crouchPressed = ControlFreak2.CF2Input.GetButtonDown("Crouch");
        interactPressed = ControlFreak2.CF2Input.GetKeyDown(KeyCode.E);
        //interactHeld = Input.GetKey(KeyCode.E);
        interactRelease = ControlFreak2.CF2Input.GetKeyUp(KeyCode.E);

        #region 推箱子
        
         //判断面前是否有箱子
        //之前放在fixedupdate里经常会有检测不到的情况, 后面放到update里面就没问题了, 猜测应该是input的输入检测问题
        float direction = transform.localScale.x;
        Vector2 grabDir = new Vector2(direction, 0f);
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D boxCheck = Raycast(new Vector2(footOffset * direction, interactHeight), grabDir, interactDistance, groundLayer);

        
        
        switch (playerType)
        {
            //Player
            case 0:
                if (boxCheck && boxCheck.collider.CompareTag("Box") && ControlFreak2.CF2Input.GetKey(KeyCode.E) )
                {
                    isPulling = true;
                    //有时候不松开是因为直接改变了box目标
                    if (box != null)
                    {
                        box.GetComponent<FixedJoint2D>().enabled = false;
                    }
                    box = boxCheck.collider.gameObject;

                    box.GetComponent<FixedJoint2D>().enabled = true;
                    box.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
                }
                else if(ControlFreak2.CF2Input.GetKeyUp(KeyCode.E)&& box != null )
                {
                    if(box.GetComponent<FixedJoint2D>().enabled == true)
                        box.GetComponent<FixedJoint2D>().enabled = false;
                    isPulling = false;
                    
                }
                else if (!isPulling && box != null && box.GetComponent<FixedJoint2D>().enabled == true)
                {
                    box.GetComponent<FixedJoint2D>().enabled = false;
                }

                break;
            //Player Shadow
            case 1:
                if (boxCheck && boxCheck.collider.CompareTag("BoxShadow") && ControlFreak2.CF2Input.GetKey(KeyCode.E))
                {
                    isPulling = true;
                    
                    box = boxCheck.collider.gameObject;

                    box.GetComponent<FixedJoint2D>().enabled = true;
                    box.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
                }
                else if(ControlFreak2.CF2Input.GetKeyUp(KeyCode.E)&& box != null )
                {
                    if(box.GetComponent<FixedJoint2D>().enabled) 
                        box.GetComponent<FixedJoint2D>().enabled = false;
                    isPulling = false;
                }

                else if (!isPulling && box != null && box.GetComponent<FixedJoint2D>().enabled == true)
                {
                    box.GetComponent<FixedJoint2D>().enabled = false;
                }
                break;
            
        }
        
        #endregion
        
        
        // //风力
        // if (GameManager.instance.gameStatus == GameStatus.Wind)
        // {
        //     if (!isInShelter)
        //     {
        //         rb.AddForce(Vector2.left*GameManager.instance.windForce,ForceMode2D.Force);
        //     }
        //
        // }
    }

    void FixedUpdate()
    {

        if (isJump)
        {
            jumpPressed = false;
        }
        
        PhysicsCheck(); //物理环境检查


        if (moveEnabled)
        {


            GroundMovement();   //地面运动
            MidAirMovemwnt();   //空中运动
        }

    }
    
    //物理的环境检查并确定状态
    void PhysicsCheck()
    {
        //判断是否在地面上
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistance, groundLayer);
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance, groundLayer);

        if (leftCheck || rightCheck)
        {
            isOnGround = true;
            //anim.SetBool("isJumping",false);
            //AnimationController_Kanon.Instance.SetGrounded(true);
            isSkiing = false;
        }



        else
        {
            isOnGround = false;
            //AnimationController_Kanon.Instance.SetGrounded(false);
        }
        

        //判断头顶是否被阻挡
        RaycastHit2D headCheck = Raycast(new Vector2(0f, headClearance*0.5f), Vector2.up, headClearance, groundLayer);
        
        if (headCheck)
            isHeadBlocked = true;
        else isHeadBlocked = false;
        
        //判断是否在斜坡上
        RaycastHit2D leftSlopeCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistance, slopeLayer);
        RaycastHit2D rightSlopeCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance, slopeLayer);

        if (leftSlopeCheck || rightSlopeCheck)
        {
            isSkiing = true;

        }

        
        if (isSkiing)
        {
            rb.velocity = new Vector2(speed * skiingSpeedMultiplier, rb.velocity.y);
            if (rb.velocity.x < 0)
                rb.transform.localScale = new Vector2(-1, 1);
            if (rb.velocity.x > 0)
                rb.transform.localScale = new Vector2(1, 1);
            
            //水平的移动
            xVelocity = ControlFreak2.CF2Input.GetAxis("Horizontal");
            //角色朝向
            FlipDirection();

            
        }
        //else isSkiing = false;
        

        FlipDirection();
        
    }

    //地面(碰到Ground)上的相关运动
    public void GroundMovement()   
    {
        if(isSkiing )
            return;

        if (GameManager.instance.gameStatus == GameStatus.Wind &&  !isInShelter &&this.CompareTag("Player"))
        {
            rb.velocity = new Vector2(-GameManager.instance.windForce, rb.velocity.y);
        }
        else
        {
            //水平的移动
            xVelocity = ControlFreak2.CF2Input.GetAxis("Horizontal");
            //蹲伏速度
            if (isCrouch)
                xVelocity /= crouchSpeedDivisor;
            //正常速度
            rb.velocity = new Vector2(xVelocity * speed, rb.velocity.y);

            //走路动画
            //AnimationController_Kanon.Instance.PlayMoveAni(Mathf.Abs(xVelocity));
            //anim.SetFloat("speed",Mathf.Abs(xVelocity));
        
        
            //角色朝向
            FlipDirection();
        }
            
        
        
        
    }

    //空中的相关运动
    void MidAirMovemwnt()
    {

        
        if(jumpPressed && isOnGround && !isJump && !isHeadBlocked && moveEnabled)    //跳跃
        {

            
            
            isJump = true;
            //AnimationController_Kanon.Instance.PlayJumpAni();
            //anim.SetBool("isJumping",true);

            //Time.time为实时的游戏时间
            jumpTime = Time.time + jumpHoldDuration;

            //给刚体添加一个脉冲力（瞬时力）
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            
            
        }
        else if (isJump)   
        {
            // if (jumpHeld)   //长按跳跃
                // rb.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);

            if (jumpTime<Time.time)
            {
                isJump = false;
            }


        }
        //
        // Debug.Log(rb.velocity.y);
        // if (Mathf.Abs(rb.velocity.y) > 0)
        // {
        //     anim.SetBool("isJumping",true);
        // }
        // else
        // {
        //     anim.SetBool("isJumping",false);
        // }

    }

    void FlipDirection()    //角色朝向
    {
        if (xVelocity < 0)
            rb.transform.localScale = new Vector2(-1, 1);
        if (xVelocity > 0)
            rb.transform.localScale = new Vector2(1, 1);
    }



    //重写 Raycast 方法：主要是参数方面和添加了画线
    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask layer)
    {
        Vector2 pos = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, layer);

        Color color = hit ? Color.red : Color.green;    //根据是否碰撞更改画线的颜色
        Debug.DrawRay(pos + offset, rayDirection * length, color);

        return hit;
    }

    internal void PlayerDied()
    {
        OnRespawnRequired?.Invoke();
    }
}
