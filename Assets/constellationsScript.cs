using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;
using rnd = UnityEngine.Random;

public class constellationsScript : MonoBehaviour 
{
	public KMBombInfo bomb;
	public KMAudio Audio;

	public KMSelectable[] btns;
	public GameObject[] btnLabels;
	public Material[] symbols;

	public GameObject starPlane;
	public Material[] starMats;

	String[] constellations = new String[] {
							   "Andromeda",
							   "Antlia",
							   "Apus",
							   "Aquarius",
							   "Aquila",
							   "Ara",
							   "Aries",
							   "Auriga",
							   "Boötes",
							   "Caelum",
							   "Camelopardalis",
							   "Cancer",
							   "Canes Venatici",
							   "Canis Major",
							   "Canis Minor",
							   "Capricornus",
							   "Carina",
							   "Cassiopeia",
							   "Centaurus",
							   "Cepheus",
							   "Cetus",
							   "Chamaeleon",
							   "Circinus",
							   "Columba",
							   "Coma Berenices",
							   "Corona Australis",
							   "Corona Borealis",
							   "Corvus",
							   "Crater",
							   "Crux",
							   "Cygnus",
							   "Delphinus",
							   "Dorado",
							   "Draco",
							   "Equuleus",
							   "Eridanus",
							   "Fornax",
							   "Gemini",
							   "Grus",
							   "Hercules",
							   "Horologium",
							   "Hydra",
							   "Hydrus",
							   "Indus",
							   "Lacerta",
							   "Leo",
							   "Leo Minor",
							   "Lepus",
							   "Libra",
							   "Lupus",
							   "Lynx",
							   "Lyra",
							   "Mensa",
							   "Microscopium",
							   "Monoceros",
							   "Musca",
							   "Norma",
							   "Octans",
							   "Ophiuchus",
							   "Orion",
							   "Pavo",
							   "Pegasus",
							   "Perseus",
							   "Phoenix",
							   "Pictor",
							   "Pisces",
							   "Piscis Austrinus",
							   "Puppis",
							   "Pyxis",
							   "Reticulum",
							   "Sagitta",
							   "Sagittarius",
							   "Scorpius",
							   "Sculptor",
							   "Scutum",
							   "Serpens",
							   "Sextans",
							   "Taurus",
							   "Telescopium",
							   "Triangulum",
							   "Triangulum Australe",
							   "Tucana",
							   "Ursa Major",
							   "Ursa Minor",
							   "Vela",
							   "Virgo",
							   "Volans",
							   "Vulpecula"};
	int[][] skyQ = {
				     new int[] {0, 6, 17, 59, 62, 65, 77, 79},
					 new int[] {7, 10, 11, 14, 37, 45, 46, 50, 54, 82},
					 new int[] {8, 12, 24, 26, 33, 39, 83, 75},
					 new int[] {4, 19, 30, 31, 34, 44, 51, 61, 70, 87},
					 new int[] {3, 15, 25, 38, 43, 60, 57, 53, 66, 71, 74, 78, 81},
					 new int[] {2, 5, 18, 22, 27, 29, 48, 49, 55, 56, 58, 72, 85, 80},
					 new int[] {1, 21, 16, 13, 28, 41, 67, 68, 76, 84, 86},
					 new int[] {9, 20, 23, 42, 40, 36, 35, 32, 47, 52, 63, 64, 69, 73}
					};

	int correctButton = -1;

	static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

	void Awake()
	{
		moduleId = moduleIdCounter++;

		btns[0].OnInteract += delegate () { PressBtn(btns[0], 0); return false; };
		btns[1].OnInteract += delegate () { PressBtn(btns[1], 1); return false; };
		btns[2].OnInteract += delegate () { PressBtn(btns[2], 2); return false; };
		btns[3].OnInteract += delegate () { PressBtn(btns[3], 3); return false; };
	}

	void Start () 
	{
		RandomizeButtons();
	}

	void PressBtn(KMSelectable btn, int index)
	{
		GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
		btn.AddInteractionPunch(.5f);
		if(!moduleSolved)
		{
			if(index == correctButton)
			{
				moduleSolved = true;
				Debug.LogFormat("[Constellations #{0}] Solved! Pressed button {1}.", moduleId, index + 1);
				GetComponent<KMBombModule>().HandlePass();
			}
			else
			{
				Debug.LogFormat("[Constellations #{0}] Strike! Pressed button {1}.", moduleId, index + 1);
				GetComponent<KMBombModule>().HandleStrike();
			}
		}
	}

