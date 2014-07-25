/* Filename: SlideNoteBank.cs
 * Get the information of the slide note from NoteBank, and
 * control when to play the hint of the slide note.
 * A slide note is conposed of SlideNotes ( The direction ) and NodeNotes ( The node ).
 */

using UnityEngine;
using System.Collections;

public class SlideNoteBank : MonoBehaviour
{
	public NodeNote[] nodeNotes = new NodeNote[GameConfig.numOfTapNotes];
	// The index of the element of the array: from * 9 + to
	public SlideNote[] slideNotes = new SlideNote[81];
	private int frame_i;				// The counter for the delayed frames
	private int toNode;
	// The number of frames from the "from" note waked up to "to" note waked up
	private int delayingFrames = ( int )GameConfig.framePerBeats;	// 1 beat
	private bool gotNewNote = false;	// Is the note updated?

	/* Time.fixedDeltaTime is defined at NoteBank. */
	/* Delay 1 frame to wake up the "to" node.
	 */
	void FixedUpdate ()
	{
		if ( gotNewNote )
			++frame_i;
		if ( frame_i == delayingFrames )
			// If the toNode has been waken up, this line is useless.
			// If the slide note is the last one, the "to" note would be waken up. Otherwise,
			// the "to" note would be waken up in the "from" note of the next SlideNote.
			nodeNotes[toNode].gameObject.SetActive( true );
	}

	/* Be called from NoteBank.Update().
	 * Get the information of the slide note from NoteBank.
	 */
	public void updateNote( int from, int to )
	{
		toNode = to;
		nodeNotes[from].gameObject.SetActive( true );
		slideNotes[from * 9 + to].gameObject.SetActive( true );
		// Reset the counter to wake up the "to" Note
		frame_i = 0;
		gotNewNote = true;
	}
}
