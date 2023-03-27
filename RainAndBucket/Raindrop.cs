// /*
//  * Project: RainAndBucket
//  *
//  * Date:   03 06, 2023 5:43 PM
//  *
//  * An Astralbytes, Brunasystems Production.
//  * Author: Phosphor(Jordan Grignon, horuuendillus@gmail.com)
//  */

using Godot;

public partial class Raindrop : Area2D
{
    // also allow rain drop acceleration to be set in the editor.
    [Export] public float Acceleration = 10;

    // create a group for the raindrop properties that are to be exported
    [ExportGroup("Raindrop Properties")]

    // allow raindrop initial speed to be set in the inspector pane of the Godot editor
    [Export]
    public float Speed = 400;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // handle are entered signal when it is emitted
        AreaEntered += area => OnAreaEntered(area);
    }

    /// <summary>
    /// Handles when the raindrop is about to leave the view screen by falling through the bottom..
    /// Because the raindrop is no longer visible we remove it, freeing any resources it is using.
    /// </summary>
    private void OnLeavingViewPort()
    {
        //remove the rain drop from the Node tree when it leaves the scene
        GetTree().QueueDelete(this);
    }

    /// <summary>
    /// Handles the signal emitted when another Area2D object collides with this object.  A check is made to see if the
    /// colliding object is the player and if so removes the raindrop from the scene.
    /// </summary>
    /// <param name="area">The Area2D object that has collided with this rain drop</param>
    private void OnAreaEntered(Area2D area)
    {
        // check if the colliding area is the player, so that rain drops colliding with each other
        // don't trigger their deletion from the tree.
        if (area.Name.Equals("Player"))
        {
            // Player caught the drop so remove it from the Node tree
            GetTree().QueueDelete(this);
        }
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        // create an empty Vector2 , equivalent to new Vector2()
        Vector2 velocity = Vector2.Zero;

        // move the drop along the Y axis
        velocity.Y += 1;

        // calculate the how far the drop has moved
        velocity = velocity * Speed;

        // get the current position
        Vector2 currentPosition = Position;

        // increase the position by movement size times elapsed time
        currentPosition += velocity * (float) delta;

        // if the center of the raindrop is at the bottom of the screen
        if (currentPosition.Y >= 800)
        {
            //remove it
            OnLeavingViewPort();
        }
        else
        {
            // set the new rain drop position
            Position = currentPosition;

            // increase speed so the drop continues to accelerate on the way down.
            Speed += Acceleration;
        }
    }
}