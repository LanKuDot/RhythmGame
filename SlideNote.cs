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
	private int delayingFrames = 24;
	private int delayedFrames;			// The counter for the delayed frames

	// Use this for initialization
	void Start ()
	{
		spriteRenderer = renderer as SpriteRenderer;

		gameObject.SetActive( false );
	}
	
	/* Time.fixedDeltaTime is defined at NoteBank */
	/* The compeleted animation: Slide Hint Start --[1 beat]-> Slide Hint Stay
	 * --[1 beat]-> Start Sliding --[1 beat]-> End Sliding.
	 */
	void FixedUpdate ()
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
		}
	}

	/* Reset on disable */
	void OnDisable()
	{
		index = 0;
		delayedFrames = 0;
	}
}
