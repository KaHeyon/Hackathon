using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    public GameObject sceneM;
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    private int jump = 0;
    [SerializeField]
    public GameObject UIManager;

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
                if (rayHit.distance < 0.2f)
                {
                    anim.SetBool("jumping", false);
                    jump = 0;
                }

            }
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            GameManager.instance.loseHP();
            OnDamaged(collision.transform.position);
            timeDelay();
            UIManager.GetComponent<UIManager>().minusHP();

      
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
  
            //point
            GameManager.instance.addHP();
           
  

            //Deactive Item
            collision.gameObject.SetActive(false);
            timeDelay();
            UIManager.GetComponent<UIManager>().addHP();
        }
        else if(collision.gameObject.tag == "Star")
        {
            GameManager.instance.addStar();
            timeDelay();
            UIManager.GetComponent<UIManager>().addStar();

        }
        else if (collision.gameObject.tag == "Finish")
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == "Level1")
            {
                SceneManager.LoadScene("MiniGame2");
            }
            if (sceneName == "Level2")
            {
                SceneManager.LoadScene("Map3");
            }
            if (sceneName == "Level3")
            {
                SceneManager.LoadScene("Clear");
            }
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
