namespace CSGoap
{
   using Godot;
    public partial class Tree : StaticBody2D
    {    
        [Export] private int hp = 3;
        [Export] private NodePath chopCooldownPath;

        private Timer chopCooldown;

        public override void _Ready()
        {
            chopCooldown = GetNode<Timer>(chopCooldownPath);
        }

        public bool Chop()
        {
            if (!chopCooldown.IsStopped())
            {
                return false;
            }
            hp -= 1;
            if (hp == 0)
            {
                QueueFree();
                return true;
            }
            chopCooldown.Start();
            return false;
        }
    }
}