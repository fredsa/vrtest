using UnityEngine;
using System.Collections;
using Valve.VR;
using UnityEngine.VR;
using System;

public class Helper : MonoBehaviour
{

	public SteamVR foo;

	void Awake ()
	{
		Debug.Log ("Awake(): " + state);
		//StartCoroutine (SwitchToOpenVR ());
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.O)) {
			Debug.Log ("Switching to OpenVR...");
			try {
				if (OpenVR.IsHmdPresent ()) {
					StartCoroutine (SwitchToOpenVR ());
				}
			} catch (Exception e) {
				Debug.Log ("VR NOT POSSIBLE: " + e);
			}
		}
		if (Input.GetKeyDown (KeyCode.N)) {
			Debug.Log ("Switching to None...");
			StartCoroutine (SwitchToNone ());
		}
	}

	IEnumerator SwitchToNone ()
	{
		SteamVR_Render steamVR_RenderInstance = SteamVR_Render.instance;

		if (VRSettings.loadedDeviceName != "") {
			Debug.Log ("******* LoadDeviceByName(\"None\")...");
			VRSettings.LoadDeviceByName ("None");
			yield return new WaitForEndOfFrame (); // required to load device
			Debug.Log ("SwitchToNone(): " + state);
		}

		if (VRSettings.enabled) {
			Debug.Log ("******* VRSettings.enabled = false");
			VRSettings.enabled = false;
			yield return new WaitForEndOfFrame (); // required for None device to become active
			Debug.Log ("SwitchToNone(): " + state);
		}

		if (steamVR_RenderInstance != null) {
			Debug.Log ("******* Destroy (steamVR_RenderInstance.gameObject)");
			Destroy (steamVR_RenderInstance.gameObject);
		}

		if (SteamVR.active) {
			Debug.LogError ("SteamVR.active");
			Debug.Log ("******* SteamVR.SafeDispose()");
			SteamVR.SafeDispose ();
		}

		Debug.Log ("******* Camera.main.ResetFieldOfView()");
		Camera.main.ResetFieldOfView ();

//		Debug.Log ("******* SteamVR.enabled = false");
//		SteamVR.enabled = false;


		Debug.Log ("SwitchToNone(): " + state);
	}

	IEnumerator SwitchToOpenVR ()
	{
		if (VRSettings.loadedDeviceName != "OpenVR") {
			Debug.Log ("******* LoadDeviceByName(\"OpenVR\")...");
			VRSettings.LoadDeviceByName ("OpenVR");
			yield return new WaitForEndOfFrame (); // required to load device
			Debug.Log ("SwitchToOpenVR(): " + state);
			yield return new WaitForEndOfFrame (); // second wait needed if Coroutine() started from Update()
			Debug.Log ("SwitchToOpenVR(): " + state);
		}

		if (!VRSettings.enabled) {
			Debug.Log ("******* VRSettings.enabled = true");
			VRSettings.enabled = true;
			////////////////////////////////yield return new WaitForEndOfFrame (); // required for OpenVR device to become active
			Debug.Log ("SwitchToOpenVR(): " + state);
		}

		Debug.Log ("******* SteamVR.enabled = true");
		SteamVR.enabled = true; //required on second enable

		Debug.Log ("******* SteamVR.instance");
		if (SteamVR.instance == null) {
			Debug.LogError ("SteamVR.instance == null");
			yield return null;
		}

		Debug.Log ("******* steamVR_Render.instance");
		if (SteamVR_Render.instance == null) {
			Debug.LogError ("steamVR_Render.instance == null");
			yield return null;
		}

		Debug.Log ("SwitchToOpenVR(): " + state);
	
		RegisterListeners ();
	}

	void RegisterListeners ()
	{
		Debug.Log ("******* REGISTERING LISTENERS...");
		SteamVR_Utils.Event.Listen ("initializing", OnInitializing);
		SteamVR_Utils.Event.Listen ("calibrating", OnCalibrating);
		SteamVR_Utils.Event.Listen ("out_of_range", OnOutOfRange);
		SteamVR_Utils.Event.Listen ("device_connected", OnDeviceConnected);
	}

	private void OnInitializing (params object[] args)
	{
		Debug.Log ("####### OnInitializing" + AsString (args));
	}

	private void OnCalibrating (params object[] args)
	{
		Debug.Log ("####### OnCalibrating" + AsString (args));
	}

	private void OnOutOfRange (params object[] args)
	{
		Debug.Log ("####### OnOutOfRange" + AsString (args));
	}

	private void OnDeviceConnected (params object[] args)
	{
		Debug.Log ("####### OnDeviceConnected" + AsString (args));
	}

	string AsString (object[] args)
	{
		string t = "";
		foreach (object arg in args) {
			t += arg.ToString () + ", ";
		}
		return "(" + t.Substring (0, t.Length - 2) + ")";
	}

	string state {
		get {
			string t =	"VRSettings(enabled=" + VRSettings.enabled + ",loadedDeviceName=" + VRSettings.loadedDeviceName + ",supportedDevices=[" + String.Join (", ", VRSettings.supportedDevices) + "]), " +
			           "VRDevice(isPresent=" + VRDevice.isPresent + ",model=" + VRDevice.model + ",refreshRate=" + VRDevice.refreshRate + ")\n" +
			           "SteamVR(active=" + SteamVR.active + ",usingNativeSupport=" + SteamVR.usingNativeSupport + ")," +
			           "FindObjectOfType<SteamVR_Render>()=" + GameObject.FindObjectOfType<SteamVR_Render> () + ",";
			try {
				t += "OpenVR(IsHmdPresent()=" + OpenVR.IsHmdPresent () + ",IsRuntimeInstalled()=" + OpenVR.IsRuntimeInstalled () + ")";
			} catch (Exception e) {
				t += "OpenVR(" + e + ")";
			}
			return t;
		}
	}

}
