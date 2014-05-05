/**
 * Filename: NoteBank.cs
 * - Contain note table of songs.
 * - The beat counter.
 * - Assign notes to TapPoints.
 */
using UnityEngine;
using System.Collections;

public class NoteBank : MonoBehaviour
{
	// Data structure of a note
	public struct Note
	{
		public int when;	// When to start
		public int who;		// Who will get a note
		/* For example, who = 2 and when = 6 means
		 * TapPoint no.2 wakes the PrepareNote at beat 6.
		 */
		// Constructor
		public Note( int when, int who )
		{
			this.who = who;
			this.when = when;
		}
	};

	public TapPoint[] tapPoints = new TapPoint[9];	// Direct reference to all TapPoints

	private int beatCounter = -1;	// The counter counts beats after the song start playing.
	private int nextBeat = 0;		// The next beat that PrepareNote would appear.
	private int noteTableIndex = 0;	// The index for reading note table.
	private Note nextNote;			// The next note that would appear.

	private float BPM = 140f;		// The BPM of the song
	private float beatTime;			// Realtime interval of single beat

	private bool noteTableEnds = false;	// Indicate that there is no more note to read.

	// Note table: for SimpleBeats140
	Note[] simpleBeats140 = {
		/* How to create an array of struct with initial value like C lauguage... */
		//	Note( when, who )
		new Note( 3, 0 ),
		new Note( 3, 2 ),
		new Note( 4, 3 ),
		new Note( 4, 5 ),
		new Note( 5, 6 ),
		new Note( 5, 8 ),
		new Note( 7, 0 ),
		new Note( 7, 1 ),
		new Note( 8, 4 ),
		new Note( 8, 5 ),
		new Note( 9, 6 ),
		new Note( 9, 7 ),
		new Note( 11, 0 ),
		new Note( 11, 2 ),
		new Note( 12, 0 ),
		new Note( 12, 2 ),
		new Note( 7, 4 )	// Dummy Note
	};

	// Use this for initialization
	void Start ()
	{
		// Calculate the real time interval of single beat
		beatTime = 60f / BPM;
		// Get the first Note from note table
		updateNote();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if ( Time.timeSinceLevelLoad > ( float ) beatTime * beatCounter )
		{
			++beatCounter;
			while( !noteTableEnds && nextBeat == beatCounter )
			{
				// Tell the TapPoint to wake a PrepareNote up.
				tapPoints[ nextNote.who ].wakeUpPrepareNote();
				updateNote();
			}
		}
	}

	// Get the next note from note table
	void updateNote()
	{
		if ( noteTableIndex < simpleBeats140.Length )
			nextNote = simpleBeats140[ noteTableIndex++ ];
		if ( noteTableIndex == simpleBeats140.Length )
			noteTableEnds = true;
		nextBeat = nextNote.when;
	}

}	// end of class NoteBank
