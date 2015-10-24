using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class MainMenuManager : MonoBehaviour {
	
	void Start () {
		versionText.text = "Version: "+Version.stage+" "+Version.current;
	}
	
	public Text versionText;
	
	public CanvasGroup cgMainMenu;
	public CanvasGroup cgSettingsMenu;
	// todo: others here (settings sub-menus)
	
	public void SetMenuState (int state) {
		// sets which menu is open (main, options, new game..)
		
		// 0 = main menu
		// 1 = settings
		
		DeactivateCG(cgMainMenu);
		DeactivateCG(cgSettingsMenu);
		//todo: deactivate all
		
		switch(state) {
		case 0:
			ActivateCG(cgMainMenu);
			break;
		case 1:
			ActivateCG(cgSettingsMenu);
			break;
		//todo: others here
		}
	}
	private void ActivateCG (CanvasGroup cg) {
		cg.interactable = true;
		cg.blocksRaycasts = true;
		
		cg.alpha = 1;
		//todo: do fancy transitions in Update
	}
	private void DeactivateCG (CanvasGroup cg) {
		cg.interactable = false;
		cg.blocksRaycasts = false;
		
		cg.alpha = 0;
		//todo: do fancy transitions in Update
	}
	
	public void Play () {
		
		if(Directory.Exists("Savegame")) {
			if(File.Exists("Savegame/save.file")) {
				
				StreamReader reader = new StreamReader("Savegame/save.file");
				bool isValidSave=false;
				
				string line="";
				
				try {
					line = reader.ReadLine();
					if(line.Split('=')[1].Trim() == Version.current) isValidSave = true;
				}
				catch (System.Exception e) {
					
					#region log dump
					List<object> logParams = new List<object>();
					
					logParams.Add("Line: "+line);
					logParams.Add("Directories: ");
					foreach(string dir in Directory.GetDirectories("Savegame")){
						logParams.Add("	"+dir);
					}
					logParams.Add("Files: ");
					foreach(string file in Directory.GetFiles("Savegame")) {
						logParams.Add("	"+file);
					}
					
					FileAttributes attributes = File.GetAttributes("Savegame/save.file");
					
					logParams.Add("");
					logParams.Add("save file read only: "+((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly));
					logParams.Add("save.file content:");
					logParams.Add("----BEGIN FILE----");
					logParams.Add("");
					logParams.Add(line);
					logParams.Add(reader.ReadToEnd());
					logParams.Add("");
					logParams.Add("----END FILE----");
					
					string logPath = ErrorLogger.CreateErrorLog("Error reading savegame", e, logParams.ToArray());
					#endregion
					
					DebugConsole.LogError("Error reading savegame - "+e.Message+"  Error log has been created at \"logs/"+logPath+"\"");
					
					//todo: show a popup error
					
					return; //finally will be executed, no worries
				}
				finally {
					reader.Close();
				}
				
				if(isValidSave) {
					LoadGame();
					return;
				}
				else { // invalid savegame version
					
					DebugConsole.LogError("Invalid savegame version! Game: "+Version.current+", Save: "+line.Split('=')[1].Trim());
					
					//todo: show a popup error
					
					return;
				}
			}
		}
		
		StartNewGame();
		
	}
	private void LoadGame () {
		Application.LoadLevel("Game");
	}
	private void StartNewGame () {
		if(Directory.Exists("Savegame")) Directory.Delete("Savegame");
		Directory.CreateDirectory("Savegame");
		
		Application.LoadLevel("Game");
	}
	
	public void Exit () {
		Application.Quit();
	}
	
}
