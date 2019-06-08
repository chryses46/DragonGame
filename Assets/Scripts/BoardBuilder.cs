using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBuilder : MonoBehaviour
{ 
    // [SerializedField]
    // public class Count
    // {
    //     public int minimum;
    //     public int maximum;

    //     public Count (int min, int max)
    //     {
    //         minimum = min;
    //         maximum = max;
    //     }
    // }
    
    public int columns = 10;                                        //Number of columns on game board.
    public int rows = 10;    
    public GameObject player1;
    public GameObject player2;                                       //Number of rows on game board.
    // public Count wallCount = new Count (5, 9);                      //Lower and upper limit for random number of walls per level.
    // public Count foodCount = new Count (1, 5);                      //Lower and upper limit for random number of food items per level.
    public GameObject mountain;                                     //Prefab to spawn for mountain.
    public GameObject[] grassTiles;                                 //Array of grass prefabs.
    public GameObject[] outerRocks;                                 //Array of outer tile prefabs.
    
    private Transform boardHolder;                                  //A variable to store a reference to the transform of the Board object.
    private List <Vector3> gridPositions = new List <Vector3> ();   //A list of possible locations to place tiles.

    void Start()
    {
        Debug.Log("Welcome to the game.");
        SetupScene(1);
    }
        
    //Clears gridPositions and prepares it to generate a new board.
    void InitialiseList ()
    {
        //Clear gridPositions.
        gridPositions.Clear ();
        
        //Loop through x axis (columns).
        for(int x = 1; x < columns-1; x++)
        {
            //Within each column, loop through y axis (rows).
            for(int y = 1; y < rows-1; y++)
            {
                //At each index add a new Vector3 to our list with the x and y coordinates of that position.
                gridPositions.Add (new Vector3(x, y, 0f));
            }
        }
    }
        
        
    //Sets up the outer rocks and grass (background) of the game board.
    void BoardSetup ()
    {
        //Instantiate Board and set boardHolder to its transform.
        boardHolder = new GameObject ("Board").transform;
        
        //Loop along x axis, starting from -1 (to fill corner) with grass or rock edge tiles.
        for(int x = -1; x < columns + 1; x++)
        {
            //Loop along y axis, starting from -1 to place rocks or rock tiles.
            for(int y = -1; y < rows + 1; y++)
            {
                //Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
                GameObject toInstantiate = grassTiles[Random.Range (0,grassTiles.Length)];
                toInstantiate.transform.localScale = new Vector3(.18f, .18f, 1f);
            
                //Check if the current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
                if(x == -1 || x == columns || y == -1 || y == rows)
                    toInstantiate = outerRocks [Random.Range (0, outerRocks.Length)];
                    toInstantiate.transform.localScale = new Vector3(.18f, .18f, 1f);

            
                //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                GameObject instance =
                    Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
                
                //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
                instance.transform.SetParent (boardHolder);

            }
        }
    }
        
        
    //RandomPosition returns a random position from our list gridPositions.
    Vector3 RandomPosition ()
    {
        //Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
        int randomIndex = Random.Range (0, gridPositions.Count);
        
        //Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
        Vector3 randomPosition = gridPositions[randomIndex];
        
        //Remove the entry at randomIndex from the list so that it can't be re-used.
        gridPositions.RemoveAt (randomIndex);
        
        //Return the randomly selected Vector3 position.
        return randomPosition;
    }
        
        
    //LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
    void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum)
    {
        //Choose a random number of objects to instantiate within the minimum and maximum limits
        int objectCount = Random.Range (minimum, maximum+1);
        
        //Instantiate objects until the randomly chosen limit objectCount is reached
        for(int i = 0; i < objectCount; i++)
        {
            //Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
            Vector3 randomPosition = RandomPosition();
            
            //Choose a random tile from tileArray and assign it to tileChoice
            GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];
            
            //Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }
        
        
    //SetupScene initializes our level and calls the previous functions to lay out the game board
    public void SetupScene (int level)
    {
        //Creates the outer walls and floor.
        BoardSetup ();
        
        //Reset our list of gridpositions.
        InitialiseList ();
        
        //Instantiate the mountain tile in the upper right hand corner of our game board
        Instantiate (mountain, new Vector3 (columns - 5.5f, rows - 5.5f, 0f), Quaternion.identity);
        mountain.transform.localScale = new Vector3(.36f, .36f, 1f);

        Instantiate (player1, new Vector3 (columns -10, rows -10, 0f), Quaternion.identity);
        player1.transform.localScale = new Vector3(.18f, .18f, 1f);

        Instantiate (player2, new Vector3 (columns -1, rows -1, 0f), Quaternion.identity);
        player2.transform.localScale = new Vector3(.18f, .18f, 1f);
    }
}
