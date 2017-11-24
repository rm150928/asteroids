using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class spaceship : MonoBehaviour {

    // Use this for initialization

    public Rigidbody2D  rb;
    public float        trust;
    public float        turnTrust;
    public float        screenTop;
    public float        screenBottom;
    public float        screenLeft;
    public float        screenRight;
    public float        bulletForce;
    public float        deathForce;


    public int          lives;
    public int          score;
    public GameObject   bullet;
    public GameObject   gameOverPannel;

    private float       thrustInput;
    private float       turnInput;

    public Color        inColor;
    public Color        normalColor;

    public Text         scoreText;
    public Text         liveText;

    void Start () {
        score = 0;

        scoreText.text = "score " + score;
        liveText.text = "leves " + lives;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Check input keyboard
        thrustInput = Input.GetAxis("Vertical");
        turnInput   = Input.GetAxis("Horizontal");

        //input fireKey

        if (Input.GetButtonDown("Fire1"))
        {
           GameObject newBullet = Instantiate(bullet, transform.position, transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(transform.up * bulletForce);
            Destroy (newBullet, 3.0f);
        }

        //turing the ship

        transform.Rotate(Vector3.forward * turnInput * Time.deltaTime * -turnTrust);



        //check screen wraping

        Vector2 newPos = transform.position;

        if (transform.position.y > screenTop)
        {
            newPos.y = screenBottom;
        }

        if (transform.position.y < screenBottom)
        {
            newPos.y = screenTop;
        }

        if (transform.position.x > screenRight)
        {
            newPos.x = screenLeft;
        }

        if (transform.position.x < screenLeft)
        {
            newPos.x = screenRight;
        }


        transform.position = newPos; 

    }
    private void FixedUpdate()
    {
        rb.AddRelativeForce(Vector2.up * thrustInput);
        //rb.AddTorque(-turnInput);
    }

    void ScorePoints(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = "score " + score;
    }
    
    void Respawn()
    {
        rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.enabled = true;
        sr.color = inColor;
        Invoke("Invulnerable", 3f);
    }

    void Invulnerable()
    {
        GetComponent<Collider2D>().enabled = true;
        GetComponent<SpriteRenderer>().color = normalColor;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.relativeVelocity.magnitude > deathForce)
        {
            lives--;
            liveText.text = "Lives " + lives;
            //respan new live
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            Invoke("Respawn", 3f);
                
            if (lives<= 0)
            {
                //gameover
                GameOver();
            }
            
           
        }
        
    }
    void GameOver()
    {
        CancelInvoke();
        gameOverPannel.SetActive(true);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Main");
    }
}
