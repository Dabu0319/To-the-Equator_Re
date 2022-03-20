using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;

    [Header("移动参数")]
    public float speed = 8f;
    public float crouchSpeedDivisor = 3f;   //蹲伏移速除数

    [Header("跳跃参数")]
    public float jumpForce = 6.3f;  //跳跃力
    public float jumpHoldForce = 1.9f;  //长按跳跃力
    public float jumpHoldDuration = 0.1f;   //长按跳跃持续时间
    public float crouchJumpBoost = 2.5f;    //蹲伏跳跃加成
    public float hangingJumpForce = 15f;    //悬挂跳跃力

    float jumpTime; //配合Duration


    [Header("状态")]
    public bool isCrouch;
    public bool isOnGround;
    public bool isJump;
    public bool isHeadBlocked;
    public bool isHanging;

    [Header("环境检测")]
    //跳跃和触地的射线检测所需参数
    public float footOffset = 0.4f; //脚的偏移
    public float headClearance = 0.5f;  //头顶空隙
    public float groundDistance = 0.2f; //到地面的距离
    //悬挂的射线所需参数
    public float playerHeight; //角色高度
    public float eyeHeight = 1.5f; //眼睛高度
    public float grabDistance = 0.4f;  //悬挂时的离墙距离
    public float reachOffset = 0.7f;   //距离玩家一定距离的自上而下的判断（壁挂）射线的起点

    public LayerMask groundLayer;

    float xVelocity;

    //按键设置
    bool jumpPressed;   //单次按下跳跃
    bool jumpHeld;  //长按跳跃
    bool crouchHeld;    //长按下蹲
    bool crouchPressed; //单次按下下蹲

    //碰撞体各状态的尺寸和位置
    Vector2 colliderStandSize;  //站立尺寸
    Vector2 colliderStandOffset;    //站立位置
    Vector2 colliderCrouchSize; //蹲伏尺寸
    Vector2 colliderCrouchOffset;   //蹲伏位置

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();

        //playerHeight = coll.size.y;

        //记录站立时的碰撞体参数
        colliderStandSize = coll.size;
        colliderStandOffset = coll.offset;
        //蹲伏状态下的y减半
        colliderCrouchSize = new Vector2(coll.size.x, coll.size.y / 2f);
        //这里的/2是从中间开始相当于两边都减半, 有没有什么方法可以让从下面减半
        //colliderCrouchOffset = new Vector2(coll.offset.x, coll.offset.y / 2f);
        colliderCrouchOffset = new Vector2(coll.offset.x, coll.offset.y - coll.size.y/4f);;
        

        anim = GetComponentInParent<Animator>();

    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
        }
        
        jumpHeld = Input.GetButton("Jump"); 
        crouchHeld = Input.GetButton("Crouch");
        crouchPressed = Input.GetButtonDown("Crouch");
    }

    void FixedUpdate()
    {
        if (isJump)
        {
            jumpPressed = false;
        }
        PhysicsCheck(); //物理环境检查
        GroundMovement();   //地面运动
        MidAirMovemwnt();   //空中运动
    }
    
    //物理的环境检查并确定状态
    void PhysicsCheck()
    {
        //判断是否在地面上
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistance, groundLayer);
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance, groundLayer);
        
        if (leftCheck || rightCheck)
            isOnGround = true;
        else isOnGround = false;

        //判断头顶是否被阻挡
        RaycastHit2D headCheck = Raycast(new Vector2(0f, coll.size.y*0.5f), Vector2.up, headClearance, groundLayer);
        
        if (headCheck)
            isHeadBlocked = true;
        else isHeadBlocked = false;

        // //判断是否悬挂
        // float direction = transform.localScale.x;   //左右朝向
        // Vector2 grabDir = new Vector2(direction, 0f);   //射线方向
        //
        // RaycastHit2D blockedCheck = Raycast(new Vector2(footOffset * direction, playerHeight), grabDir, grabDistance, groundLayer); //头顶射线
        // RaycastHit2D wallCheck = Raycast(new Vector2(footOffset * direction, eyeHeight), grabDir, grabDistance, groundLayer); //眼睛射线
        // RaycastHit2D ledgeCheck = Raycast(new Vector2(reachOffset * direction, playerHeight), Vector2.down, grabDistance, groundLayer); //壁挂检测射线
        //
        // if (!isOnGround && rb.velocity.y < 0 && !blockedCheck && wallCheck && ledgeCheck)    //不在地面上、下落状态时、三线判断
        // {
        //     Vector3 pos = transform.position;   //记录原位置进行修改
        //
        //     pos.x += (wallCheck.distance - 0.05f) * direction;    //修改位置在固定位置上
        //     pos.y -= ledgeCheck.distance;
        //
        //     transform.position = pos;
        //
        //     rb.bodyType = RigidbodyType2D.Static;   //定住角色
        //     isHanging = true;
        // }
    }

    //地面(碰到Ground)上的相关运动
    void GroundMovement()   
    {
        //悬挂
        if (isHanging)
            return;

        //蹲伏和起身
        if (crouchHeld && !isCrouch && isOnGround)
            Crouch();
        else if (!crouchHeld && isCrouch && !isHeadBlocked)
            StandUp();
        else if (!isOnGround && isCrouch)
            StandUp();

        //水平的移动
        xVelocity = Input.GetAxis("Horizontal");
        //蹲伏速度
        if (isCrouch)
            xVelocity /= crouchSpeedDivisor;
        //正常速度
        rb.velocity = new Vector2(xVelocity * speed, rb.velocity.y);

        //走路动画
  
        //anim.SetFloat("XMove",Mathf.Abs(xVelocity));
        
        
        //角色朝向
        FlipDirection();
    }

    //空中的相关运动
    void MidAirMovemwnt()
    {
        if (isHanging)      //悬挂的后续操作
        {
            if (jumpPressed)    //悬挂跳跃
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                //rb.AddForce(new Vector2(0f, hangingJumpForce), ForceMode2D.Impulse);
                rb.velocity = new Vector2(rb.velocity.x, hangingJumpForce);
                isHanging = false;
            }
            if (crouchPressed)   //悬挂下落，注意使用单次按下，防止蹲跳悬挂的瞬间掉落
            {
                rb.bodyType = RigidbodyType2D.Dynamic;

                isHanging = false;
            }
        }
       
        if(jumpPressed && isOnGround && !isJump && !isHeadBlocked)    //跳跃
        {
            if (isCrouch)   //蹲跳
            {
                StandUp();
                rb.AddForce(new Vector2(0f, crouchJumpBoost), ForceMode2D.Impulse);
            }

            //isOnGround = false;物理的环境判断中即可
            isJump = true;

            //Time.time为实时的游戏时间
            jumpTime = Time.time + jumpHoldDuration;

            //给刚体添加一个脉冲力（瞬时力）
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
        else if (isJump)   
        {
            if (jumpHeld)   //长按跳跃
                rb.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);
            
            isJump = false;
        }

        // if (Mathf.Abs(rb.velocity.y) > 0)
        // {
        //     anim.SetBool("Jump",true);
        // }
        // else
        // {
        //     anim.SetBool("Jump",false);
        // }

    }

    void FlipDirection()    //角色朝向
    {
        if (xVelocity < 0)
            rb.transform.localScale = new Vector2(-1, 1);
        if (xVelocity > 0)
            rb.transform.localScale = new Vector2(1, 1);
    }

    void Crouch()   //蹲伏
    {
        isCrouch = true;
        //碰撞体变为蹲伏状态
        coll.size = colliderCrouchSize;
        coll.offset = colliderCrouchOffset;
    }

    void StandUp()  //起身
    {
        isCrouch = false;
        //碰撞体变为站立状态
        coll.size = colliderStandSize;
        coll.offset = colliderStandOffset;
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
}
