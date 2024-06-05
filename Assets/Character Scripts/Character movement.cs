using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Charactermovement : MonoBehaviour
{
    // Variables
    private float horizontal;
    //Acabo de descobrir que existeixen els readonly ho utilitzaré perque m'ho suggereix el visual studio ja investigaré una mica per a que serveix
    readonly float speed = 8f;
    private bool isFacingRight = true;
    public static Charactermovement playerCharMove;
    private int hp;
    public int scID;
    public bool damaged;
    public GameObject playerObject;

    [SerializeField] private AudioSource damageSound;
    [SerializeField] private AudioSource jumpSound;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float knockbackForce = 5f;

    //Activamos los componentes del Player
    public void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //Llamamos la funcion RestorePosition
        RestorePosition(true);

    }


    private void Awake()
    {
        //scID la usaremos para tener un indice en las escenas y asi poderlas recargar y atraversarlas
        scID = SceneManager.GetActiveScene().buildIndex;

        hp = 5;

        if (playerCharMove == null)
        {
            playerCharMove = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(playerCharMove);
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetButtonDown("Jump"))
        {
            ApplyGravity();
        }

        UpdateAnimator();
        MoveCharacter();
        FlipCharacter();

        if (Input.GetButtonDown("Cancel"))
        {
            PauseGame();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colision con " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (hp > 0)
            {
                Debug.Log("HP: " + hp);
                Hurt(collision);
            }
            else
            {
                StartCoroutine(Death());
                SceneManager.LoadScene("DIE");
            }
        }
        else if (collision.gameObject.CompareTag("InstaKill"))
        {
            StartCoroutine(Death());
            SceneManager.LoadScene("DIE");
        }
        else if (collision.gameObject.CompareTag("AntiBug"))
        {
            RestorePosition(false);
        }
    }
    private void MoveCharacter()
    {
        animator.SetBool("Moving", horizontal != 0f);
    }
    
    private void Hurt(Collision2D collision)
    {
        if (!damaged)
        {
            StartCoroutine(DamageCD());

            animator.SetTrigger("DMG");
            hp--;
            Vector2 direction = (transform.position - collision.transform.position).normalized;

            Vector2 parabolicDirection = new Vector2(direction.x, direction.y + 30f).normalized;

            rb.velocity = parabolicDirection * knockbackForce;
        }

    }

    IEnumerator DamageCD()
    {
        damaged = true;
        damageSound.Play();
        yield return new WaitForSeconds(1.5f); 
        damaged = false;
    }

    //Con esta corrutina marcamos la muerte del player, los playerprefs y recargaremos la primera escena
    IEnumerator Death()
    {
        yield return new WaitForSeconds(0.01f);
        PlayerPrefs.DeleteAll();
        Destroy(gameObject);
    }

    private void ApplyGravity()
    {
        if (IsGrounded())
        {
            jumpSound.Play();
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.1f);
            rb.gravityScale *= -1;
            rb.rotation += 180;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void FlipCharacter()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }

    private void UpdateAnimator()
    {
        animator.SetBool("OnAir", !IsGrounded());
    }

    private void PauseGame()
    {
        scID = SceneManager.GetActiveScene().buildIndex;
        Time.timeScale = 0f;
        SavePosition();
        PlayerPrefs.SetInt("PreviousSceneIndex", scID);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Pause");
    }

    //Con esta funcion mediante PlayerPrefs restauramos la posicion del jugador
    private void SavePosition()
    {
        PlayerPrefs.SetFloat("PlayerX", transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", transform.position.y);
        PlayerPrefs.SetFloat("PlayerZ", transform.position.z);
        PlayerPrefs.Save();
    }

    public void RestorePosition(bool safe)
    {
        if (safe && PlayerPrefs.HasKey("PlayerX") && PlayerPrefs.HasKey("PlayerY") && PlayerPrefs.HasKey("PlayerZ"))
        {
            float x = PlayerPrefs.GetFloat("PlayerX");
            float y = PlayerPrefs.GetFloat("PlayerY");
            float z = PlayerPrefs.GetFloat("PlayerZ");
            transform.position = new Vector3(x, y, z);
        }
        else if(!safe)
        {
            transform.position = GameObject.FindWithTag("SpawnPoint").transform.position;
        }
    }
}
