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


    public GameObject[] charactersSelection;
    public GameObject characterSelector;
    public int[] charactersHealth;
    public int[] charactersDamage;
    public GameObject[] enemiesSelection;
    public GameObject enemySelector;
    public int[] enemiesHealth;
    public int[] enemiesDamage;
    private bool isCharacterSelected;
    private bool isEnemySelected;
    private int indexA;
    private int indexE;
    void Start()
    {
        isCharacterSelected = false;
        isEnemySelected = false;
        charactersSelection = GameObject.FindGameObjectsWithTag("Ally");
        charactersHealth = new int[charactersSelection.Length];
        charactersDamage = new int[charactersSelection.Length];

        enemiesSelection = GameObject.FindGameObjectsWithTag("Enemy");
        enemiesHealth = new int[enemiesSelection.Length];
        enemiesDamage = new int[enemiesSelection.Length];
        for (int i = 0; i < enemiesSelection.Length && i < charactersSelection.Length; i++)
        {
            GameManager healthValueE = enemiesSelection[i].GetComponent<GameManager>();
            enemiesHealth[i] = healthValueE.healthE;
            GameManager DamageValueE = enemiesSelection[i].GetComponent<GameManager>();
            enemiesDamage[i] = DamageValueE.damageE;

            GameManager healthValueA = enemiesSelection[i].GetComponent<GameManager>();
            charactersHealth[i] = healthValueA.healthA;
            GameManager DamageValueA = enemiesSelection[i].GetComponent<GameManager>();
            charactersDamage[i] = DamageValueA.damageA;

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
        if (isCharacterSelected && isEnemySelected)
        {
            willAtack = false;
            Combat(enemiesSelection, indexE, charactersDamage[indexA], enemiesHealth);
            isEnemySelected = !isEnemySelected;

            Combat(charactersSelection, 0, 0, charactersHealth);
            isCharacterSelected = !isCharacterSelected;
            destroyClones();
            spawned = false;
        }
        if (Input.GetKeyDown(KeyCode.Space) && !willAtack && !deciding)
        {
            actionSelected = false;
            soulSelected = false;
            bagSelected = false;
            isCharacterSelected = !isCharacterSelected;
            if (!spawned) SpawnMenu();
            deciding = true;
        }

        if (!isCharacterSelected)
        {
            enemySelector.SetActive(false);
            indexA = SelectYourCharacter(characterSelector, charactersSelection, indexA);
        }
        if (deciding)
        {
            changePosition();
            if (Input.GetKeyDown(KeyCode.X))
            {
                menuSelection();
                if (actionSelected)
                {
                    willAtack = true;
                    deciding = false;
                }
                //se deja de decidir la acción
            }
        }
        if (willAtack)
        {
            enemySelector.SetActive(true);
            indexE = SelectYourCharacter(enemySelector, enemiesSelection, indexE);
            if (Input.GetKeyDown(KeyCode.Space))
                isEnemySelected = !isEnemySelected;
        }

    }
    void menuSelection()
    {
        if (menuClone1.transform.position == positions[0]) actionSelected = true;
        if (menuClone2.transform.position == positions[0]) soulSelected = true;
        if (menuClone3.transform.position == positions[0]) bagSelected = true;
    }


    void SpawnMenu()
    {
        spawned = true;
        menuClone1 = (GameObject)Instantiate(menuItem1, positions[0], Quaternion.identity);
        menuClone2 = (GameObject)Instantiate(menuItem2, positions[1], Quaternion.identity);
        menuClone3 = (GameObject)Instantiate(menuItem3, positions[2], Quaternion.identity);
    }

    void SpawnMenu(Vector3 first, Vector3 second, Vector3 third)
    {
        menuClone1 = (GameObject)Instantiate(menuItem1, first, Quaternion.identity);
        menuClone2 = (GameObject)Instantiate(menuItem2, second, Quaternion.identity);
        menuClone3 = (GameObject)Instantiate(menuItem3, third, Quaternion.identity);
    }

    void destroyClones()
    {
        Destroy(menuClone1);
        Destroy(menuClone2);
        Destroy(menuClone3);
    }

    void updateMenuPosition(Vector3 characterP)
    {

        positions[0] = new UnityEngine.Vector3(spawnPosition1.position.x, spawnPosition1.position.y, characterP.z);
        positions[1] = new UnityEngine.Vector3(spawnPosition2.position.x, spawnPosition2.position.y, characterP.z);
        positions[2] = new UnityEngine.Vector3(spawnPosition3.position.x, spawnPosition3.position.y, characterP.z);
    }
    public int SelectYourCharacter(GameObject selector, GameObject[] selectionArray, int index)
    {
        UnityEngine.Vector3 characterPosition = selectionArray[index].transform.position;
        selector.transform.position = new UnityEngine.Vector3(selector.transform.position.x, selector.transform.position.y, characterPosition.z);
        updateMenuPosition(characterPosition);
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (index < selectionArray.Length - 1)
            {
                index++;
            }
            else
            {
                index = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (index > 0)
            {
                index--;
            }
            else
            {
                index = selectionArray.Length - 1;
            }
        }
        return index;
    }

    void changePosition()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && spawned)
        {
            destroyClones();
            selected += 1;
            if (selected > 2) selected = 0;
            SpawnMenu(positions[selected], positions[adjustPositionLeft(selected + 1)], positions[adjustPositionLeft(selected + 2)]);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && spawned)
        {
            destroyClones();
            selected -= 1;
            if (selected < 0) selected = 2;
            SpawnMenu(positions[selected], positions[adjustPositionRight(selected - 2)], positions[adjustPositionRight(selected - 1)]);
        }
    }

    int adjustPositionLeft(int p)
    {
        if (p == 3) return 0;
        if (p == 4) return 1;
        else return p;
    }

    int adjustPositionRight(int p)
    {
        if (p == -1) return 2;
        if (p == -2) return 1;
        else return p;
    }


    void Combat(GameObject[] selection, int index, int damage, int[] healthArray)
    {
        if (selection == charactersSelection)
        {
            for (int i = 0; i < charactersSelection.Length; i++)
            {
                int rN = Random.Range(0, charactersSelection.Length);
                charactersHealth[rN] -= enemiesDamage[i];
                if (charactersHealth[rN] <= 0)
                {
                    Destroy(charactersSelection[rN]);
                    if (rN + 1 > charactersSelection.Length - 1)
                    {
                        System.Array.Resize(ref charactersSelection, charactersSelection.Length - 1);
                    }
                    else
                    {
                        charactersSelection[rN] = charactersSelection[rN + 1];
                    }
                }
                Debug.Log("Recibio daño" + (rN + 1) + " " + charactersHealth[i]);
            }

        }
        else
        {
            healthArray[index] -= damage;
            if (healthArray[index] <= 0)
            {
                Destroy(selection[index]);
                if (index + 1 > selection.Length - 1)
                {
                    System.Array.Resize(ref selection, selection.Length - 1);
                }
                else
                {
                    selection[index] = selection[index + 1];
                }
            }
            Debug.Log("Daño al enemigo: " + index + 1 + " " + healthArray[index]);
        }
    }



}
