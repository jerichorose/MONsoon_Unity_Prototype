using Codice.Client.BaseCommands;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(PlayerController))]
public class PC_Inspector : Editor
{
    PlayerController pc;
    private void OnEnable()
    {
        pc = (PlayerController)target;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        GUILayout.Label("Current Gravity Mode: " + pc.CurrentGravityMode.ToString());
        if(pc.gd != null)
        {
            GUILayout.Label(" Contact Count: " + pc.gd.ContactCount.ToString());
            GUILayout.Label("Are you grounded... " + pc.gd.IsGrounded());
        }

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Movement Speed:");
        pc.MoveSpeed = EditorGUILayout.FloatField(pc.MoveSpeed);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Sprint Multiplier:");
        pc.SprintMultiplier = EditorGUILayout.FloatField(pc.SprintMultiplier);
        EditorGUILayout.EndHorizontal();


        pc.jumpSettingsOpen = EditorGUILayout.Foldout(pc.jumpSettingsOpen, "Jump Settings", true);
        if(pc.jumpSettingsOpen)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Use New Jump:");
            pc.UseNewJump = EditorGUILayout.Toggle(pc.UseNewJump);
            EditorGUILayout.EndHorizontal();

            if(pc.UseNewJump)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Jump Strength:");
                pc.NewJumpStrength = EditorGUILayout.FloatField(pc.NewJumpStrength);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Jump Min Time:");
                pc.NewJumpMinTime = EditorGUILayout.FloatField(pc.NewJumpMinTime);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Jump Max Time:");
                pc.NewJumpMaxTime = EditorGUILayout.FloatField(pc.NewJumpMaxTime);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Force Mode:");
                pc.NewForceMode = (CustomForceMode)EditorGUILayout.EnumPopup(pc.NewForceMode);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Rising/Normal Gravity:");
                pc.NewRisingGravity = EditorGUILayout.FloatField(pc.NewRisingGravity);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Falling Gravity:");
                pc.NewFallingGravity = EditorGUILayout.FloatField(pc.NewFallingGravity);
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Jump Force:");
                pc.JumpForce = EditorGUILayout.FloatField(pc.JumpForce);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Force Mode:");
                pc.MyForceMode = (CustomForceMode)EditorGUILayout.EnumPopup(pc.MyForceMode);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Custom Gravity:");
                pc.UsingCustomGravity = EditorGUILayout.Toggle(pc.UsingCustomGravity);
                EditorGUILayout.EndHorizontal();

                if (pc.UsingCustomGravity)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("Rising/Normal Gravity:");
                    pc.RisingGravity = EditorGUILayout.FloatField(pc.RisingGravity);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("Falling Gravity:");
                    pc.FallingGravity = EditorGUILayout.FloatField(pc.FallingGravity);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("Top Arc Gravity:");
                    pc.UsingTopArcGravity = EditorGUILayout.Toggle(pc.UsingTopArcGravity);
                    if (pc.UsingTopArcGravity)
                    {
                        pc.TopArcGravity = EditorGUILayout.FloatField(pc.TopArcGravity);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label("Top Arc Rising Length:");
                        pc.TopArcRising = EditorGUILayout.FloatField(pc.TopArcRising);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label("Top Arc Falling Length:");
                        pc.TopArcFalling = EditorGUILayout.FloatField(pc.TopArcFalling);
                    }
            
                }
                EditorGUILayout.EndHorizontal();
            }

        }

        pc.cameraSettingsOpen = EditorGUILayout.Foldout(pc.cameraSettingsOpen, "Camera Settings", true);
        if(pc.cameraSettingsOpen)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("YAW Sensitivity", "How sensitive the cameras response to horizontal mouse movement is. The minimum value is 100 and the maximum is 1000."));
            pc.YawSensitivity = EditorGUILayout.FloatField(pc.YawSensitivity);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Invert Yaw:", "Inverting Yaw means that moving your mouse left moves the camera right, vice versa. Check to invert Yaw, uncheck otherwise."));
            pc.InvertYaw = EditorGUILayout.Toggle(pc.InvertYaw);
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Pitch Sensitivity:", "How sensitive the cameras response to vertical mouse movement is. The minimum value is 100 and the maximum is 1000."));
            pc.PitchSensitivity = EditorGUILayout.FloatField(pc.PitchSensitivity);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Pitch Min:", "Pitch Min is the lowest vertical angle you want the player to be able to move the Camera. Minimum is -90, Maximum is 90. 0 is directly behind player, 90 is directly above player."));
            pc.PitchMin = EditorGUILayout.FloatField(pc.PitchMin);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Pitch Max:", "Pitch Max is highest vertical angle you want the player to be able to move the Camera.Minimum is -90, Maximum is 90. 0 is directly behind player, 90 is directly above player."));
            pc.PitchMax = EditorGUILayout.FloatField(pc.PitchMax);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Invert Pitch:", "Inverting Pitch means that moving your mouse up moves the camera down, vice versa. Check to invert Pitch, uncheck otherwise."));
            pc.InvertPitch = EditorGUILayout.Toggle(pc.InvertPitch);
            EditorGUILayout.EndHorizontal();

        }
    }
}
