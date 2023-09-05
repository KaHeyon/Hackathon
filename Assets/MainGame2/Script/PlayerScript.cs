using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    private int jump=0;
    public GameObject UIManager;

    Vector3 targetPosition = new Vector3(-8.0f, 0.7f, 0.0f);
    float yThreshold = -12.0f;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Move by control
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        //Max Speed
        if (rigid.velocity.x > maxSpeed)//right max speed
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1))//left max speed
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

        //stop speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        //Direction Sprite
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        //walking animation
        if (Mathf.Abs(rigid.velocity.x) < 0.3)
            anim.SetBool("walking", false);
        else
            anim.SetBool("walking", true);

        //jump
        if (Input.GetButtonDown("Jump"))
        {

            if (jump < 2)
            {
                jump++;
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                anim.SetBool("jumping", true);
                

            }
            
        }

        //Landing Platform
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.3f) 
                {
                    anim.SetBool("jumping", false);
                    jump = 0;
                }
                    
            }
        }

        //떨어지면 제자리
        if (transform.position.y <= yThreshold)
        {
            transform.position = targetPosition;
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
        
            GameManager.instance.loseHP();
            UIManager.GetComponent<UIManager>().minusHP();
            OnDamaged(collision.transform.position);
            timeDelay();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
       
            //point
            GameManager.instance.addHP();
            UIManager.GetComponent<UIManager>().addHP();

            //Deactive Item
            collision.gameObject.SetActive(false);
            timeDelay();
        }
        else if (collision.gameObject.tag == "Star")
        {
            timeDelay();
            GameManager.instance.addStar();
            UIManager.GetComponent<UIManager>().addStar();
            timeDelay();
        }
        else if (collision.gameObject.tag == "Finish")
        {
            Debug.Log("collision");
            SceneManager.LoadScene("map3");
        }
    }

    void OnDamaged(Vector2 targetPos)
    {
        //Change Layer (Immortal Active)
        gameObject.layer = 10;
        //View Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        //Reaction Force
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 5, ForceMode2D.Impulse);

        //animation
        anim.SetTrigger("damaged");

        Invoke("OffDamaged", 3);

    }

    void OffDamaged()
    {
        gameObject.layer = 9;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    IEnumerator timeDelay()
    {
        yield return new WaitForSeconds(1f);
    }
}