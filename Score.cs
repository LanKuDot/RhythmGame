/* Filename: Score.cs
 * Update and display the score of the game.
 */

using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour
{
	/* Make the updateScore() could be called directly. */
	public static Score Instance;

	private GameObject scoreGUI;
	private int score;

	void Awake()
	{
		Instance = this;
		// Set reference to displaing text.
		scoreGUI = GameObject.Find( "ScoreGUI" );
	}

	void Start()
	{
		// Initialize the score
		score = 0;
	}
	
	public void updateScore( int deltaScore )
	{
		score += deltaScore;
		// Display the new score.
		scoreGUI.guiText.text = "Score: " + score;
	}
}	// end of class Score
