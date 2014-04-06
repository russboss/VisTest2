using UnityEngine; 
using Assets.Code.States; 
using Assets.Code.Interfaces; 

public class StateManager : MonoBehaviour 
{ 
	private IStateBase activeState; 
	
	[HideInInspector] 
	public GameData gameDataRef; 
	private static StateManager instanceRef; 
	
	void Awake () 
	{ 
		
		if(instanceRef == null) 
		{ 
			instanceRef = this; 
			DontDestroyOnLoad(gameObject); 
		} 
		else 
		{ 
			DestroyImmediate(gameObject); 
		} 
	} 
	
	void Start () 
	{ 
		activeState = new BeginState(this); 
		gameDataRef = GetComponent < GameData >(); 
	} 
	
	void Update()
	{
		if (activeState != null) activeState.StateUpdate(); 
		
		// Check for Escape key. If pressed, then quit
		if (Input.GetKeyUp (KeyCode.Escape)) 
			Application.Quit();

		UpdateMovement();
	} 

	
	// UpdateMovement
	//
	// Consolidate all movement code here
	//
	
	// We can adjust these to influence speed and rotation of player controller
	private float MoveScaleMultiplier     = 1.0f; 
	private float RotationScaleMultiplier = 1.0f; 
	private bool  AllowMouseRotation      = true;
	private bool  HaltUpdateMovement      = false;
	private float MoveScale 	   = 1.0f;
	public float Acceleration 	   = 0.1f;
	public float RotationAmount    = 1.5f;
	private float 	YRotation 	 = 0.0f;

	static float sDeltaRotationOld = 0.0f;
	public virtual void UpdateMovement()
	{

		// Do not apply input if we are showing a level selection display
		if(HaltUpdateMovement == true)
			return;
		
		bool moveForward = false;
		bool moveLeft  	 = false;
		bool moveRight   = false;
		bool moveBack    = false;
		
		MoveScale = 1.0f;
		
		// * * * * * * * * * * *
		// Keyboard input
		
		// Move
		
		// WASD
		if (Input.GetKey(KeyCode.W)) moveForward = true;
		if (Input.GetKey(KeyCode.A)) moveLeft	 = true;
		if (Input.GetKey(KeyCode.S)) moveBack 	 = true; 
		if (Input.GetKey(KeyCode.D)) moveRight 	 = true; 
		// Arrow keys
		if (Input.GetKey(KeyCode.UpArrow))    moveForward = true;
		if (Input.GetKey(KeyCode.LeftArrow))  moveLeft 	  = true;
		if (Input.GetKey(KeyCode.DownArrow))  moveBack 	  = true; 
		if (Input.GetKey(KeyCode.RightArrow)) moveRight   = true; 
		
		if ( (moveForward && moveLeft) || (moveForward && moveRight) ||
		    (moveBack && moveLeft)    || (moveBack && moveRight) )
			MoveScale = 0.70710678f;

		
		MoveScale *= Time.deltaTime;
		
		// Compute this for key movement
		float moveInfluence = Acceleration * 0.1f * MoveScale * MoveScaleMultiplier;
		
		// Run!
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			moveInfluence *= 2.0f;
		
		
		// Rotate
		
		// compute for key rotation
		float rotateInfluence = Time.deltaTime * RotationAmount * RotationScaleMultiplier;
		
		//reduce by half to avoid getting ill
		if (Input.GetKey(KeyCode.Q)) 
			YRotation -= rotateInfluence * 0.5f;  
		if (Input.GetKey(KeyCode.E)) 
			YRotation += rotateInfluence * 0.5f; 
		
		// * * * * * * * * * * *
		// Mouse input
		
		// Move
		
		// Rotate
		float deltaRotation = 0.0f;
		if(AllowMouseRotation == false)
			deltaRotation = Input.GetAxis("Mouse X") * rotateInfluence * 3.25f;
		
		float filteredDeltaRotation = (sDeltaRotationOld * 0.0f) + (deltaRotation * 1.0f);
		YRotation += filteredDeltaRotation;
		sDeltaRotationOld = filteredDeltaRotation;
		
		
		// Rotate
		
		// compute for key rotation
		rotateInfluence = Time.deltaTime * RotationAmount * RotationScaleMultiplier;
		
		//reduce by half to avoid getting ill
		if (Input.GetKey(KeyCode.Q)) 
			YRotation -= rotateInfluence * 0.5f;  
		if (Input.GetKey(KeyCode.E)) 
			YRotation += rotateInfluence * 0.5f; 
		
		// * * * * * * * * * * *
		
	}

	
	void OnGUI() 
	{ 
		if(activeState != null) 
			activeState.ShowIt(); 
	} 
	
	public void SwitchState(IStateBase newState) 
	{ 
		activeState = newState; 
	} 
	
	public void Restart() 
	{ 
		if (gameObject != null)
			Destroy(gameObject); 
		Application.LoadLevel("Scene0"); 
	} 
}
