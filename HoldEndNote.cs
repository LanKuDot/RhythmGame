﻿using UnityEngine;
using System.Collections;

public class HoldEndNote : MonoBehaviour
{
	public Sprite[] sprites;
	private SpriteRenderer spriteRenderer;
	private int delayingFrames;		// The number of frames in delaying
	private int rotatingFrames;		// The number of frames in rotating
	private int totalFrames;		// = rotatingFrames + delayingFrames
	private float degreePerFrame;	// The degree rotating per frame
	private bool gotNewHoldTime;	// Dose it get the new holding time?
	private int index;
	private Color color;			// The colot setting of the sprite

	// Use this for initialization
	void Awake ()
	{
		spriteRenderer = renderer as SpriteRenderer;
		color = spriteRenderer.material.color;

		// Rotate in 1 beat
		rotatingFrames = ( int )GameConfig.framePerBeats;
		degreePerFrame = 90.0f / ( float )rotatingFrames;

		gameObject.SetActive( false );
	}
	
	/* Time.fixedDeltaTime is defined at NoteBank. */
	void FixedUpdate ()
	{
		if ( gotNewHoldTime )
		{
			spriteRenderer.sprite = sprites[0];

			++index;
			if ( index == totalFrames )
				gameObject.SetActive( false );

			// Fade-in effect
			if ( color.a != 1.0f )
			{
				color.a += 0.1f;
				spriteRenderer.material.color = color;
			}

			if ( index > delayingFrames )
				spriteRenderer.transform.Rotate( Vector3.forward * degreePerFrame );
		}
	}

	/* Be called from HoldNote.
	 * Set the total frames the note would display according to the holding beats.
	 */
	public void setHoldingFrames( int duringFrames )
	{
		totalFrames = duringFrames;
		delayingFrames = totalFrames - rotatingFrames;

		gotNewHoldTime = true;
	}

	/* Reset on disable */
	void OnDisable()
	{
		// Reset
		gotNewHoldTime = false;
		index = 0;
		spriteRenderer.transform.eulerAngles = new Vector3( 0.0f, 0.0f, 90.0f );
		color.a = 0.0f;
	}
}