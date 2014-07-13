/*
 * Filename: TouchingEvent.cs
 * Dectect touching from mouse or mobile screen, and
 * then check if the touched point is overlapping
 * on the game object with Coillder2D component.
 */

using UnityEngine;
using System.Collections;

public class TouchingEvent : MonoBehaviour
{
	// Running environment
	RuntimePlatform platform = Application.platform;
	bool isTouchingDevice;

	// Use this for initialization
	void Start ()
	{
		// Is the running device has touching screen?
		if ( platform == RuntimePlatform.Android || 
		    platform == RuntimePlatform.IPhonePlayer )
			isTouchingDevice = true;
		else if ( platform == RuntimePlatform.WindowsEditor )
			isTouchingDevice = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if ( isTouchingDevice )
		{
			if ( Input.touchCount > 0 )		// Someone touched.
			{
				int totalTouch = Input.touchCount;
				// Scan all touch point
				for ( int i = 0; i < totalTouch; ++i )
				{
					if ( Input.GetTouch(i).phase == TouchPhase.Began )
						checkTouch( Input.GetTouch(i).position, TouchPhase.Began );
					else if ( Input.GetTouch (i).phase == TouchPhase.Ended )
						checkTouch( Input.GetTouch(i).position, TouchPhase.Ended );
				}
			}
		}	// end of if ( isTouchingDevice )
		else
		{
			if ( Input.GetMouseButtonDown(0) )	// Left clicked
				checkTouch( Input.mousePosition, TouchPhase.Began );
			else if ( Input.GetMouseButtonUp(0) )	// End of left holding
				checkTouch( Input.mousePosition, TouchPhase.Ended );
		}
	}	// end of Update()

	// Checking if the touching point is overlapping TapPoints.
	void checkTouch( Vector2 pos, TouchPhase touchState )
	{
		// Transform the screen position to world ( real ) position.
		Vector3 worldPoint = Camera.main.ScreenToWorldPoint( pos );
		// This game is 2D game.
		Vector2 touchPos_world = new Vector2( worldPoint.x, worldPoint.y );
		/* Checking if overlapping something, checking only on "TapPoint" layer.
		 * Note that the target gameObject you want to check must have a Collider2D
		 * component. */
		int layerMask = 1 << 8;		// Layer mask for layer(8): TapPoint
		Collider2D hit = Physics2D.OverlapPoint( touchPos_world, layerMask );
		// If the touched point is on the TapPoint.
		if ( hit )
		{
			Debug.Log ( hit.transform.gameObject.name );
			// Send message to the touched object to run function "touched"
			switch ( touchState )
			{
			case TouchPhase.Began:
				hit.transform.gameObject.SendMessage( "touched", null, SendMessageOptions.DontRequireReceiver );
				break;
			case TouchPhase.Ended:
				hit.transform.gameObject.SendMessage( "touchEnded", null, SendMessageOptions.DontRequireReceiver );
				break;
			}
		}
	}

}	// end of class TouchingEvent
