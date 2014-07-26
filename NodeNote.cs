/* Filename: NodeNote.cs
 * Play the animation of the node note.
 */

using UnityEngine;
using System.Collections;

public class NodeNote : MonoBehaviour
{
	public Sprite[] sprites;
	private SpriteRenderer spriteRenderer;
	private float degreePerFrame;
	private int rotatingFrames;
	private int delayingFrames;		// The number of frames in delaying
	private int totalFrames;		// = rotatingFrame + delayingFrame
	private int index;				// The index of frame
	private int waitingFrames;		// The waiting frames from waken up to playing animation
	private Color color;			// The color setting of the renderer

	// Use this for initialization
	void Start ()
	{
		spriteRenderer = renderer as SpriteRenderer;
		
		// The click note would rotate 90 degrees in 2 beats
		rotatingFrames = ( int )GameConfig.framePerBeats * 2;
		degreePerFrame = 90.0f / ( float )rotatingFrames;
		// Delaying some frames to avoid the ClickNote disappered suddenly
		delayingFrames = 10;
		// Calculate total frames
		totalFrames = rotatingFrames + delayingFrames;
		
		// Get the color setting of the renderer and initialize the alpha to 0
		color = spriteRenderer.material.color;

		gameObject.SetActive( false );
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		if ( waitingFrames == 0 )
		{
			spriteRenderer.sprite = sprites[0];
			// Rotate the click note per frame
			if ( index < rotatingFrames )
				spriteRenderer.transform.Rotate( Vector3.forward * degreePerFrame );

			// Fade in effect
			if ( color.a != 1.0f )
			{
				color.a += 0.1f;
				spriteRenderer.material.color = color;
			}

			++index;
			// If the animation ends, sleep.
			if ( index == totalFrames )
				gameObject.SetActive( false );
		}
		else
			--waitingFrames;
	}

	public void setWaitingFrames( int frames )
	{
		waitingFrames = frames;
	}

	void OnDisable()
	{
		// Reset Index
		index = 0;
		spriteRenderer.transform.eulerAngles = new Vector3( 0.0f, 0.0f, 90.0f );
		// Reset alpha value
		color.a = 0.0f;
		spriteRenderer.material.color = color;

		waitingFrames = 999;
	}
}
