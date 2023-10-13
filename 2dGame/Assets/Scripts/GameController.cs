using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject menuItem1;
    public GameObject menuItem2;
    public GameObject menuItem3;
    private GameObject menuClone1;
    private GameObject menuClone2;
    private GameObject menuClone3;
    public Transform spawnPosition1;
    public Transform spawnPosition2;
    public Transform spawnPosition3;

    private Vector3[] positions;
    private int selected;
    private bool spawned;
    // Start is called before the first frame update
    void Start()
    {
        selected = 0;
        spawned = false;
        positions = new Vector3[3];
        positions[0] = spawnPosition1.position;
        positions[1] = spawnPosition2.position;
        positions[2] = spawnPosition3.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X) && !spawned)
            SpawnMenu();
        if(Input.GetKeyDown(KeyCode.Z)){
            destroyClones();
            spawned = false;
        }
        changePosition();
    }

    void SpawnMenu(){
        spawned = true;
        menuClone1 = (GameObject)Instantiate(menuItem1, positions[0], Quaternion.identity);
        menuClone2 = (GameObject)Instantiate(menuItem2, positions[1], Quaternion.identity);
        menuClone3 = (GameObject)Instantiate(menuItem3, positions[2], Quaternion.identity);
    }

    void SpawnMenu(Vector3 first, Vector3 second, Vector3 third){
        menuClone1 = (GameObject)Instantiate(menuItem1, first, Quaternion.identity);
        menuClone2 = (GameObject)Instantiate(menuItem2, second, Quaternion.identity);
        menuClone3 = (GameObject)Instantiate(menuItem3, third, Quaternion.identity);
    }
    void destroyClones(){
        Destroy(menuClone1);
        Destroy(menuClone2);
        Destroy(menuClone3);
    }
    void changePosition(){
        if(Input.GetKeyDown(KeyCode.J) && spawned){
            destroyClones();
            selected +=1;
            if(selected > 2) selected = 0;
            SpawnMenu(positions[selected], positions[adjustPositionLeft(selected+1)], positions[adjustPositionLeft(selected+2)]);
        }
        if(Input.GetKeyDown(KeyCode.L) && spawned){
            destroyClones();
            selected -=1;
            if(selected < 0) selected = 2;
            SpawnMenu(positions[selected], positions[adjustPositionRight(selected-2)], positions[adjustPositionRight(selected-1)]);
        }
    }

    int adjustPositionLeft(int p){
        if(p == 3) return 0;
        if(p == 4) return 1;
        else return p;
    }

    int adjustPositionRight(int p){
        if(p == -1) return 2;
        if(p == -2) return 1;
        else return p;
    }
}
