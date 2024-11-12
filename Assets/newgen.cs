using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class newgen : MonoBehaviour
{
    public string checkpoint = "0";
    public string folderName = "Obstacles";
    public float spawnTime = 0.5f;
    public float gamespeed = 570;
    public AudioClip soundClip; // Drag your sound clip into this field in the Inspector
    public AudioSource audioSource;
    public Transform[] spawnPoints;
    private int spawnCount = 0;
    public TextMeshProUGUI statusbar;
    public TextMeshProUGUI gamestatus;
    public UnityEngine.UI.Button startbutton;
    public bool running = false;
    public int highscore = 0;
    void Start()
    {


        startbutton.onClick.AddListener(StartButtonClick);
        Debug.Log("Welcome to gold scavenger! An experimental space station exploded, leaving gold raining down on this area. You have to collect as much as possible! \n However, other people have the same idea and are camping out on the road to grab as much as possible! [click to read more btw] \n Don't run over them, Or your car will no longer be suitable to drive. Try to get as rich as possible before it all runs out! \n You can go off the road, but that means you get a lower score.");

    }
    void Update()
    {
        GameObject newObject = GameObject.Find("road");
        Rigidbody roadrb = newObject.GetComponent<Rigidbody>();
        //Debug.Log(running);
        if (running == true)
        {


            roadrb.velocity = new Vector3(-1000, 0, 0);
        }
    }
    void StartButtonClick()
    {
        points = 0;
        Debug.Log("Starting game!");
        gamestatus.text = "";
        running = true;
        InvokeRepeating("Spawn", 1.5f, 1.5f);
        InvokeRepeating("pointsys", 0.375f, 0.375f);
        InvokeRepeating("keycheck", 0.11f, 0.11f);
        statusbar.text = "Score: 0";
        startbutton.gameObject.SetActive(false);
    }
    void Spawn()
    {

        spawnCount++;
        GameObject[] objects = Resources.LoadAll<GameObject>("Obstacles");
        int objectIndex = Random.Range(0, objects.Length);
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);


        int[] numbers = new int[] { 1882, 2152, 2019 };
        System.Random random = new System.Random();
        int randomNumber = random.Next(0, 3);


        GameObject newObject = Instantiate(objects[objectIndex], new Vector3(0, 0, numbers[randomNumber]), Quaternion.identity);
        Rigidbody rb = newObject.GetComponent<Rigidbody>();
        newObject.name = spawnCount + "obstacle";

        rb.velocity = new Vector3(-600, 0, 0);

    }

    void keycheck()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            StartCoroutine(MoveForwardWithDelay(false));
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            StartCoroutine(MoveForwardWithDelay(true));
        }
    }

    private IEnumerator MoveForwardWithDelay(bool dir)
    {
        int pos = Mathf.RoundToInt(transform.position.z);
        float directionm = 0f;
        if (pos == 1882 && dir == false)
        {
            directionm = 1.5f;
        }
        else if (pos == 2152 && dir == true)
        {
            directionm = -1.5f;
        }
        else if (pos == 2017)
        {
            if (dir == true)
            {
                directionm = -1.5f;
            }
            else
            {
                directionm = 1.5f;
            }
        }

        // Valid position within the allowed range
        for (int i = 0; i < 30; i++)
        {
            transform.position += Vector3.forward * (directionm *3);
            yield return 0;
        }

    }



    void OnCollisionEnter(Collision collision)
    {
        string objectName = collision.gameObject.name;

        if (objectName.EndsWith("obstacle", System.StringComparison.OrdinalIgnoreCase))
        {
            reset();
            audioSource = GetComponent<AudioSource>();
            audioSource.Play();
            Debug.Log("You died, passing " + objectName.Replace("obstacle", "") + " tents, collecting " + points + " chunks of gold.");
            checkpoint = objectName.Replace("obstacle", "");
            //you died screen
            Rigidbody[] allObjects = FindObjectsOfType<Rigidbody>();
            foreach (Rigidbody obj in allObjects)
            {
                obj.velocity = Vector3.zero;
                obj.angularVelocity = Vector3.zero;
            }



        }


    }

    void pointsys()
    {
        int[] numbers = new int[] { 1882, 2152, 2019 };
        int pointlocation = Random.Range(0, 3);
        GameObject pointObject = Resources.Load<GameObject>("pointsasset");
        GameObject newpoints = Instantiate(pointObject, new Vector3(0, 0, numbers[pointlocation]), Quaternion.identity);
        Rigidbody rb = newpoints.GetComponent<Rigidbody>();
        newpoints.name = "points";
        rb.velocity = new Vector3(-600, 0, 0);


    }



    public int points = 0;
    public void reset()
    {
        if (points > highscore)
        {
            highscore = points;
        }   
        statusbar.text = "Score: " + points + "\nHigh score: " + highscore;
        gamestatus.text = "Game over!";

        CancelInvoke("Spawn");
        CancelInvoke("pointsys");
        CancelInvoke("keycheck");
        GameObject newObject = GameObject.Find("road");
        Rigidbody roadrb = newObject.GetComponent<Rigidbody>();
        running = false;
        roadrb.position = new Vector3(87354, -54, 2024);
        GameObject[] Points = GameObject.FindGameObjectsWithTag("point");

        // Destroy each GameObject
        foreach (GameObject point in Points)
        {
            Destroy(point);
        }
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Finish");
        foreach (GameObject bgObject in obstacles)
        {
            Destroy(bgObject);
        }
        startbutton.gameObject.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.name == "points")
        {
            Destroy(other.gameObject);
            points++;
            statusbar.text = "Score: " + points;
            //timebar.text =
        }
        if (other.gameObject.name == "finishline")
        {
            reset();
            Debug.Log("You have finished successfully, avoiding all of the tents and collecting " + points + " chunks of gold! You are now insanely rich for the next 5 seconds until you crash this car probably ):");
        }

    }




}
