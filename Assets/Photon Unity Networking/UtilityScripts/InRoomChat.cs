﻿using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class InRoomChat : Photon.MonoBehaviour 
{
    public Rect GuiRect = new Rect(0,0, 250,300);
    public bool IsVisible = true;
    public bool AlignBottom = false;
    public List<string> messages = new List<string>();
    private string inputLine = "";
	private Vector2 scrollPos = new Vector2(0, Mathf.Infinity);  //Vector2.zero;

	private bool isWindowVisible = false;
	public Rect windowHome = new Rect(20, 20, 300, 500);
	private bool isInputLineFocused = false;

    public static readonly string ChatRPC = "Chat";

    public void Start()
    {
        if (this.AlignBottom)
        {
            this.GuiRect.y = Screen.height - this.GuiRect.height;
        }

		windowHome = new Rect(20, 20, 300, 400);
	}

	public void OnGUI(){

		if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.M))
			isWindowVisible = !isWindowVisible;

		if (isWindowVisible)
		{
			windowHome = GUI.Window (0, windowHome, WindowFunctionHome, " ");
		}

		if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
		{
			if (!string.IsNullOrEmpty(this.inputLine))
			{
				this.photonView.RPC("Chat", PhotonTargets.All, this.inputLine);
				this.inputLine = "";
				GUI.FocusControl("");
				isInputLineFocused = false;
				return; // printing the now modified list would result in an error. to avoid this, we just skip this single frame
			}
			else
			{
				GUI.FocusControl("ChatInput");
				isInputLineFocused = true;
			}
		}

	}

    //public void OnGUI()
	void WindowFunctionHome (int windowID)
    {

        if (!this.IsVisible || PhotonNetwork.connectionStateDetailed != PeerState.Joined)
        {
            return;
        }
        
		if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Escape))
		{
			//this.inputLine = "";
			GUI.FocusControl("");
			isInputLineFocused = false;
			return; // printing the now modified list would result in an error. to avoid this, we just skip this single frame
		}

		if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
        {
            if (!string.IsNullOrEmpty(this.inputLine))
            {
                this.photonView.RPC("Chat", PhotonTargets.All, this.inputLine);
                this.inputLine = "";
                GUI.FocusControl("");
				isInputLineFocused = false;
                return; // printing the now modified list would result in an error. to avoid this, we just skip this single frame
            }
            else
            {
                GUI.FocusControl("ChatInput");
				isInputLineFocused = true;
            }
        }
		
        GUI.SetNextControlName("");
        GUILayout.BeginArea(this.GuiRect);
		GUI.color = Color.black;

		if (isInputLineFocused) {
			scrollPos = GUILayout.BeginScrollView (new Vector2 (0, Mathf.Infinity));
		}
		else
		{
			scrollPos = GUILayout.BeginScrollView (scrollPos);
		}
		GUILayout.FlexibleSpace();

        ///for (int i = messages.Count - 1; i >= 0; i--)
        ///{
        ///    GUILayout.Label(messages[i]);
        ///}
        
		for (int i = 0 ; i < messages.Count ; i++)
		{
			GUILayout.Label(messages[i]);
		}

        GUILayout.EndScrollView();

        GUILayout.BeginHorizontal();
        GUI.SetNextControlName("ChatInput");
        inputLine = GUILayout.TextField(inputLine);
        if (GUILayout.Button("Send", GUILayout.ExpandWidth(false)))
        {
            this.photonView.RPC("Chat", PhotonTargets.All, this.inputLine);
            this.inputLine = "";
            GUI.FocusControl("");
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
		GUI.DragWindow();
        
    }

    [RPC]
    public void Chat(string newLine, PhotonMessageInfo mi)
    {
        string senderName = "anonymous";

        if (mi != null && mi.sender != null)
        {
            if (!string.IsNullOrEmpty(mi.sender.name))
            {
                senderName = mi.sender.name;
            }
            else
            {
                senderName = "player " + mi.sender.ID;
            }
        }

        this.messages.Add(senderName +": " + newLine);
    }

    public void AddLine(string newLine)
    {
        this.messages.Add(newLine);
    }
}
