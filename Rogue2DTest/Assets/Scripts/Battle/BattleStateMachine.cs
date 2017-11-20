using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleStateMachine : MonoBehaviour {


    public enum PerformAction
    {
        WAIT,
        TAKEACTION,
        PERFORMACTION
    }

    public enum HeroGUI
    {
        ACTIVATE,
        WAITING,
        INPUT1,
        INPUT2,
        DONE
    }

    public PerformAction battleStates;
    public Transform spacer;
    public List<TurnHandler> performList = new List<TurnHandler>();
    public List<GameObject> playersInBattle = new List<GameObject>();
    public List<GameObject> enemiesInBattle = new List<GameObject>();
    
    public HeroGUI heroInput;
    public GameObject enemyButton;

    public List<GameObject> heroesToManage = new List<GameObject>();
    private TurnHandler heroChoice;

	// Use this for initialization
	void Start ()
    {
        battleStates = PerformAction.WAIT;
        enemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        playersInBattle.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        EnemyButtons();
	}
	
	// Update is called once per frame
	void Update ()
    {
		switch (battleStates)
        {
            case (PerformAction.WAIT):
                if(performList.Count > 0)
                {
                    battleStates = PerformAction.TAKEACTION;
                }
                break;

            case (PerformAction.TAKEACTION):
                GameObject performer = GameObject.Find(performList[0].attacker);
                if(performList[0].type == "Enemy")
                {
                    EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine>();
                    ESM.targetToAttack = performList[0].targetOfAttacker;
                    ESM.currentState = EnemyStateMachine.TurnState.ACTION;
                }
                if (performList[0].type == "Player")
                {

                }
                battleStates = PerformAction.PERFORMACTION;
                break;

            case (PerformAction.PERFORMACTION):

                break;
        }
	}

    public void CollectActions(TurnHandler action)
    {
        performList.Add(action);
    }

    void EnemyButtons()
    {
        foreach(GameObject enemy in enemiesInBattle)
        {
            GameObject newButton = Instantiate(enemyButton) as GameObject;
            EnemySelectButton button = newButton.GetComponent<EnemySelectButton>();
            EnemyStateMachine currentEnemy = enemy.GetComponent<EnemyStateMachine>();

            Text buttonText = newButton.GetComponentInChildren <Text>();
            buttonText.text = currentEnemy.enemy.name;

            button.enemyObject = enemy;
            newButton.transform.SetParent(spacer,false);
        }
    }
}
