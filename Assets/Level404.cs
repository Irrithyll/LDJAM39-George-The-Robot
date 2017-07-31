using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level404 : MonoBehaviour {


    // Use this for initialization
    void Start()
    {
        string string404 = "*** CONGRATULATIONS ***" +
            "\nA ALL SYSTEMS RESTORED; POWER AT 100%" +

            "\n\n*** STOP: 0x000000000040" +
            "\n\n*** SCORE : " +
            "\n* LEVELS COMPLETED: " + GameStats.levelsCompleted.ToString() +
            "\n* BATTERIES COLLECTED: " + GameStats.batteriesCollected.ToString() +
            "\n* POWER FAILURES: " + GameStats.deaths.ToString() +
            "\n* TIMES JUMPED: " + GameStats.jumps.ToString() +
            "\n\n*** FINALSCORE: " + "1,000,00 PTS";

        GetComponent<TextMeshPro>().SetText(string404);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
