using System;
using Microsoft.Xna.Framework;
using Monocle;
using Microsoft.Xna.Framework.Input;

namespace Celeste.Mod.AnalogViewer
{
    class AnalogDisplay : Entity
    {
        public AnalogDisplay()
        {
            Tag = Tags.HUD;
        }

        public override void Render()
        {
            if (!AnalogModule.Settings.Enabled) return;
            const int labelX = 25;
            const int coordX = 100;
            const int angleX = 425;
            const int arrowX = 525;
            int y = 200;
            GamePadDeadZone deadZone;
            if (AnalogModule.Settings.ShowIndepdentAxes)
            {
                deadZone = GamePadDeadZone.IndependentAxes;
                DrawText("+ : ", labelX, y);
                DrawText(GetAnalogCoordinates(deadZone), coordX, y);
                DrawText(GetAnalogAngle(deadZone), angleX, y);
                MTexture icon = GetIcon();
                icon?.Draw(new Vector2(arrowX, y), Vector2.Zero, Color.White, ActiveFont.LineHeight / icon.Height);
                y += 75;
            }
            if (AnalogModule.Settings.ShowNoDeadZone)
            {
                deadZone = GamePadDeadZone.None;
                DrawText("X:", labelX, y);
                DrawText(GetAnalogCoordinates(deadZone), coordX, y);
                DrawText(GetAnalogAngle(deadZone), angleX, y);
                y += 75;
            }
            if (AnalogModule.Settings.ShowCircularDeadZone)
            {
                deadZone = GamePadDeadZone.Circular;
                DrawText("O:", labelX, y);
                DrawText(GetAnalogCoordinates(deadZone), coordX, y);
                DrawText(GetAnalogAngle(deadZone), angleX, y);
            }
        }

        private String GetAnalogCoordinates(GamePadDeadZone DeadZoneConfig = GamePadDeadZone.IndependentAxes)
        {
            GamePadState gamePadState = GamePad.GetState(0, DeadZoneConfig);
            return String.Format("({0:F2}, {1:F2})", gamePadState.ThumbSticks.Left.X, gamePadState.ThumbSticks.Left.Y);
        }

        private String GetAnalogAngle(GamePadDeadZone DeadZoneConfig = GamePadDeadZone.IndependentAxes)
        {
            GamePadState gamePadState = GamePad.GetState(0, DeadZoneConfig);
            return String.Format("{0:F0}°", gamePadState.ThumbSticks.Left.Angle() * 180.0 / Math.PI);
        }

        public MTexture GetIcon()
        {
            Vector2 corrected = CorrectDashPrecision(Input.LastAim);
            return Input.GuiDirection(corrected);
        }

        private Vector2 CorrectDashPrecision(Vector2 dir)
        {
            if (dir.X != 0f && Math.Abs(dir.X) < 0.001f)
            {
                dir.X = 0f;
                dir.Y = Math.Sign(dir.Y);
            }
            else if (dir.Y != 0f && Math.Abs(dir.Y) < 0.001f)
            {
                dir.Y = 0f;
                dir.X = Math.Sign(dir.X);
            }
            return dir;
        }

        private void DrawText(String text, int x, int y)
        {
            ActiveFont.DrawOutline(text,
                                position: new Vector2(x, y),
                                justify: new Vector2(0f, 0f),
                                scale: Vector2.One,
                                color: Color.White, stroke: 2f,
                                strokeColor: Color.Black);
        }
    }
}
