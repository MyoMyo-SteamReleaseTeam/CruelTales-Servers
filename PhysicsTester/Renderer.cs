using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using FlatPhysics;

namespace PhysicsTester
{
	public class Renderer
	{
		public FlatVector ScreenSize { get; set; }
		public FlatVector ViewSize => ScreenSize / _zoom;
		public FlatVector ViewLeftBottom => CameraWorldPosition - ViewSize * 0.5f;
		public FlatVector ViewRightTop => CameraWorldPosition + ViewSize * 0.5f;
		private FlatVector _cameraWolrdPosition;
		public FlatVector CameraWorldPosition
		{
			get => _cameraWolrdPosition;
			set
			{
				_cameraWolrdPosition = value;
				ScreenCameraPosition = (CameraWorldPosition - (ViewSize / 2).FlipY()) * _zoom;
			}
		}
		//public FlatVector CameraWorldPosition => (CameraPosition / _zoom + (ViewSize / 2).FlipY());
		public FlatVector ScreenCameraPosition { get; set; } //=> (CameraWorldPosition - (ViewSize / 2).FlipY()) * _zoom;

		private float _zoom = 1;
		public float Zoom
		{
			get => _zoom;
			set
			{
				_zoom = value < 1 ? 1 : value;
				ScreenCameraPosition = (CameraWorldPosition - (ViewSize / 2).FlipY()) * _zoom;
			}
		}

		// Reference
		[AllowNull] private Graphics _graphics;

		// Pen
		private Color _crossColor = Color.DarkGray;
		private Color _defaultColor = Color.White;

		// Font
		private Font _defaultFont = new Font("Aial", 10);

		// Property
		private int _crossHairLength = 1000;

        public Renderer()
        {
		}

		public void BindGraphics(Graphics g)
		{
			_graphics = g;
			DrawOriginCross();
		}

		public FlatVector GetMousePosition(FlatVector screenPos)
		{
			float mx = screenPos.X / ScreenSize.X;
			float my = 1.0f - screenPos.Y / ScreenSize.Y;

			var lb = ViewLeftBottom;
			var rt = ViewRightTop;

			mx = FlatMath.Lerp(lb.X, rt.X, mx);
			my = FlatMath.Lerp(lb.Y, rt.Y, my);

			return new FlatVector(mx, my);
		}

		#region Line

		public void DrawLine(int x1, int y1, int x2, int y2, Color color)
		{
			DrawLine(new FlatVector(x1, y1), new FlatVector(x2, y2), color);
		}

		public void DrawLine(int x1, int y1, int x2, int y2)
		{
			DrawLine(new FlatVector(x1, y1), new FlatVector(x2, y2));
		}

        public void DrawLine(FlatVector p1, FlatVector p2)
		{
			this.DrawLine(p1, p2, _defaultColor);
		}

		public void DrawLine(FlatVector p1, FlatVector p2, Color color)
		{
			p1 *= Zoom;
			p2 *= Zoom;
			p1 = p1.FlipY() - ScreenCameraPosition.FlipY();
			p2 = p2.FlipY() - ScreenCameraPosition.FlipY();
			using (Pen p = new Pen(color))
			{
				_graphics.DrawLine(p, p1.X, p1.Y, p2.X, p2.Y);
			}
		}

		#endregion

		#region Circle

		public void DrawCircleFill(FlatVector center, float radius, Color color)
		{
			center *= Zoom;
			center = center.FlipY() - ScreenCameraPosition.FlipY();
			radius *= Zoom * 2;
			center -= new FlatVector(radius, radius) * 0.5f;
			using (Brush b = new SolidBrush(color))
			{
				_graphics.FillEllipse(b, center.X, center.Y, radius, radius);
			}
		}

		public void DrawCircle(FlatVector center, float radius, Color color)
		{
			center *= Zoom;
			center = center.FlipY() - ScreenCameraPosition.FlipY();
			radius *= Zoom * 2;
			center -= new FlatVector(radius, radius) * 0.5f;
			using (Pen p = new Pen(color))
			{
				_graphics.DrawEllipse(p, center.X, center.Y, radius, radius);
			}
		}

