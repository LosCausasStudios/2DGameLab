using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
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
    //public Transform selectedPosition;

    private Vector3[] positions;
    private int selected;
    private bool spawned;
    private bool actionSelected;
    private bool soulSelected;
    private bool bagSelected;
    private bool deciding;
    private bool willAtack;

    public List<Character> allies = new List<Character>();
    public List<Character> enemies = new List<Character>();
    public GameObject characterSelector;
    public GameObject enemySelector;
    private bool isCharacterSelected;
    private bool isEnemySelected;
    public int indexA;
    public int indexE;
    void Start()
    {   
        isCharacterSelected = false;
        isEnemySelected = false;
        Character[] allCharacters = FindObjectsOfType<Character>();
        foreach(Character character in allCharacters){
            if(character.CompareTag("Ally")){
              allies.Add(character);  
              character.damage = 30;
            }
            else if(character.CompareTag("Enemy")){
              enemies.Add(character);
              character.damage = 25;
            }

        }

        selected = 0;
        spawned = false;
        positions = new Vector3[3];
        positions[0] = spawnPosition1.position;
        positions[1] = spawnPosition2.position;
        positions[2] = spawnPosition3.position;
        deciding = false;
        actionSelected = false;
        bagSelected = false;
        soulSelected = false;

    }

    void Update()
    {

        if(isCharacterSelected && isEnemySelected){
            willAtack = false;
            Combat(enemies, indexE, indexA);
            isEnemySelected = !isEnemySelected;
            
            Combat(allies, 0, 0);
            isCharacterSelected = !isCharacterSelected;
            destroyClones();
            spawned = false;
        }
        if (Input.GetKeyDown(KeyCode.Space) && !willAtack)
        {
            actionSelected = false;
            soulSelected = false;
            bagSelected = false;
            isCharacterSelected = !isCharacterSelected;
            if(!spawned) SpawnMenu();
            deciding = true;
        }

        if (!isCharacterSelected)
        {
            enemySelector.SetActive(false);
            indexA = SelectYourCharacter(characterSelector, allies, indexA);
        }
        if(deciding){
            changePosition();
            if(Input.GetKeyDown(KeyCode.X)){
                menuSelection();
                if(actionSelected) 
                {
                    willAtack = true;
                    deciding = false;
                }
                //se deja de decidir la acción
            }
        }
        if(willAtack)
        {
            enemySelector.SetActive(true);
            indexE = SelectYourCharacter(enemySelector, enemies, indexE);
            if(Input.GetKeyDown(KeyCode.Space))
                isEnemySelected = !isEnemySelected;
        }
        
    }
    void menuSelection(){
        if(menuClone1.transform.position == positions[0]) actionSelected = true;
        if(menuClone2.transform.position == positions[0]) soulSelected = true;
        if(menuClone3.transform.position == positions[0]) bagSelected = true;
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

    void updateMenuPosition(Vector3 characterP){

        positions[0] = new UnityEngine.Vector3(spawnPosition1.position.x, spawnPosition1.position.y, characterP.z);
        positions[1] = new UnityEngine.Vector3(spawnPosition2.position.x, spawnPosition2.position.y, characterP.z);
        positions[2] = new UnityEngine.Vector3(spawnPosition3.position.x, spawnPosition3.position.y, characterP.z);
    }
    public int SelectYourCharacter(GameObject selector, List<Character> characterList, int index)
    {
        if(index < 0 || index > characterList.Count -1){
            index = 0;
        }
        UnityEngine.Vector3 characterPosition = characterList[index].transform.position;
        selector.transform.position = new UnityEngine.Vector3(selector.transform.position.x, selector.transform.position.y, characterPosition.z);
        updateMenuPosition(characterPosition);
        if(Input.GetKeyDown(KeyCode.DownArrow)){
            if(index < characterList.Count -1){
                index++ ;
            }
            else{
                index = 0;
            }
        }
        if(Input.GetKeyDown(KeyCode.UpArrow)){
            if(index > 0){
                index--;
            }
            else{
                index = characterList.Count-1;
            }
        }
        return index;
    }

    void changePosition(){
        if(Input.GetKeyDown(KeyCode.LeftArrow) && spawned){
            destroyClones();
            selected +=1;
            if(selected > 2) selected = 0;
            SpawnMenu(positions[selected], positions[adjustPositionLeft(selected+1)], positions[adjustPositionLeft(selected+2)]);
        }
        if(Input.GetKeyDown(KeyCode.RightArrow) && spawned){
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

    
    void Combat(List<Character> characterList, int index, int index2){
        if(characterList == allies){
            for(int i = 0; i < allies.Count; i++){
                int rN = Random.Range(0,allies.Count);
                allies[rN].health -= enemies[i].damage;
                Debug.Log("Recibio daño"+ (rN+1) + " " + allies[i].health);
                if(allies[rN].health <= 0){     
                    Destroy(allies[rN].gameObject);
                    allies.RemoveAt(rN);               
                }
            }

        }
        else{
            characterList[index].health -= allies[index2].damage;
            if(characterList[index].health <= 0){      
                Destroy(characterList[index].gameObject);
                characterList.RemoveAt(index);
            }
            Debug.Log("Daño al enemigo: "+ index2 + 1 +" " + characterList[index].health);
        }

    }
    void healthUpdate(List<Character> characterList){
    }
}
