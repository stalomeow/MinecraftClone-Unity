using UnityEngine;
using System.Collections;
[System.Serializable]
public class DayNightController : MonoBehaviour {
	//Speed of the cycle (if you set this to 1 the one hour in the cycle will pass in 1 real life second)
	public float daySpeedMultiplier = 0.1f;
	//main directional light
	public Light sunLight;
	//control intensity of sun?
	public bool controlIntensity = true;
	//what time this cycle should start
	public float startTime = 12.0f;
	//what's the current time
	float currentTime = 0.0f;
	public string timeString = "00:00 AM";
	//x rotation value of the light
	private float xValueOfSun = 90.0f;
	//Clouds
	[SerializeField]	public Transform[] cloudSpheres;
	//Rotation speed of clouds
	public float cloudRotationSpeed = 1.0f;
	//Rotation speed of spheres
	[SerializeField]	public Transform[] starSpheres;
	//Twinkle frequency of the stars
	public float twinkleFrequency = 5.0f;
	//background counter for twikle effect
	private float twinkleCounter = 0.0f;
	//star's rotation speed
	public float starRotationSpeed = 0.15f;
	//camera to follow
	public Camera cameraToFollow;

 	// Use this for initialization
	void Start () {
		//set the start time
		currentTime = startTime;
	}
	
	// Update is called once per frame
	void Update () {
		//increment time
		currentTime += Time.deltaTime*daySpeedMultiplier;
		//reset time
		if (currentTime >= 24.0f) {
			currentTime %= 24.0f;
		}
		//Check for sunlight
		if (sunLight) {
			ControlLight();
		}
		//Check for cloudsphere
		if (cloudSpheres.Length > 0) {
			ControlClouds();
		}
		//Check for starsphere
		if (starSpheres.Length > 0) {
			StarSphere();
		}
		//Camera control
		ControlCamera ();
		//Gets The timeString;
		CalculateTime ();
	}

	void ControlLight() {
		//Rotate light
		xValueOfSun = -(90.0f+currentTime*15.0f);
		sunLight.transform.eulerAngles = sunLight.transform.right*xValueOfSun;
		//reset angle
		if (xValueOfSun >= 360.0f) {
			xValueOfSun = 0.0f;
		}
		//This basically turn on and off the sun light based on day / night
		if (controlIntensity && sunLight && (currentTime >= 18.0f || currentTime <= 5.5f)) {
			sunLight.intensity = Mathf.MoveTowards(sunLight.intensity,0.0f,Time.deltaTime*daySpeedMultiplier*10.0f);
		} else if (controlIntensity && sunLight) {
			sunLight.intensity = Mathf.MoveTowards(sunLight.intensity,1.0f,Time.deltaTime*daySpeedMultiplier*10.0f);
		}

	}

	void ControlClouds (){
		//Rotate clouds
		foreach (Transform cloud in cloudSpheres) {
			if (cloud){
				cloud.transform.Rotate(Vector3.forward*cloudRotationSpeed*daySpeedMultiplier*Time.deltaTime);
			}
		}
	}

	void StarSphere() {
		//Get the color of the stars
		Color currentColor;
		//Rotate and eneble and disable stars
		foreach (Transform stars in starSpheres) {
			if (stars){
				stars.transform.Rotate(Vector3.forward*starRotationSpeed*daySpeedMultiplier*Time.deltaTime);
				if (currentTime > 5.5f && currentTime < 18.0f && stars.GetComponent<Renderer>()){
					currentColor = stars.GetComponent<Renderer>().material.color;
					stars.GetComponent<Renderer>().material.color = new Color (currentColor.r,currentColor.g,currentColor.b,Mathf.Lerp(currentColor.a , 0.0f,Time.deltaTime*50.0f*daySpeedMultiplier));
				}
			
			}

		}
		//Choose in between range
		int chosenOne = Random.Range (0, starSpheres.Length);

		//Twinkle effect
		if (starSpheres [chosenOne] && twinkleCounter <= 0.0f && (currentTime >= 18.0f || currentTime <= 5.5f) && starSpheres [chosenOne].GetComponent<Renderer>()) {
			twinkleCounter = 1.0f;
			currentColor = starSpheres [chosenOne].GetComponent<Renderer>().material.color;
			starSpheres [chosenOne].GetComponent<Renderer>().material.color = new Color (currentColor.r,currentColor.g,currentColor.b,Random.Range(0.01f,0.5f));
		}
		//Reset counter
		if (twinkleCounter > 0.0f) {
			twinkleCounter -= Time.deltaTime*daySpeedMultiplier*twinkleFrequency;
		}
	}

	void ControlCamera () {
		//Get camera
		if (!cameraToFollow) {
			cameraToFollow = Camera.main;
			return;
		}
		//set position to the camera
		if (cameraToFollow) {
			transform.position = cameraToFollow.transform.position;
		}
	}
	void CalculateTime (){
		//Is it am of pm?
		string AMPM = "";
		float minutes = ((currentTime) - (Mathf.Floor(currentTime)))*60.0f;
		if (currentTime <= 12.0f) {
			AMPM = "AM";

		} else {
			AMPM = "PM";
		}
		//Make the final string
		timeString = Mathf.Floor(currentTime).ToString() + " : " + minutes.ToString("F0") + " "+AMPM ;

	}

}
