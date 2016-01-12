using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Crab.Controllers;
using Crab.Utils;
using Crab;


public class Main : SceneScript
{

    public enum Events
    {
        NONE,
        PRESENTATION,
        GREETINGS,
        BUTTONS_SETUP,
        BUTTONS_SETUP_DONE,
        GREETINGS_NO_ANSWER,
        GREETINGS_ANSWER,
        CASE,
        CASE_READY,
        CASE_INSIST,
        CASE_DECIDE,
        CASE_2,
        CASE_2_READY,
        CASE_2_DECIDE,
        CASE_2_DECIDE_2,
        CASE_2_INSIST,
        FINAL,
        FINAL_DONE,
        FINAL_DONE_2
    }

    protected EventsMap events;

    [Space()]
    public FloodLight playerLight;
    public Light consoleLight;
    public FloodLight greenLight;
    public FloodLight blueLight;
    public Crab.Event HUD;

    [Space()]
    public Console console;
    public Crab.Event buttonHolster;
    public Button greenButton;
    public Button blueButton;
    public SelectionText buttonTexts;
    public List<GameObject> spawnObjects;

    [Space()]
    public Canvas blueHelp;
    public Canvas greenHelp;

    private bool insistedAboutKid = false;

    protected override void BeforeGameStart()
    {
        events = new EventsMap(this);
        consoleLight.enabled = false;
    }

    protected override void OnGameStart(PlayerController player)
    {
        events.RegistryEvent((int)Events.PRESENTATION, 2000);
        AudioListener.pause = false;
        Time.timeScale = 1;
        blueHelp.enabled = false;
        greenHelp.enabled = false;
    }

    //GameStats Handling
    //

