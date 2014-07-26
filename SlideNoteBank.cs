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

	/* Be called from NoteBank.Update().
	 * Get the information of the slide note from NoteBank.
	 */
	public void updateNote( int from, int to )
	{
		nodeNotes[from].gameObject.SetActive( true );
		nodeNotes[from].setWaitingFrames( 0 );
		nodeNotes[to].gameObject.SetActive( true );
		nodeNotes[to].setWaitingFrames( (int)GameConfig.framePerBeats );
		slideNotes[from * 9 + to].gameObject.SetActive( true );
		slideNotes[from * 9 + to].setWaitingFrames( 0 );
	}
}
