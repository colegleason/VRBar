using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;

namespace VRCSDK2
{
	[CustomEditor(typeof(VRCSDK2.VRC_PlayerMods))]
	public class VRCPlayerModsEditor : Editor 
	{
		VRCSDK2.VRC_PlayerMods myTarget;
		VRCSDK2.VRC_EventHandler handler;

		void OnEnable()
		{
			if(myTarget == null)
				myTarget = (VRCSDK2.VRC_PlayerMods)target;
		}

		public override void OnInspectorGUI()
		{
			if(handler != null)
			{
				if(myTarget.isRoomPlayerMods)
					handler.enabled = false;
				else
					handler.enabled = true;
			}
			else
			{
				handler = myTarget.gameObject.GetComponent<VRCSDK2.VRC_EventHandler>();
			}

			myTarget.isRoomPlayerMods = EditorGUILayout.Toggle("isRoomPlayerMods", myTarget.isRoomPlayerMods);

			if(GUILayout.Button("Add Remove Mods Event"))
			{
				SetRemovePlayerModsEventHandlerEvent();
			}

			if(GUILayout.Button("Add Mod"))
			{
				SetAddPlayerModsEventHandlerEvent();
				VRCPlayerModEditorWindow.Init(myTarget, delegate() 
				{
					Repaint();
				});
			}
			
			List<VRCPlayerMod> playerMods = myTarget.playerMods;
			for(int i=0; i<playerMods.Count; ++i)
			{
				VRCSDK2.VRCPlayerMod mod = playerMods[i];
				EditorGUILayout.BeginVertical("box");
				EditorGUILayout.LabelField(mod.name, EditorStyles.boldLabel);
				if( mod.allowNameEdit )
					mod.name = EditorGUILayout.TextField( "Mod Name: ", mod.name );
				for(int j=0; j<mod.properties.Count; ++j)
				{
					VRCSDK2.VRCPlayerModProperty prop = mod.properties[j];
					myTarget.playerMods[i].properties[j] = DrawFieldForProp(prop);
				}
				if(GUILayout.Button ("Remove Mod"))
				{
					myTarget.RemoveMod(mod);
					break;
				}
				EditorGUILayout.EndVertical();
			}
		}

		VRCSDK2.VRCPlayerModProperty DrawFieldForProp(VRCSDK2.VRCPlayerModProperty property)
		{
			if(property.type.SystemType == typeof(int))
			{
				property.intValue = EditorGUILayout.IntField(property.name, property.intValue);
			}
			else if(property.type.SystemType == typeof(float))
			{
				property.floatValue = EditorGUILayout.FloatField(property.name, property.floatValue);
			}
			else if(property.type.SystemType == typeof(string))
			{
				property.stringValue = EditorGUILayout.TextField(property.name, property.stringValue);
			}
			else if(property.type.SystemType == typeof(bool))
			{
				property.boolValue = EditorGUILayout.Toggle(property.name, property.boolValue);
			}
			else if(property.type.SystemType == typeof(GameObject))
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField( property.name );
				property.gameObjectValue = (GameObject) EditorGUILayout.ObjectField( property.gameObjectValue, typeof( GameObject ), true );
				EditorGUILayout.EndHorizontal();
			}
			else if(property.type.SystemType == typeof(KeyCode))
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField( property.name );
				property.keyCodeValue = (KeyCode) EditorGUILayout.EnumPopup( property.keyCodeValue );
				EditorGUILayout.EndHorizontal();
			}
			else if(property.type.SystemType == typeof(VRCSDK2.VRC_EventHandler.VrcBroadcastType))
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField( property.name );
				property.broadcastValue = (VRCSDK2.VRC_EventHandler.VrcBroadcastType) EditorGUILayout.EnumPopup( property.broadcastValue );
				EditorGUILayout.EndHorizontal();
			}
			else if(property.type.SystemType == typeof(VRCSDK2.VRCPlayerModFactory.HealthOnDeathAction))
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField( property.name );
				property.onDeathActionValue = (VRCSDK2.VRCPlayerModFactory.HealthOnDeathAction) EditorGUILayout.EnumPopup( property.onDeathActionValue);
				EditorGUILayout.EndHorizontal();
			}
			else if(property.type.SystemType == typeof(RuntimeAnimatorController))
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField( property.name );
				property.animationController = (RuntimeAnimatorController) EditorGUILayout.ObjectField( property.animationController, typeof( RuntimeAnimatorController ), false );
				EditorGUILayout.EndHorizontal();
			}
			return property;
		}

		private void SetAddPlayerModsEventHandlerEvent()
		{
			if(!handler.Events.Exists(ae => ae.ParameterString == "AddPlayerMods"))
			{
				VRCSDK2.VRC_EventHandler.VrcEvent useEvent = new VRCSDK2.VRC_EventHandler.VrcEvent();
				useEvent.Name = "Use";
				useEvent.EventType = VRCSDK2.VRC_EventHandler.VrcEventType.SendMessage;
				useEvent.ParameterString = "AddPlayerMods";
				useEvent.ParameterObject = myTarget.gameObject;
				
				handler.Events.Add(useEvent);
			}
		}
		
		private void SetRemovePlayerModsEventHandlerEvent()
		{
			if(!handler.Events.Exists(re => re.ParameterString == "RemovePlayerMods"))
			{
				VRCSDK2.VRC_EventHandler.VrcEvent useEvent = new VRCSDK2.VRC_EventHandler.VrcEvent();
				useEvent.Name = "Use";
				useEvent.EventType = VRCSDK2.VRC_EventHandler.VrcEventType.SendMessage;
				useEvent.ParameterString = "RemovePlayerMods";
				useEvent.ParameterObject = myTarget.gameObject;
				
				handler.Events.Add(useEvent);
			}
		}
	}
}