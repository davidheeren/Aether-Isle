using Game;
using Utilities;

namespace Inventory
{
    public abstract class Tool : Useable
    {
        readonly ToolData toolData;

        Timer durationTimer;
        Timer cooldownTimer;

        bool isEntered;

        protected Tool(ToolData toolData, ActorComponents components) : base(toolData, components)
        {
            this.toolData = toolData;
        }

        public override void Equip()
        {
            base.Equip();

            durationTimer = new Timer(toolData.durationTime).ForceDone();
            cooldownTimer = new Timer(toolData.cooldownTime);
        }

        public override bool ShouldUse()
        {
            if (!cooldownTimer.IsDone)
                return false;

            bool input = InputManager.Instance.input.Game.Use.WasPressedThisFrame() && !isEntered;

            return !durationTimer.IsDone || input;
        }

        public override void Enter()
        {
            base.Enter();

            durationTimer.Reset();
            isEntered = true;
        }

        public override void Exit()
        {
            base.Exit();

            cooldownTimer.Reset();
            isEntered = false;
        }
    }
}
