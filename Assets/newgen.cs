using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class newgen : MonoBehaviour
{
    public string checkpoint = "0";
    public string folderName = "Obstacles";
    public float spawnTime = 0.5f;
    public bool spawnallowed = true;
    public float gamespeed = 570;
    public AudioSource explode;
    public Transform[] spawnPoints;
    private int spawnCount = 0;
    void Start()
    {

        Debug.Log("Welcome to gold scavenger! An experimental space station exploded, leaving gold raining down on this area. You have to collect as much as possible! \n However, other people have the same idea and are camping out on the road to grab as much as possible! [click to read more btw] \n Don't run over them, Or your car will no longer be suitable to drive. Try to get as rich as possible before it all runs out! \n You can go off the road, but that means you get a lower score.");
       
        InvokeRepeating("Spawn", 1.5f,1.5f);
        InvokeRepeating("pointsys", 0.375f,0.375f);
        InvokeRepeating("keycheck", 0.11f,0.11f);
    }

    void Spawn()    
{
    if(spawnallowed==true){
    spawnCount++;
    GameObject[] objects = Resources.LoadAll<GameObject>("Obstacles");
    int objectIndex = Random.Range(0, objects.Length);
    int spawnPointIndex = Random.Range(0, spawnPoints.Length);
    

int[] numbers = new int[] {1882,2152,2019};
System.Random random = new System.Random();
int randomNumber = random.Next(0, 3);

    
    GameObject newObject = Instantiate(objects[objectIndex], new Vector3(0, 0, numbers[randomNumber]), Quaternion.identity);
    Rigidbody rb = newObject.GetComponent<Rigidbody>();
    newObject.name = spawnCount+"obstacle";
   
    rb.velocity = new Vector3(-600,0, 0);
}}
void Update()
    {
    
        GameObject newObject = GameObject.Find("road");
        Rigidbody rb = newObject.GetComponent<Rigidbody>();
     
        rb.velocity = new Vector3(-420, 0, 0);
        if (newObject.transform.position.x <-1000)
        {
            //end of game here
        }
    
    
        
    
    
    }

 void keycheck()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            StartCoroutine(MoveForwardWithDelay(1.5f));
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            StartCoroutine(MoveForwardWithDelay(-1.5f));
        }
    }

private IEnumerator MoveForwardWithDelay(float dir)
{
    float newPosition = transform.position.z + dir;

    if ((newPosition >= 1882 && newPosition <= 2152) ||
        (newPosition >= 2017 && newPosition <= 2152) ||
        (newPosition >= 1882 && newPosition <= 2017))
    {
        // Valid position within the allowed range
        for (int i = 0; i < 90; i++)
        {
            transform.position += Vector3.forward * dir;
            yield return new WaitForSeconds(0.004f);
        }
    }
    else
    {
        // Invalid position, adjust the position to the nearest allowed value
        if (newPosition < 1882)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 1882);
        }
        else if (newPosition > 2152)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 2152);
        }
        else if (newPosition > 2017)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 2017);
        }
    }
}



void OnCollisionEnter(Collision collision)
{
    string objectName = collision.gameObject.name;
    
    if (objectName.EndsWith("obstacle", System.StringComparison.OrdinalIgnoreCase))
    {
        Debug.Log("You died, passing "+objectName.Replace("obstacle", "")+" tents, collecting "+points+" chunks of gold.");
        checkpoint = objectName.Replace("obstacle", "");
        //you died screen
        Rigidbody[] allObjects = FindObjectsOfType<Rigidbody>();
        foreach (Rigidbody obj in allObjects)
        {
            obj.velocity = Vector3.zero;
            obj.angularVelocity = Vector3.zero;
        }
       spawnallowed = false;
        
        

    }
    
    
}

void pointsys()
{
    if(spawnallowed==true){
    int[] numbers = new int[] {1882,2152,2019};
    for (int i = 0; i < 3; i++){
        GameObject pointObject = Resources.Load<GameObject>("pointsasset");
        GameObject newpoints = Instantiate(pointObject, new Vector3(0, 0, numbers[i]), Quaternion.identity);
        Rigidbody rb = newpoints.GetComponent<Rigidbody>();
        newpoints.name = "points";
        rb.velocity = new Vector3(-600,0, 0);
    }


}}



    public int points = 0;

    private void OnTriggerEnter(Collider other)
{
    if (other.gameObject.name == "points")
    {
        Destroy(other.gameObject);
        points++;
        
    }
    if(other.gameObject.name=="finishline"){
        Debug.Log("You have finished successfully, avoiding all of the tents and collecting "+points+" chunks of gold! You are now insanely rich for the next 5 seconds until you crash this car probably ):");
    }
}




}
