﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bars : TutorialState
{
    public Bars(TutorialManager tutorialManager) : base(tutorialManager){}

    public string instructionText = "Now, let's explain what all these bars do.";
    public string healthBar = "This keeps track of Immunio's health.";
    public string waveBar = "This keeps track of the collective enemy health. Deplete this bar, and you'll move on to the next wave.";
    public string infectionBar = "Beware! Take too long to destroy a wave, and an infection will occur when this bar fills up.\nSmall infection particles will start to infect you!";
    public string infection = "Upon infection, your screen will turn yellow. Your speed decreases and you take more damage.";

    public GameObject[] barSprite =  new GameObject[3];
    public GameObject enemySprite;

    private Vector3 InstructionTextTopPosition;

    public override void StateStart()
    {
        InstructionTextTopPosition = TutorialManager.InstructionTextTop.transform.position;
        TutorialManager.InstructionTextTop.transform.position = new Vector3(Screen.width/2, Screen.height/2,0);
        TutorialManager.SetInstruction(healthBar);

        barSprite[0] = GameObject.Instantiate(TutorialManager.healthBar, TutorialManager.healthBar.transform.position, TutorialManager.healthBar.transform.rotation, TutorialManager.TutorialSprite.transform) as GameObject;
        barSprite[1] = GameObject.Instantiate(TutorialManager.waveBar, TutorialManager.waveBar.transform.position, TutorialManager.waveBar.transform.rotation, TutorialManager.TutorialSprite.transform) as GameObject;
        barSprite[2] = GameObject.Instantiate(TutorialManager.infectionBar, TutorialManager.infectionBar.transform.position, TutorialManager.infectionBar.transform.rotation, TutorialManager.TutorialSprite.transform) as GameObject;
        barSprite[2].transform.Find("Foreground").GetComponent<Image>().fillAmount = 0.75f;
        barSprite[1].transform.Find("Foreground").GetComponent<Image>().fillAmount = 0.75f;
        barSprite[1].SetActive(false);
        barSprite[2].SetActive(false);

        enemySprite  = GameObject.Instantiate(TutorialManager.spriteTemplate, TutorialManager.TutorialSprite.transform.position + new Vector3 (0,-Screen.height/4,0),  TutorialManager.Overlay.transform.rotation, TutorialManager.TutorialSprite.transform) as GameObject;   
        enemySprite.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Tutorial/infection");
        enemySprite.transform.localScale += new Vector3 (0.8f,0.8f,0);
        enemySprite.SetActive(false);
    }

    public override void Update(){
		if(this.pressNumber == 1){
            TutorialManager.SetInstruction(waveBar);
            barSprite[0].SetActive(false);
            barSprite[1].SetActive(true);
		}

        if(this.pressNumber == 2){
            TutorialManager.SetInstruction(infectionBar);
            barSprite[1].SetActive(false);
            barSprite[2].SetActive(true);
            enemySprite.SetActive(true);
        }

        if(this.pressNumber == 3){
            TutorialManager.SetInstruction(infection);
            enemySprite.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Tutorial/infectionScreen");
            enemySprite.transform.localScale += new Vector3 (6f,6f,0);
            this.pressNumber++;
        }

        if(this.pressNumber == 5){
            TutorialManager.InstructionTextTop.transform.position = InstructionTextTopPosition;
            TutorialManager.SetState(new RunAndGun(TutorialManager));
        }

	}
}
