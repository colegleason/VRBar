using UnityEditor;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;

public class VRCSdkBuilder : MonoBehaviour 
{
	static Dictionary<System.Type,string> Depricated;

	static void InitializeDepricated()
	{
		Depricated = new Dictionary<System.Type, string>();

		Depricated.Add( typeof( AvatarDescriptor ), "Replace with VRC_AvatarDescriptor from the VRCSDK2 DLL." );
		Depricated.Add( typeof( VRC_AvatarDescriptor ), "This script must be assigned from the VRCSDK2 DLL." );
		Depricated.Add( typeof( VRC_KeyEvents ), "This script must be assigned from the VRCSDK2 DLL." );
		Depricated.Add( typeof( VRC_PhysicsRoot ), "This script must be assigned from the VRCSDK2 DLL." );

		Depricated.Add( typeof( VRC_AddDamage ), "This script must be assigned from the VRCSDK2 DLL." );
		Depricated.Add( typeof( VRC_AddHealth ), "This script must be assigned from the VRCSDK2 DLL." );
		Depricated.Add( typeof( VRC_AvatarPedestal ), "This script must be assigned from the VRCSDK2 DLL." );
		Depricated.Add( typeof( VRC_GunStats ), "This script must be assigned from the VRCSDK2 DLL." );
		Depricated.Add( typeof( VRC_JukeBox ), "This script must be assigned from the VRCSDK2 DLL." );
		Depricated.Add( typeof( VRC_PortalMarker ), "This script must be assigned from the VRCSDK2 DLL." );
		Depricated.Add( typeof( VRC_SimplePhysics ), "This script must be assigned from the VRCSDK2 DLL." );
		Depricated.Add( typeof( VRC_SlideShow ), "This script must be assigned from the VRCSDK2 DLL." );
		Depricated.Add( typeof( VRC_SyncAnimation ), "This script must be assigned from the VRCSDK2 DLL." );

		Depricated.Add( typeof( VRC_PlayerMods ), "This script must be assigned from the VRCSDK2 DLL." );
		Depricated.Add( typeof( VRC_SceneDescriptor ), "This script must be assigned from the VRCSDK2 DLL." );
		Depricated.Add( typeof( VRC_SceneResetPosition ), "This script must be assigned from the VRCSDK2 DLL." );
		Depricated.Add( typeof( VRC_SceneSmoothShift ), "This script must be assigned from the VRCSDK2 DLL." );
		Depricated.Add( typeof( VRC_Station ), "This script must be assigned from the VRCSDK2 DLL." );
		Depricated.Add( typeof( VRC_TimedEvents ), "This script must be assigned from the VRCSDK2 DLL." );
		Depricated.Add( typeof( VRC_TriggerColliderEventTrigger ), "This script must be assigned from the VRCSDK2 DLL." );
		Depricated.Add( typeof( VRC_UseEvents ), "This script must be assigned from the VRCSDK2 DLL." );

		Depricated.Add( typeof( VRC_DataStorage ), "This script must be assigned from the VRCSDK2 DLL." );
		Depricated.Add( typeof( VRC_EventDispatcherLocal ), "Please remove this script. It's not necessary." );
		Depricated.Add( typeof( VRC_EventHandler ), "This script must be assigned from the VRCSDK2 DLL." );
	}

	[MenuItem("VRChat/Build Custom Avatar from Selection")]
	static void ExportAvatarResource () 
	{
		VRCSDK2.VRC_AvatarDescriptor Desc = ( Selection.activeObject as GameObject ).GetComponent<VRCSDK2.VRC_AvatarDescriptor>();
		if( Desc == null )
		{
			UnityEditor.EditorUtility.DisplayDialog("Build Custom Avatar", "You must place a VRC_AvatarDescriptor on the root of you custom scene", "Ok" );
			return;
		}

		if( CheckCompatibility( Selection.activeObject as GameObject ) > 0 )
		{
			UnityEditor.EditorUtility.DisplayDialog("Build Custom Avatar", "Your avatar scene contains depricated scripts. See the error log.", "Ok" );
			return;
		}

		GameObject ScenePrefab = PrefabUtility.CreatePrefab( "Assets/_CustomAvatar.prefab", Selection.activeObject as GameObject );
		
		string path = EditorUtility.SaveFilePanel ("Save Custom Avatar", "", "NewAvatar", "vrca");
		if (path.Length != 0) 
		{
			BuildTarget InitialTarget = EditorUserBuildSettings.activeBuildTarget;
			BuildPipeline.BuildAssetBundle(
				ScenePrefab, null, path, 
				BuildAssetBundleOptions.CollectDependencies | 
				BuildAssetBundleOptions.CompleteAssets);
			EditorUserBuildSettings.SwitchActiveBuildTarget( InitialTarget );
		}
		
		AssetDatabase.DeleteAsset( "Assets/_CustomAvatar.prefab" );
		AssetDatabase.Refresh();

		VRCSDK2.VRC_Editor.RecordActivity( "avatar", Path.GetFileName( path ) );
	}

