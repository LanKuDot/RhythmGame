/**
 * - Filename: PrepareNote.cs
 * - The lifetime of PrepareNote:
 * 1. Sleep after initializing.
 * 2. Sleeping until waked by TapPoint, and then playing the animation.
 * 3. If the animation ends, sleep.
 * 4. Goto 2.
 * - To increase the performance of the PrepareNote,
 * the frame of PrepareNote would update every fixed time interval,
 * not busy calling.
 * Hence, using FixedUpdate() instead of Update().
 */
using UnityEngine;
using System.Collections;

public class PrepareNote : MonoBehaviour
{
	public Sprite[] sprites;	// Array storing all frames of prepareNote
	private SpriteRenderer spriteRenderer;
	private int index;			// The index of frame

	// Use this for initialization
	void Start ()
	{
		spriteRenderer = renderer as SpriteRenderer;
		/* - Calculate the fixedDeltaTime for FixedUpdate().
		 * - The formula of framesPerSec is
		 * n * BPM / x / 60,
		 * which n is the number of frames,
		 * BPM is the bpm of the song,
		 * and x is the time interval ( in beats ) for playing
		 * the entire frames of prepareNotes.
		 * - fixedDeltaTime ( the interval that FixedUpdate() called,
		 * and FixedUpdate() would update the frame of PrepareNote ) 
		 * equals to 1 / framesPerSec.
		 */
		// Demo: x = 2, BPM = 140.
		Time.fixedDeltaTime = ( float ) 60.0f * 2.0f / 140.0f / sprites.Length;
		// Initialize the index number of frame
		index = 0;
		// Sleeping after finished initializing.
		gameObject.SetActive( false );
	}
	
	// FixedUpdate() is called every framerate rate
	void FixedUpdate ()
	{
		// Update to next frame.
		spriteRenderer.sprite = sprites[ index ];
		++index;
		// If the animation ends, sleep.
		gameObject.SetActive( false );
	}

	// OnDisable() is called when the game object becomes inactive.
	void OnDisable()
	{
		// Reset index to 0
		index = 0;
		/* Set the render frame to 0.
		 * The render of PrepareNote begins before OnEnable() called
		 * when the PrepareNote becomes active.
		 * If the reset of the index is in OnEnable(),
		 * the render would start at the last stop frame.
		 */
		spriteRenderer.sprite = sprites[0];
	}

}	// end of class PrepareNote
