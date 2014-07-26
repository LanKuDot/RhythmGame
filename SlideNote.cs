/* Filename: SlideNote.cs
 * Play the hint animation of the slide note.
 * Indicate the direction of the SlideNote.
 */

using UnityEngine;
using System.Collections;

public class SlideNote : MonoBehaviour
{
	public Sprite[] sprites;
	private SpriteRenderer spriteRenderer;
	private int index;
	private int delayingFrames = 48;
	private int delayedFrames;			// The counter for the delayed frames
	private int waitingFrames;			// The waiting frames from waken up to playing animation
	private Color color;

	// Use this for initialization
	void Start ()
	{
		spriteRenderer = renderer as SpriteRenderer;
		color = spriteRenderer.material.color;

		gameObject.SetActive( false );
	}
	
	/* Time.fixedDeltaTime is defined at NoteBank */
	/* The compeleted animation: Slide Hint Start --[1 beat]-> Slide Hint Stay
	 * --[1 beat]-> Start Sliding --[1 beat]-> End Sliding.
	 */
	void FixedUpdate ()
	{
		if ( waitingFrames == 0 )
		{
			spriteRenderer.sprite = sprites[index];
			++index;
			if ( index == 47 )
			{
				gameObject.SetActive( false );
			}
			if ( index == 24 && delayedFrames != delayingFrames )
			{
				--index;
				++delayedFrames;

				if ( color.a != 1.0f )
				{
					color.a += 0.1f;
					spriteRenderer.material.color = color;
				}
			}
		}
		else
			--waitingFrames;
	}

	public void setWaitingFrames( int frames )
	{
		waitingFrames = frames;
	}

	/* Reset on disable */
	void OnDisable()
	{
		// Make the program show the direction directly, not gradully
		index = 23;

		delayedFrames = 0;
		color.a = 0.0f;
		spriteRenderer.material.color = color;
	}
}
