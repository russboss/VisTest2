using UnityEngine; 
using System.Collections; 
using Assets.Code.Interfaces;

namespace Assets.Code.States 
{ 
	public class PlayStateScene1_1 : IStateBase 
	{ 
		private StateManager manager; 
		private GameObject player; 
		private PlayerControl controller; 

		public PlayStateScene1_1 (StateManager managerRef) 
		{ 
			manager = managerRef; 
			if(Application.loadedLevelName != "Scene1") 
				Application.LoadLevel("Scene1"); 

			player = GameObject.Find("Player"); 
			controller = player.GetComponent <PlayerControl>();
			player.rigidbody.isKinematic = false; 

			foreach(GameObject camera in manager.gameDataRef.cameras) 
			{ 
				if(camera.name != "Following Camera") 
				{
					if (camera != null)
						camera.SetActive(false); 
				}
				else 
				{
					if (camera != null)
					 	camera.SetActive(true);
				}
			} 
		} 
		
		public void StateUpdate() 
		{ 
			if(manager.gameDataRef.playerLives <= 0) 
			{ 
				manager.SwitchState(new LostStateScene1(manager)); 
				manager.gameDataRef.ResetPlayer(); 
				player.rigidbody.isKinematic = true; 
				player.transform.position = new Vector3(50, .5f, 40); 
			} 
			
			if(manager.gameDataRef.score >= 2) 
			{ manager.SwitchState(new WonStateScene1(manager)); 
				player.rigidbody.isKinematic = true; 
				player.transform.position = new Vector3(50, .5f, 40); 
			} 
			
			if((Input.GetKeyDown(KeyCode.LeftControl)) || (Input.GetKeyDown(KeyCode.Z)))
			{ 
				controller.FireEnergyPulse(); 
			} 
			
			// Test for 1 key being pushed, if so, switch to PlayStateScene1_1
			if(Input.GetKeyUp(KeyCode.Alpha2))
			{ 
				manager.SwitchState(new PlayStateScene1_2(manager)); 
			} 
		} 
		
		public void ShowIt() 
		{ 
			/*
				GUI.Box(new Rect(10,10,100,25), 
				      string.Format("Score: " + manager.gameDataRef.score)); 
				if(GUI.Button(new Rect(Screen.width / 2 - 130, 10, 260, 30),
		              string.Format("Click here or Press 2 for Level 1, State 2")) || Input.GetKeyUp(KeyCode.Alpha2)) 
				{ 
					manager.SwitchState(new PlayStateScene1_2(manager)); 
				} 
				
				GUI.Box(new Rect(Screen.width - 110,10,100,25), 
				        string.Format("Lives left: " + manager.gameDataRef.playerLives)); 
			*/
		} 
	} 
}
