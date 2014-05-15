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
	private int position_ID;	// The index of the position of PrepareNotes

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
		 * - About ( sprites.Length - 5 ):
		 * Because I added 5 more frames after the best tapping timing, and
		 * the original frames would be played in 2 beats, calulating the
		 * fixedDeltaTime must minus 5 ( the # of frames added ).
		 */
		// Demo: x = 2, BPM = 140.
		Time.fixedDeltaTime = ( float ) 60.0f * 2.0f / GameConfig.songBPM / ( sprites.Length - 5 );
		/* Initialize the index number of frame by assigning a garbage value to
		 * avoid grading after initialization. PrepareNote will call OnDisable()
		 * which sends the stop frame to Grader, but there is no note assigned
		 * to TapPoint. The Grader will discard the invaild value.
		 */
		index = sprites.Length + 10;
		// Setting position ID
		position_ID = gameObject.name[12];
		position_ID -= 48;
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
		if ( index == sprites.Length )
			gameObject.SetActive( false );
	}

	// OnDisable() is called when the game object becomes inactive.
	void OnDisable()
	{
		Grader.Instance.grading( position_ID, index );
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
