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
		public int howLong;	// For HoldNote: The holding time in beats
							// For SlideNote: The destination note

		// Constructor
		public Note( int when, int who, GameConfig.NoteTypes which, int howLong = 0 )
		{
			this.who = who;
			this.when = when - 2;	// Each Note has 2 beats to prepare
			this.which = which;
			this.howLong = howLong;
		}
	};

	public TapPoint[] tapPoints
		= new TapPoint[GameConfig.numOfTapNotes];	// Direct reference to all TapPoints
	public SlideNoteBank slideNoteBank;		// Direct reference to SlideNoteBank to assign slide note

	private int beatCounter = -1;	// The counter counts beats after the song start playing.
	private int nextBeat = 0;		// The next beat that PrepareNote would appear.
	private int noteTableIndex = 0;	// The index for reading note table.
	private Note nextNote;			// The next note that would appear.

	private float BPM = GameConfig.songBPM;		// The BPM of the song
	private float beatTime;			// Realtime interval of single beat

	private bool noteTableEnds = false;	// Indicate that there is no more note to read.

	// Alias of the type of note
	private static GameConfig.NoteTypes CLICK = GameConfig.NoteTypes.CLICK;
	private static GameConfig.NoteTypes HOLD = GameConfig.NoteTypes.HOLD;
	private static GameConfig.NoteTypes SLIDE = GameConfig.NoteTypes.SLIDE;

	// Note table: for SimpleBeats140
	Note[] simpleBeats140 = {
		/* How to create an array of struct with initial value like C lauguage... */
		//	Note( when, who, which, howLong )
		new Note(  6, 3, CLICK ),
		new Note(  6, 5, CLICK ),
		new Note( 10, 1, SLIDE, 6 ),
		new Note( 11, 6, SLIDE, 5 ),
		new Note( 12, 0, CLICK ),
		new Note( 12, 2, CLICK ),
		new Note( 12, 5, SLIDE, 0 ),
		new Note( 13, 0, SLIDE, 7 ),
		new Note( 14, 7, SLIDE, 2 ),
		new Note( 14, 6, CLICK ),
		new Note( 14, 8, CLICK ),
		new Note( 15, 2, SLIDE, 3 ),
		new Note( 16, 0, CLICK ),
		new Note( 16, 3, SLIDE, 8 ),
		new Note( 17, 1, CLICK ),
		new Note( 17, 8, SLIDE, 1 ),
		new Note( 18, 2, CLICK ),
		new Note( 19, 6, CLICK ),
		new Note( 20, 7, CLICK ),
		new Note( 21, 8, CLICK ),
		new Note( 22, 3, CLICK ),
		new Note( 22, 5, CLICK ),
		new Note( 23, 0, CLICK ),
		new Note( 24, 3, CLICK ),
		new Note( 25, 6, CLICK ),
		new Note( 26, 2, CLICK ),
		new Note( 27, 5, CLICK ),
		new Note( 28, 8, CLICK ),
		new Note( 29, 4, HOLD, 1 ),
		new Note( 30, 3, CLICK ),
		new Note( 30, 5, CLICK ),
		new Note( 32, 0, CLICK ),
		new Note( 33, 4, CLICK ),
		new Note( 34, 8, CLICK ),
		new Note( 35, 2, CLICK ),
		new Note( 36, 4, CLICK ),
		new Note( 37, 6, CLICK ),
		new Note( 38, 3, CLICK ),
		new Note( 38, 5, CLICK ),
		new Note( 39, 6, CLICK ),
		new Note( 40, 4, CLICK ),
		new Note( 41, 2, CLICK ),
		new Note( 42, 8, CLICK ),
		new Note( 43, 4, CLICK ),
		new Note( 44, 0, CLICK ),
		new Note( 46, 0, CLICK ),
		new Note( 46, 2, CLICK ),
		new Note( 47, 6, HOLD, 1 ),
		new Note( 47, 8, HOLD, 1 ),
		new Note( 50, 3, CLICK ),
		new Note( 50, 5, CLICK ),
		new Note( 51, 6, HOLD, 1 ),
		new Note( 51, 8, HOLD, 1 ),
		new Note( 54, 3, CLICK ),
		new Note( 54, 5, CLICK ),
		new Note( 55, 6, HOLD, 1 ),
		new Note( 55, 8, HOLD, 1 ),
		new Note( 58, 3, CLICK ),
		new Note( 58, 5, CLICK ),
		new Note( 59, 6, HOLD, 1 ),
		new Note( 59, 8, HOLD, 1 ),
		new Note( 65, 0, CLICK ),
		new Note( 66, 2, CLICK ),
		new Note( 67, 6, CLICK ),
		new Note( 68, 8, CLICK ),
		new Note( 70, 3, CLICK ),
		new Note( 70, 5, CLICK ),
		new Note( 73, 8, CLICK ),
		new Note( 74, 6, CLICK ),
		new Note( 75, 2, CLICK ),
		new Note( 76, 0, CLICK ),
		new Note( 78, 3, CLICK ),
		new Note( 78, 5, CLICK ),
		new Note( 80, 4, HOLD, 3 ),
		new Note( 84, 3, CLICK ),
		new Note( 84, 5, CLICK ),
		new Note( 7, 4, CLICK )	// Dummy Note
	};

	// Use this for initialization
	void Start ()
	{
		// Calculate the real time interval of single beat
		beatTime = 60f / BPM;
		// Setting the time interval of each frame
		Time.fixedDeltaTime = beatTime / GameConfig.framePerBeats;
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
				if ( nextNote.which == SLIDE )
				{
					slideNoteBank.updateNote( nextNote.who, nextNote.howLong );
				}
				// Tell the TapPoint to wake a PrepareNote up.
				else
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