    void Update()
    {
        events.Update();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!HUD.IsStarted())
            {
                HUD.StartEvent();
            }
            else
            {
                HUD.FinishEvent();
            }
        }
    }

    void OnApplicationFocus(bool status)
    {
        if (!status)
        {
            HUD.StartEvent();
        }
    }

    void OnEvent(int id)
    {
        switch ((Events)id)
        {
            case Events.PRESENTATION:
                playerLight.TurnOn();
                consoleLight.enabled = true;
                events.RegistryEvent((int)Events.GREETINGS, 3000);
                break;

            case Events.GREETINGS:
                console.Write("Good Morning, Mr. Brian");
                events.RegistryEvent((int)Events.BUTTONS_SETUP, 2000);
                break;

            case Events.BUTTONS_SETUP:
                buttonHolster.StartEvent();
                events.RegistryEvent((int)Events.BUTTONS_SETUP_DONE, 4000);
                break;

            case Events.BUTTONS_SETUP_DONE:
                UnlockButtons(Events.GREETINGS_ANSWER, "Good Morning", "Hey");
                blueHelp.enabled = true;
                greenHelp.enabled = true;
                events.RegistryEvent((int)Events.GREETINGS_NO_ANSWER, 14000);
                break;


            case Events.GREETINGS_NO_ANSWER:
                LockButtons();
                blueHelp.enabled = false;
                greenHelp.enabled = false;
                console.Write("You are not very talkative today, Brian", 75);
                events.RegistryEvent((int)Events.CASE, 5000);
                break;

            case Events.CASE:
                console.Write("Anyway, lets start.", 150);
                console.Write("");
                console.Write("Judge Mr. Brian (Nº241)");
                console.Write("");
                console.Write("Case Nº 72");
                console.Write("Mr.Burke was an architect that built a cottage for Mr.Bender and he didn't take care of it.", 50);
                console.Write("The cottage collapsed after 2 years and Bender was injured.");
                console.Write("Whose fault it is?", 100);
                events.RegistryEvent((int)Events.CASE_READY, 20000);
                break;

            case Events.CASE_READY:
                UnlockButtons(Events.CASE_DECIDE, "Mr. Burke", "Mr. Bender");
                events.RegistryEvent((int)Events.CASE_INSIST, Random.Range(18000, 24000));
                break;

            case Events.CASE_INSIST:
                console.Write("We have other cases to judge, Brian.", 75);
                console.Write("Keep going my friend.", 100);
                break;

            case Events.CASE_2:
                console.Write("");
                console.Write("Okay, one less. Next case:", 150);
                console.Write("");
                console.Write("Case Nº 56");
                console.Write("The 5 of February a lot of money was found under a ruined cottage by a kid.", 50);
                console.Write("The kid had a dream. His dream was to travel into space.", 75);
                console.Write("He needed that money.", 100);
                console.Write("But then the justice claimed it. They said it was a historical treasure.", 50);
                console.Write("Who got the money?", 100);

                events.RegistryEvent((int)Events.CASE_2_READY, 29000);
                break;

            case Events.CASE_2_READY:
                events.RegistryEvent((int)Events.CASE_2_INSIST, Random.Range(18000, 24000));
                if(!insistedAboutKid)
                    UnlockButtons(Events.CASE_2_DECIDE, "The Kid", "The Justice");
                else
                    UnlockButtons(Events.CASE_2_DECIDE_2, "The Kid", "The Justice");
                break;


            case Events.CASE_2_INSIST:
                console.Write("We can't waste all the day here.", 75);
                console.Write("Maybe it is too hard...", 100);
                break;

            case Events.FINAL:
                console.Write("");
                console.Write("This was a productive session Mr. Brian (Nº241).", 150);
                console.Write("Tomorrow more...");
                console.Write("....");
                console.Write("...");
                console.Write("..");
                console.Write("");
                console.Write("");
                console.Write("");
                events.RegistryEvent((int)Events.FINAL_DONE, 9000);
                break;

            case Events.FINAL_DONE:
                playerLight.TurnOff();
                consoleLight.enabled = false;
                events.RegistryEvent((int)Events.FINAL_DONE_2, 3000);
                break;

            case Events.FINAL_DONE_2:
                //Game Over
                FindObjectOfType<Menu>().Play(2);
                break;

            default:
                Debug.LogWarning("Unknow event (id: " + id + ")");
                break;
        }
    }

    /**
     * RESULT
    **/
    void OnDecide(ButtonDecideResult result)
    {
        ButtonType type = result.type;

        switch (result.eventId)
        {
            case Events.GREETINGS_ANSWER:
                events.CancelEvent((int)Events.GREETINGS_NO_ANSWER);
                LockButtons();
                blueHelp.enabled = false;
                greenHelp.enabled = false;
                console.Write("Why are you using the buttons to answer me?", 100);
                events.RegistryEvent((int)Events.CASE, 5000);
                break;

            case Events.CASE_DECIDE:
                events.CancelEvent((int)Events.CASE_INSIST);
                LockButtons();
                console.Write("");
                if (type == ButtonType.GREEN)
                {
                    console.Write("Nice choose.", 100);
                    console.Write("Maybe he could have done a better job with more budget.", 150);
                    console.Write("Mr. Bender was always very greedy.", 150);
                    console.Write("He always wanted to have more money... now he is dead.", 150);
                    events.RegistryEvent((int)Events.CASE_2, 14000);
                }
                else
                {
                    console.Write("So your choose is Mr. Bender", 100);
                    console.Write("He was also my choose Mr.Brian! ", 100);
                    console.Write("But Mr.Bender died. It's true that the cottage was cheap but...", 100);
                    console.Write("He only wanted the best for his family.");
                    console.Write("He worked so hard for it.", 100);
                    events.RegistryEvent((int)Events.CASE_2, 14000);
                }

                SpawnRandom();
                break;

            case Events.CASE_2_DECIDE:
                events.CancelEvent((int)Events.CASE_2_INSIST);
                LockButtons();
                console.Write("");
                if (type == ButtonType.GREEN)
                {
                    //The Kid
                    console.Write("Come on Mr. Brian, you can do it better.", 75);
                    console.Write("The Justice claimed the money,");
                    console.Write("who got it?", 100);
                    insistedAboutKid = true;
                    events.RegistryEvent((int)Events.CASE_2_READY, 9000);
                }
                else
                {
                    //The Justice
                    console.Write("Correct! Exactly!", 100);
                    console.Write("But the kid never went to space.", 150);
                    events.RegistryEvent((int)Events.FINAL, 4000);
                }
                SpawnRandom();
                break;

            case Events.CASE_2_DECIDE_2:
                events.CancelEvent((int)Events.CASE_2_INSIST);
                LockButtons();
                console.Write("");
                if (type == ButtonType.GREEN)
                {
                    //The Kid
                    console.Write("Impressive.");
                    console.Write("You are a special subject...", 100);
                    events.RegistryEvent((int)Events.FINAL, 4000);
                }
                else
                {
                    //The Justice
                    console.Write("Correct! Exactly!", 100);
                    console.Write("But the kid never went the space.");
                    events.RegistryEvent((int)Events.FINAL, 6000);
                }
                SpawnRandom();
                break;
        }

    }

    void LockButtons()
    {
        blueButton.Lock();
        greenButton.Lock();
        buttonTexts.FinishEvent();
        buttonTexts.SetTexts("", "");
    }

    void UnlockButtons(Events eventId, string greenValue = "", string blueValue = "")
    {
        blueButton.UnLock(eventId);
        greenButton.UnLock(eventId);
        buttonTexts.StartEvent();
        buttonTexts.SetTexts(greenValue, blueValue);
    }

    void SpawnRandom() {
        for (int i = 0; i < spawnObjects.Count; i++) {
            GameObject go = spawnObjects[Random.Range(0, spawnObjects.Count)];
            if (go && !go.activeInHierarchy)
            {
                spawnObjects.Remove(go);
                go.SetActive(true);
                return;
            }
        }
    }

}