	void RandomizeButtons()
	{
		int[] quadIndex =  {0, 1, 2, 3, 4, 5, 6, 7};
		quadIndex = quadIndex.OrderBy(x => rnd.Range(1, 1000)).ToArray();

		int[] btnIndex =  {0, 1, 2, 3};
		btnIndex = btnIndex.OrderBy(x => rnd.Range(1, 1000)).ToArray();
		
		for (int i = 0; i < skyQ.Length; i++)
		{
			skyQ[i] = skyQ[i].OrderBy(x => rnd.Range(1, 1000)).ToArray();
		}

		starPlane.GetComponentInChildren<Renderer>().material = starMats[skyQ[quadIndex[0]][0]];

		Debug.LogFormat("[Constellations #{0}] The displayed constellation is {1} - Sky Quadrant {2}.", moduleId, constellations[skyQ[quadIndex[0]][0]], GetSkyQuadrant(quadIndex[0]));

		btnLabels[btnIndex[0]].GetComponentInChildren<Renderer>().material = symbols[skyQ[quadIndex[0]][1]];
		btnLabels[btnIndex[1]].GetComponentInChildren<Renderer>().material = symbols[skyQ[quadIndex[1]][0]];
		btnLabels[btnIndex[2]].GetComponentInChildren<Renderer>().material = symbols[skyQ[quadIndex[2]][0]];
		btnLabels[btnIndex[3]].GetComponentInChildren<Renderer>().material = symbols[skyQ[quadIndex[3]][0]];

		int btn1Index = btnIndex.ToList().IndexOf(0);
		int btn2Index = btnIndex.ToList().IndexOf(1);
		int btn3Index = btnIndex.ToList().IndexOf(2);
		int btn4Index = btnIndex.ToList().IndexOf(3);

		correctButton = btnIndex[0];
		
		Debug.LogFormat("[Constellations #{0}] Button 1 symbol represents the {1} constellation - Sky Quadrant {2}.", moduleId, constellations[skyQ[quadIndex[btn1Index]][btn1Index == 0 ? 1 : 0]], GetSkyQuadrant(quadIndex[btn1Index]));
		Debug.LogFormat("[Constellations #{0}] Button 2 symbol represents the {1} constellation - Sky Quadrant {2}.", moduleId, constellations[skyQ[quadIndex[btn2Index]][btn2Index == 0 ? 1 : 0]], GetSkyQuadrant(quadIndex[btn2Index]));
		Debug.LogFormat("[Constellations #{0}] Button 3 symbol represents the {1} constellation - Sky Quadrant {2}.", moduleId, constellations[skyQ[quadIndex[btn3Index]][btn3Index == 0 ? 1 : 0]], GetSkyQuadrant(quadIndex[btn3Index]));
		Debug.LogFormat("[Constellations #{0}] Button 4 symbol represents the {1} constellation - Sky Quadrant {2}.", moduleId, constellations[skyQ[quadIndex[btn4Index]][btn4Index == 0 ? 1 : 0]], GetSkyQuadrant(quadIndex[btn4Index]));

		Debug.LogFormat("[Constellations #{0}] Button {1} is the correct button.", moduleId, btnIndex[0] + 1);
	}

	String GetSkyQuadrant(int index)
	{
		switch(index)
		{
			case 0:
				return "NQ1 (Northern Hemisphere, top-right)";
			case 1:
				return "NQ2 (Northern Hemisphere, bottom-right)";
			case 2:
				return "NQ3 (Northern Hemisphere, bottom-left)";
			case 3:
				return "NQ4 (Northern Hemisphere, top-left)";
			case 4:
				return "SQ4 (Southern Hemisphere, top-right)";
			case 5:
				return "SQ3 (Southern Hemisphere, bottom-right)";
			case 6:
				return "SQ2 (Southern Hemisphere, bottom-left)";
			case 7:
				return "SQ1 (Southern Hemisphere, top-left)";
		}

		return null;
	}

}
