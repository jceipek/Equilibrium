﻿/************************************************************************************

Filename    :   OVRCameraControllerEditor.cs
Content     :   Player controller interface. 
				This script adds editor functionality to the OVRCameraController
Created     :   March 06, 2013
Authors     :   Peter Giokaris

Copyright   :   Copyright 2013 Oculus VR, Inc. All Rights reserved.

Use of this software is subject to the terms of the Oculus LLC license
agreement provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

************************************************************************************/
#define CUSTOM_LAYOUT

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(QCameraController))]

//-------------------------------------------------------------------------------------
// ***** OVRCameraControllerEditor
//
// OVRCameraControllerEditor adds extra functionality in the inspector for the currently
// selected OVRCameraController.
//
public class QCameraControllerEditor : Editor
{
	// target component
	private QCameraController m_Component;

	// OnEnable
	void OnEnable()
	{
		m_Component = (QCameraController)target;
	}

	// OnDestroy
	void OnDestroy()
	{
	}

	// OnInspectorGUI
	public override void OnInspectorGUI()
	{
		GUI.color = Color.white;
		
		Undo.SetSnapshotTarget(m_Component, "QCameraController");

		{
#if CUSTOM_LAYOUT
			m_Component.CameraRootPosition  = EditorGUILayout.Vector3Field("Camera Root Position", m_Component.CameraRootPosition);
			m_Component.NeckPosition 		= EditorGUILayout.Vector3Field("Neck Position", m_Component.NeckPosition);
			m_Component.EyeCenterPosition 	= EditorGUILayout.Vector3Field("Eye Center Position", m_Component.EyeCenterPosition);
			
			OVREditorGUIUtility.Separator();

			m_Component.UsePlayerEyeHeight  = EditorGUILayout.Toggle ("Use Player Eye Height", m_Component.UsePlayerEyeHeight);
			
			OVREditorGUIUtility.Separator();
			
			m_Component.FollowOrientation = EditorGUILayout.ObjectField("Follow Orientation", 
																		m_Component.FollowOrientation,
																		typeof(Transform), true) as Transform;
			m_Component.TrackerRotatesY 	= EditorGUILayout.Toggle("Tracker Rotates Y", m_Component.TrackerRotatesY);
			
			OVREditorGUIUtility.Separator();	

			// Remove Portrait Mode from Inspector window for now
			//m_Component.PortraitMode        = EditorGUILayout.Toggle ("Portrait Mode", m_Component.PortraitMode);
			m_Component.PredictionOn        = EditorGUILayout.Toggle ("Prediction On", m_Component.PredictionOn);
			m_Component.CallInPreRender     = EditorGUILayout.Toggle ("Call in Pre-Render", m_Component.CallInPreRender);
			m_Component.WireMode     		= EditorGUILayout.Toggle ("Wire-Frame Mode", m_Component.WireMode);
			m_Component.LensCorrection     	= EditorGUILayout.Toggle ("Lens Correction", m_Component.LensCorrection);
			m_Component.Chromatic     		= EditorGUILayout.Toggle ("Chromatic", m_Component.Chromatic);
			
			OVREditorGUIUtility.Separator();
		
			m_Component.BackgroundColor 	= EditorGUILayout.ColorField("Background Color", m_Component.BackgroundColor);
			m_Component.NearClipPlane       = EditorGUILayout.FloatField("Near Clip Plane", m_Component.NearClipPlane);
			m_Component.FarClipPlane        = EditorGUILayout.FloatField("Far Clip Plane", m_Component.FarClipPlane);			
			
			OVREditorGUIUtility.Separator();
#else			 
			DrawDefaultInspector ();
#endif
		}

		if (GUI.changed)
		{
			Undo.CreateSnapshot();
			Undo.RegisterSnapshot();
			EditorUtility.SetDirty(m_Component);
		}
		
		Undo.ClearSnapshotTarget();
	}		
}

