using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;

[CustomEditor(typeof(VRCSDK2.VRC_Station))]
public class VRCPlayerStationEditor : Editor 
{
	VRCSDK2.VRC_Station myTarget;
	VRCSDK2.VRC_EventHandler handler;

	void OnEnable()
	{
		if(myTarget == null)
			myTarget = (VRCSDK2.VRC_Station)target;
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if(handler != null)
			SetStationEventHandlerEvents();
		else
			handler = myTarget.gameObject.GetComponent<VRCSDK2.VRC_EventHandler>();

	}

	private void SetStationEventHandlerEvents()
	{
		if(!handler.Events.Exists(ue => ue.ParameterString == "UseStation"))
		{
			VRCSDK2.VRC_EventHandler.VrcEvent useStationEvent = new VRCSDK2.VRC_EventHandler.VrcEvent();
			useStationEvent.Name = "Use";
			useStationEvent.EventType = VRCSDK2.VRC_EventHandler.VrcEventType.SendMessage;
			useStationEvent.ParameterString = "UseStation";
			useStationEvent.ParameterObject = myTarget.gameObject;

			handler.Events.Add(useStationEvent);
		}

		if(!handler.Events.Exists(ee => ee.ParameterString == "ExitStation"))
		{
			VRCSDK2.VRC_EventHandler.VrcEvent exitStationEvent = new VRCSDK2.VRC_EventHandler.VrcEvent();
			exitStationEvent.Name = "Exit";
			exitStationEvent.EventType = VRCSDK2.VRC_EventHandler.VrcEventType.SendMessage;
			exitStationEvent.ParameterString = "ExitStation";
			exitStationEvent.ParameterObject = myTarget.gameObject;
			
			handler.Events.Add(exitStationEvent);
		}

	}
}