		public void DrawCircleGUI(FlatVector center, float radius, Color color)
		{
			center *= Zoom;
			center = center.FlipY() - ScreenCameraPosition.FlipY();
			center -= new FlatVector(radius, radius) * 0.5f;
			using (Pen p = new Pen(color))
			{
				_graphics.DrawEllipse(p, center.X, center.Y, radius, radius);
			}
		}

		#endregion

		#region Box

		private Point[] _tempPoints = new Point[3];
		public void DrawPolygon(FlatVector[] vertices, int[] triangles, Color color)
		{
			using (Pen p = new Pen(color))
			{
				int triangleCount = triangles.Length / 3;
				for (int i = 0; i < triangleCount; i++)
				{
					for (int v = 0; v < 3; v++)
					{
						int ti = triangles[i * 3 + v];
						FlatVector tv = vertices[ti].FlipY() * Zoom - ScreenCameraPosition.FlipY(); ;
						_tempPoints[v] = new Point((int)tv.X, (int)tv.Y);
					}

					_graphics.DrawPolygon(p, _tempPoints);
				}
			}
		}

		public void DrawPolygonFill(FlatVector[] vertices, int[] triangles, Color color)
		{
			using (Brush b = new SolidBrush(color))
			{
				int triangleCount = triangles.Length / 3;
				for (int i = 0; i < triangleCount; i++)
				{
					for (int v = 0; v < 3; v++)
					{
						int ti = triangles[i * 3 + v];
						FlatVector tv = vertices[ti].FlipY() * Zoom - ScreenCameraPosition.FlipY(); ;
						_tempPoints[v] = new Point((int)tv.X, (int)tv.Y);
					}

					_graphics.FillPolygon(b, _tempPoints);
				}
			}
		}

		public void DrawBoxFill(FlatVector center, float width, float height, Color color)
		{
			center *= Zoom;
			center = center.FlipY() - ScreenCameraPosition.FlipY();

			width *= Zoom;
			height *= Zoom;
			center -= new FlatVector(width, height) * 0.5f;
			
			using (Brush b = new SolidBrush(color))
			{
				_graphics.FillRectangle(b, center.X, center.Y, width, height);
			}
		}

		public void DrawBox(FlatVector center, float width, float height, Color color)
		{
			center *= Zoom;
			center = center.FlipY() - ScreenCameraPosition.FlipY();

			width *= Zoom;
			height *= Zoom;
			center -= new FlatVector(width, height) * 0.5f;

			using (Pen p = new Pen(color))
			{
				_graphics.DrawRectangle(p, center.X, center.Y, width, height);
			}
		}

		public void DrawBoxGUI(FlatVector center, float width, float height, Color color, int outline = 1)
		{
			center *= Zoom;
			center = center.FlipY() - ScreenCameraPosition.FlipY();

			center -= new FlatVector(width, height) * 0.5f;

			using (Pen p = new Pen(color, outline))
			{
				_graphics.DrawRectangle(p, center.X, center.Y, width, height);
			}
		}

		#endregion

		#region Text

		public void DrawTextGUI(string text, FlatVector position, Color color)
		{
			using (SolidBrush b = new SolidBrush(color))
			{
				_graphics.DrawString(text, _defaultFont, b, position.X, position.Y);
			}
		}

		public void DrawText(string text, FlatVector position, Color color)
		{
			position *= Zoom;
			position = position.FlipY() - ScreenCameraPosition.FlipY();

			using (SolidBrush b = new SolidBrush(color))
			{
				_graphics.DrawString(text, _defaultFont, b, position.X, position.Y);
			}
		}

		#endregion

		public void DrawOriginCross()
		{
			this.DrawLine(-_crossHairLength, 0, _crossHairLength, 0, _crossColor);
			this.DrawLine(0, -_crossHairLength, 0, _crossHairLength, _crossColor);
		}
	}
}
