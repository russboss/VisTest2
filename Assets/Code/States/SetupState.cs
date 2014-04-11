using UnityEngine; 
using Assets.Code.Interfaces; 
using System.Collections; 
using System.Collections.Generic; 
using System.Linq;
using System.IO;
using System;


namespace Assets.Code.States 
{ 
	public class SetupState : IStateBase 
	{ 
		private StateManager manager; 
		private GameObject mainCamera; 
		private GameObject newCube;
		private int xDiv = 10;
		private int yDiv = 1000000;
		private int zDiv = 1000;
		private int scale = 40;
		private PlayerControl controller; 
		private string fileDirectory = ".\\Assets\\PopulationData\\WDIDumpOneClean02.csv";
		//private string fileDirectory = "C:\\Unity Projects\\VizTest2\\Assets\\PopulationData\\WDIDumpOneClean02.csv";
		//private string fileDirectory = "C:\\Unity\\VizTest2\\Assets\\PopulationData\\WDIDumpOneClean02.csv";
		private List<List<string>> filedata = new  List<List<string>>();

		private List<GameObject> points = new List<GameObject>();
		//private GameObject pointPrefab = (GameObject)Resources.Load("Cube");


		//average position of all points
		private Vector3 averageCoordinate;
		float avgX=0, avgY=0, avgZ=0;


