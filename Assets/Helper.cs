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
		//LogState ();
	}

	void Start ()
	{
		StartCoroutine (DoIt ());
		//StartCoroutine (DontDoIt ());
	}

	//	IEnumerator DontDoIt ()
	//	{
	//		yield return new WaitForSeconds (.5f);
	//
	//		Debug.Log ("******* VRSettings.enabled = false");
	//		VRSettings.enabled = false;
	//
	//		yield return new WaitForSeconds (.5f);
	//
	//		Debug.Log ("******* LoadDeviceByName('')...");
	//		VRSettings.LoadDeviceByName ("");
	//
	//		yield return new WaitForSeconds (.5f);
	//
	//		LogState ();
	//	}

	IEnumerator DoIt ()
	{
//		Debug.Log ("******* SteamVR.enabled = true");
//		SteamVR.enabled = true;
//		LogState ();

		if (VRSettings.loadedDeviceName != "OpenVR") {
			//Debug.Log ("******* LoadDeviceByName('OpenVR')...");
			VRSettings.LoadDeviceByName ("OpenVR");
			yield return new WaitForEndOfFrame (); // required to load device
			//LogState ();
		}

		if (!VRSettings.enabled) {
			//Debug.Log ("******* VRSettings.enabled = true");
			VRSettings.enabled = true;
			LogState ();
		}

		//yield return new WaitForSeconds (.5f);

		if (!SteamVR.active) {
			Debug.Log ("******* SteamVR.instance");
			var foo = SteamVR.instance;
		}

		var bar = SteamVR_Render.instance;

		LogState ();

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

	void LogState ()
	{
		Debug.Log (
			"VRSettings(enabled=" + VRSettings.enabled + ",loadedDeviceName=" + VRSettings.loadedDeviceName + ",supportedDevices=[" + String.Join (", ", VRSettings.supportedDevices) + "]), " +
			"VRDevice(isPresent=" + VRDevice.isPresent + ",model=" + VRDevice.model + ",refreshRate=" + VRDevice.refreshRate + ")\n" +
			"SteamVR(active=" + SteamVR.active + ",usingNativeSupport=" + SteamVR.usingNativeSupport + ")," +
			"FindObjectOfType<SteamVR_Render>()=" + GameObject.FindObjectOfType<SteamVR_Render> () + "," +
			"OpenVR(IsHmdPresent()=" + OpenVR.IsHmdPresent () + ",IsRuntimeInstalled()=" + OpenVR.IsRuntimeInstalled () + ")"
		);
	}

}
