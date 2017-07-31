using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BSOD : MonoBehaviour {

	// Use this for initialization
	void Start () {
        string bsod = "*** GAME OVER ***" +
            "\n\n\nA PROBLEM HAS BEEN DETECTED AND ALL MAIN SYSTEMS HAVE BEEN SHUT DOWN TO PREVENT DAMAGE TO YOUR OPERATING SYSTEM." +
            "\n\nTHE PROBLEM SEEMS TO BE CAUSED BY THE FOLLOWING FILE: POWERMANAGER.SYS" +
            "\n" + 
            "\n\n\nTRY AGAIN ?" + "\n>" + "\n\n\n\nTECHNICAL INFORMATION:" +

            "\n\n*** STOP: 0x000000000050" +
            "\n\n*** SCORE: " +
            "\n* LEVELS COMPLETED: " + GameStats.levelsCompleted.ToString() +
            "\n* BATTERIES COLLECTED: " + GameStats.batteriesCollected.ToString() + 
            "\n* POWER FAILURES: " + GameStats.deaths.ToString() +
            "\n* TIMES JUMPED: " + GameStats.jumps.ToString() +
            "\n\n*** FINALSCORE: " + "1,000,00 PTS";

        GetComponent<TextMeshPro>().SetText(bsod);
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
        }
		
	}
}