	[MenuItem("VRChat/Build Custom Scene from Selection")]
	static void ExportSceneResource () 
	{
		VRCSDK2.VRC_SceneDescriptor Desc = ( Selection.activeObject as GameObject ).GetComponent<VRCSDK2.VRC_SceneDescriptor>();
		if( Desc == null )
		{
			UnityEditor.EditorUtility.DisplayDialog("Build Custom Scene", "You must place a VRC_SceneDescriptor on the root of you custom scene", "Ok" );
			return;
		}
		if( Desc.spawns.Length < 1 )
		{
			UnityEditor.EditorUtility.DisplayDialog("Build Custom Scene", "You must add at least one spawn to spawns in your VRC_SceneDescriptor.", "Ok" );
			return;
		}

		if( CheckCompatibility( Selection.activeObject as GameObject ) > 0 )
		{
			UnityEditor.EditorUtility.DisplayDialog("Build Custom Scene", "Your scene contains depricated scripts. See the error log.", "Ok" );
			return;
		}

		ListDynamicPrefabs( Desc.DynamicPrefabs, Selection.activeObject as GameObject );
		const int SyncIdStart = 500;
		const int SyncIdMax = 500;
		int SyncCount = AssignSyncIds( SyncIdStart, Selection.activeObject as GameObject ) - SyncIdStart;
		if( SyncCount > SyncIdMax )
		{
			UnityEditor.EditorUtility.DisplayDialog("Build Custom Scene", "Your scene contains too many VRC_ObjectApi scripts for synchronization.", "Ok" );
			return;
		}

		Desc.LightMode = LightmapSettings.lightmapsMode;
		switch( LightmapSettings.lightmapsMode )
		{
		case LightmapsMode.Single:
			Desc.LightMapsFar = new Texture2D[ LightmapSettings.lightmaps.Length ];
			Desc.LightMapsNear = null;
			break;
		case LightmapsMode.Dual:
			Desc.LightMapsFar = new Texture2D[ LightmapSettings.lightmaps.Length ];
			Desc.LightMapsNear = new Texture2D[ LightmapSettings.lightmaps.Length ];
			break;
		case LightmapsMode.Directional:
			Desc.LightMapsFar = new Texture2D[ LightmapSettings.lightmaps.Length ];
			Desc.LightMapsNear = new Texture2D[ LightmapSettings.lightmaps.Length ];
			break;
		}
		
		for( int i = 0; i < LightmapSettings.lightmaps.Length; ++i )
		{
			Desc.LightMapsFar[i] = LightmapSettings.lightmaps[i].lightmapFar;
			if( Desc.LightMapsNear != null )
				Desc.LightMapsNear[i] = LightmapSettings.lightmaps[i].lightmapNear;
		}
		
		Desc.LoadRenderSettings = true;
		Desc.RenderAmbientLight = RenderSettings.ambientLight;
		Desc.RenderSkybox = RenderSettings.skybox;
		Desc.RenderFog = RenderSettings.fog;
		Desc.RenderFogColor = RenderSettings.fogColor;
		Desc.RenderFogMode = RenderSettings.fogMode;
		Desc.RenderFogDensity = RenderSettings.fogDensity;
		Desc.RenderFogLinearStart = RenderSettings.fogStartDistance;
		Desc.RenderFogLinearEnd = RenderSettings.fogEndDistance;
		
		GameObject ScenePrefab = PrefabUtility.CreatePrefab( "Assets/_CustomScene.prefab", Selection.activeObject as GameObject );
		
		string path = EditorUtility.SaveFilePanel ("Save Custom Scene", "", "NewScene", "vrcs");
		if (path.Length != 0) 
		{
			BuildTarget InitialTarget = EditorUserBuildSettings.activeBuildTarget;

			BuildTarget assetBundleTarget = BuildTarget.WebPlayer;
			if(InitialTarget == BuildTarget.Android)
				assetBundleTarget = BuildTarget.Android;

			BuildPipeline.BuildAssetBundle(
				ScenePrefab, 
				null, 
				path, 
				BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets,
				assetBundleTarget
			);

			EditorUserBuildSettings.SwitchActiveBuildTarget( InitialTarget );
		}
		
		AssetDatabase.DeleteAsset( "Assets/_CustomScene.prefab" );
		AssetDatabase.Refresh();

		VRCSDK2.VRC_Editor.RecordActivity( "scene", Path.GetFileName( path ) );
	}

	static int CheckCompatibility( GameObject obj )
	{
		if( Depricated == null )
			InitializeDepricated();

		int failures = 0;
		bool res = true;

		Component[] components = obj.GetComponents<Component>();
		foreach( Component c in components )
		{
			if( c == null )
				UnityEngine.Debug.LogWarning( "Object contains missing script references.", obj );
			else if( Depricated.ContainsKey( c.GetType() ) )
			{
				++failures;
				UnityEngine.Debug.LogError( c.GetType().ToString() + ": " + Depricated[ c.GetType() ], obj );
			}
		}

		for( int i = 0; i < obj.transform.childCount; ++i )
			failures += CheckCompatibility( obj.transform.GetChild(i).gameObject );

		return failures;
	}

	static void ListDynamicPrefabs( List<GameObject> list, GameObject obj )
	{
		VRCSDK2.VRC_ObjectSpawn[] spawns = obj.GetComponents<VRCSDK2.VRC_ObjectSpawn>();
		foreach( VRCSDK2.VRC_ObjectSpawn s in spawns )
		{
			if( list.Contains( s.ObjectPrefab ) == false )
				list.Add( s.ObjectPrefab );
		}

		for( int i = 0; i < obj.transform.childCount; ++i )
			ListDynamicPrefabs( list, obj.transform.GetChild(i).gameObject );
	}

	static int AssignSyncIds( int id, GameObject obj )
	{
		VRCSDK2.VRC_ObjectSync[] syncObjects = obj.GetComponents<VRCSDK2.VRC_ObjectSync>();
		foreach( VRCSDK2.VRC_ObjectSync o in syncObjects )
			o.networkId = id++;

		for( int i = 0; i < obj.transform.childCount; ++i )
			id = AssignSyncIds( id, obj.transform.GetChild(i).gameObject );

		return id;
	}
}
