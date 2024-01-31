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
            //int arrowX = 525;
            //int arrowY = 200;
            int arrowX = (int) Math.Round((float) Engine.Instance.GraphicsDevice.PresentationParameters.BackBufferWidth / (float) 2);
            int arrowY = (int) Math.Round((float) Engine.Instance.GraphicsDevice.PresentationParameters.BackBufferHeight / (float) 2);
            int y = 200;
            Vector2 arrow = new Vector2(arrowX, arrowY);
            Vector2 offset = Vector2.Zero;
            GamePadDeadZone deadZone;
            Level level = SceneAs<Level>();

            if (AnalogModule.Settings.ShowIndepdentAxes)
            {
                deadZone = GamePadDeadZone.IndependentAxes;
                DrawText("+ : ", labelX, y);
                DrawText(GetAnalogCoordinates(deadZone), coordX, y);
                DrawText(GetAnalogAngle(deadZone), angleX, y);
                MTexture icon = GetIcon();
                if (level.Tracker.GetEntity<Player>() != null) { arrow = level.WorldToScreen(level.Tracker.GetEntity<Player>().TopLeft); }
                arrow = Vector2.Add(arrow, GetAnalogCoordinatesVec2(deadZone) * 100);
                icon?.Draw(arrow, Vector2.Zero, Color.White, ActiveFont.LineHeight / icon.Height);
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

        private Vector2 GetAnalogCoordinatesVec2(GamePadDeadZone DeadZoneConfig = GamePadDeadZone.IndependentAxes)
        {
            GamePadState gamePadState = GamePad.GetState(0, DeadZoneConfig);
            return new Vector2(gamePadState.ThumbSticks.Left.X, -gamePadState.ThumbSticks.Left.Y);
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
}
