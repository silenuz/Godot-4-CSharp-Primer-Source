// /*
//  * Project: RainAndBucket
//  *
//  * Date:   03 06, 2023 12:52 PM
//  *
//  * An Astralbytes, Brunasystems Production.
//  * Author: Phosphor(Jordan Grignon, horuuendillus@gmail.com)
//  */

using System;
using Godot;

public partial class MainScene : Node2D
{
	// track how much time has elapsed since the last rain drop
	private double _elapsedTime;

	// cache the raindrop scene so we can instantiate it as needed
	private PackedScene _rainDrop;

	// random number generator to generate rain drop placement on the x axis
	private Random _randomNumber = new Random();

	// cache the audio player so we can access it without using GetNode each time
	AudioStreamPlayer audioPlayer;

	// Export spawn rate so it is possible to change value in the Godot editor
	// This value is how many seconds to wait before spawning a new rain drop
	[Export]
	public double SpawnRate = 1; // default is to spawn a new drop each second,

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// cache the audio player
		audioPlayer = (AudioStreamPlayer) GetNode("RainSounds");
		
		// handle the emit of the finished signal
		audioPlayer.Finished += () => OnRainSoundsFinished();
		
		// cache the raindrop scene
		_rainDrop =  GD.Load<PackedScene>("res://raindrop.tscn");
	}

	/// <summary>
	///  Handles the emit of OnRainSoundsFinished by the RainSounds node.  When the signal is received
	/// it simply restarts the audio stream.
	/// </summary>
	private void OnRainSoundsFinished()
	{ 
		// play rain sounds from the beginning
		audioPlayer.Play(0);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// add delta to the elapsed time
		_elapsedTime += delta;
		
		// if elapsed time is greater than spawn rate , spawn a new rain drop
		// and reset elapsed time
		if (_elapsedTime >= SpawnRate)
		{
			SpawnRaindrop();
			_elapsedTime = 0;
		}
	}


	/// <summary>
	/// Spawns a new rain drop with a random location on the X axis
	/// </summary>
	private void SpawnRaindrop()
	{
		// instantiate the raindrop scene
		Raindrop drop = (Raindrop) _rainDrop.Instantiate();
		
		// set the position to be random along the x axis
		drop.Position = new Vector2(_randomNumber.Next(16,448), 0);
		
		// add the rain drop scene to the main scene
		AddChild(drop);
	}
}