		public SetupState (StateManager managerRef) 
		{ 
			manager = managerRef; 
			if(Application.loadedLevelName != "Scene0") 
				Application.LoadLevel("Scene0"); 
			//player = GameObject.Find("Player"); 
			//controller = player.GetComponent <PlayerControl>(); 
			mainCamera = GameObject.Find("Main Camera");
			//mainCamera = GameObject.Find("OVRPlayerController");
			// Import data
			newCube = GameObject.Find("Cube");

			// Grab a camera
			
			foreach(GameObject camera in manager.gameDataRef.cameras) 
			{ 
				//if(camera.name != "OVRPlayerController") 
				if(camera.name != "Main Camera") 
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
			import(fileDirectory); 


		} 

		//read csv in and store to filedata
		public void ReadFile(string filename)
		{
			var sr = File.OpenText(filename);
			filedata = sr.ReadToEnd().Split('\n').Select(s=>s.Split(',').ToList()).ToList();
			sr.Close();
		}
		public void averageSum(float x, float y, float z)
		{
			avgX+=x;
			avgY+=y;
			avgZ+=z;
		}
		public Vector3 averageDiv (int count){
			return new Vector3(avgX/count, avgY/count, avgZ/count);
		}


		public void StateUpdate () 
		{ 
			/*
			if(! Input.GetButton("Jump")) 
				controller.transform.Rotate(0, controller.setupSpinSpeed * Time.deltaTime, 0); 
				*/
		} 
		
		public void StateFixedUpdate()
		{ 
		} 

		public void import(String directory){
			ReadFile(directory);
			//access by 
			//string myData = filedata[4][3]; // Row 5 column 4
			String labelx = filedata [0][1];
			String labely = filedata [0][2];
			String labelz = filedata [0][3];

			//!!!!!!!!!!!!!!!!!!!!!!!!!!!!! i < filedata.count
			float x, y, z, r, w;
			for (int i =1; i<filedata.Count; i++) {
				
				Debug.Log("["+i+"] " + filedata[i][0] + "," + filedata[i][1]+","+ filedata[i][2]+","+ filedata[i][3]);
				
				
				
				if(filedata[i][1] != "" || filedata[i][2] !="" || filedata[i][3] !=""){
					String objectLabel = filedata [i][0];
					x = float.Parse (filedata[i][1])/xDiv;
					y = float.Parse (filedata[i][2])/yDiv;
					z = float.Parse (filedata[i][3])/zDiv;
					r = float.Parse (filedata[i][4]);
					//w = float.Parse (filedata[i][5]);
					// GameObject temp= (GameObject)GameObject.Instantiate(pointPrefab,new Vector3(x,y,z), Quaternion.identity);
					//GameObject temp= (GameObject)GameObject.Instantiate(newCube, new Vector3(x,y,z), Quaternion.identity);
					
					GameObject cube = (GameObject)GameObject.Instantiate(newCube, new Vector3(x,y,z), Quaternion.identity);

					cube.AddComponent<Rigidbody>();
					cube.transform.position = new Vector3(x, y, z);
					cube.transform.localScale = new Vector3(scale, scale, scale);


					cube.rigidbody.AddTorque(r ,0 ,0);
					//cube.rigidbody.isKinematic = true;
					cube.rigidbody.useGravity = false;
					//cube.rigidbody.maxAngularVelocity = 0;
					cube.rigidbody.angularDrag = 0;
					//cube.transform.localRotation = new Vector3(r,0,0);
					//cube.transform.rotation = Quaternion.AngleAxis(r,new Vector3(1,0,0) );
					//cube.transform.rotation = Quaternion.AngleAxis(w,new Vector3(0,1,0) );
					//cube.transform.Rotate(new Vector3(r,0,0));


					// Tag to new cube
					// cube.tag = (String)filedata[i][0];
					
					//sum all the coordinates
					averageSum(x,y,z);
					
					// Add the instance to a global list
					
					points.Add(cube);
				}
				

			}
			//
			averageCoordinate = averageDiv(filedata.Count);




			mainCamera.transform.position += averageCoordinate - new Vector3 (50, 50, 50);
			
			//player.transform.position += points [10].transform.position - new Vector3 (10, 30, 30);;
			
			
			
			// Switch to following camera
			foreach(GameObject camera in manager.gameDataRef.cameras) 
			{ 
				// if(camera.name != "Following Camera") 
				if(camera.name != "Main Camera")
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
		public void ShowIt () 
		{ 
			// Lives selection menu
			/*
				GUI.Box(new Rect(Screen.width - 110,10,100,25), 
				        string.Format("Lives left: " + manager.gameDataRef.playerLives)); 
				GUI.Box(new Rect(Screen.width -110,40,100,120), "Player Lives"); 
				if(GUI.Button(new Rect(Screen.width - 100,70,80,20), "5")) 
					manager.gameDataRef.SetPlayerLives(5); 
				if(GUI.Button(new Rect(Screen.width - 100,100,80,20), "10")) 
					manager.gameDataRef.SetPlayerLives(10); 
				if(GUI.Button(new Rect(Screen.width - 100,130,80,20), "Can't Lose")) 
					manager.gameDataRef.SetPlayerLives(1000); 
			*/

			// Actions menu
			//GUI.Box(new Rect(10,10,100,60), "Actions"); 
			//if(GUI.Button(new Rect(20,40,80,20), "Import"))
				//import(fileDirectory); 
			/*
			if(GUI.Button(new Rect(20,70,80,20), "Visualize")) 
				controller.PickedColor(controller.blue); 
			
			if(GUI.Button(new Rect(20,100,80,20), "Animate")) 
				controller.PickedColor(controller.green);
			
			if(GUI.Button(new Rect(20,130,80,20), "Analyze")) 
				controller.PickedColor(controller.yellow); 
			
			if(GUI.Button(new Rect(20,160,80,20), "Report")) 
				controller.PickedColor(controller.white);
			*/
			/* // Player color selection menu
			GUI.Box(new Rect(10,200,100,180), "Player Color"); 
			if(GUI.Button(new Rect(20,230,80,20), "Red"))
				controller.PickedColor(controller.red); 
			
			if(GUI.Button(new Rect(20,260,80,20), "Blue")) 
				controller.PickedColor(controller.blue); 
			
			if(GUI.Button(new Rect(20,290,80,20), "Green")) 
				controller.PickedColor(controller.green);
			
			if(GUI.Button(new Rect(20,320,80,20), "Yellow")) 
				controller.PickedColor(controller.yellow); 
			
			if(GUI.Button(new Rect(20,350,80,20), "White")) 
				controller.PickedColor(controller.white); 
			*/

			/* // Bottom center button
			GUI.Label(new Rect(Screen.width/2 -95, Screen.height - 90, 190, 30), 
			          "Hold Spacebar to pause rotation"); 
			*/

			// Bottom center label
			/*
			if (GUI.Button(new Rect(Screen.width/2 -100, Screen.height - 50, 200, 40), 
		               "Click Here or Press 'P' to Start") || Input.GetKeyUp(KeyCode.P)) 
			{ 
				manager.SwitchState (new PlayStateScene1_1 (manager));
				//player.transform.position = new Vector3(50, .5f, 40); 
			} 
			*/
		} 
	} 
}