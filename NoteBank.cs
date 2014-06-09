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
		public GameConfig.NoteTypes which;
		public int howLong;
		/* For example, who = 2 and when = 6 means
		 * TapPoint no.2 wakes the PrepareNote at beat 6.
		 */
		// Constructor
		public Note( int when, int who, GameConfig.NoteTypes which, int howLong = 0 )
		{
			this.who = who;
			this.when = when;
			this.which = which;
			this.howLong = howLong;
		}
	};

	public TapPoint[] tapPoints
		= new TapPoint[GameConfig.numOfTapNotes];	// Direct reference to all TapPoints

	private int beatCounter = -1;	// The counter counts beats after the song start playing.
	private int nextBeat = 0;		// The next beat that PrepareNote would appear.
	private int noteTableIndex = 0;	// The index for reading note table.
	private Note nextNote;			// The next note that would appear.

	private float BPM = GameConfig.songBPM;		// The BPM of the song
	private float beatTime;			// Realtime interval of single beat

	private bool noteTableEnds = false;	// Indicate that there is no more note to read.

	// Note table: for SimpleBeats140
	Note[] simpleBeats140 = {
		/* How to create an array of struct with initial value like C lauguage... */
		//	Note( when, who, which, howLong )
		new Note( 3, 0, GameConfig.NoteTypes.CLICK ),
		new Note( 4, 2, GameConfig.NoteTypes.CLICK ),
		new Note( 5, 3, GameConfig.NoteTypes.HOLD, 2 ),
		new Note( 6, 5, GameConfig.NoteTypes.HOLD, 4 ),
		new Note( 7, 6, GameConfig.NoteTypes.CLICK ),
		new Note( 8, 8, GameConfig.NoteTypes.CLICK ),
		new Note( 9, 0, GameConfig.NoteTypes.CLICK ),
		new Note( 10, 1, GameConfig.NoteTypes.CLICK ),
		new Note( 11, 4, GameConfig.NoteTypes.CLICK ),
		new Note( 12, 5, GameConfig.NoteTypes.CLICK ),
		new Note( 14, 6, GameConfig.NoteTypes.CLICK ),
		new Note( 16, 7, GameConfig.NoteTypes.CLICK ),
		new Note( 18, 0, GameConfig.NoteTypes.CLICK ),
		new Note( 7, 4, GameConfig.NoteTypes.CLICK )	// Dummy Note
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
				tapPoints[ nextNote.who ].wakeUpPrepareNote( nextNote.which, nextNote.howLong );
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
