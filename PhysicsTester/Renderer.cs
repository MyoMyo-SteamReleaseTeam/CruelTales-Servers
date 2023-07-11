using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using KaNet.Physics;

namespace PhysicsTester
{
	public class Renderer
	{
		public Vector2 ScreenSize { get; set; }
		public Vector2 ViewSize => ScreenSize / _zoom;
		public Vector2 ViewLeftBottom => CameraWorldPosition - ViewSize * 0.5f;
		public Vector2 ViewRightTop => CameraWorldPosition + ViewSize * 0.5f;
		private Vector2 _cameraWolrdPosition;
		public Vector2 CameraWorldPosition
		{
			get => _cameraWolrdPosition;
			set
			{
				_cameraWolrdPosition = value;
				ScreenCameraPosition = (CameraWorldPosition - (ViewSize / 2).FlipY()) * _zoom;
			}
		}
		public Vector2 ScreenCameraPosition { get; set; }
		public BoundingBox _cameraViewBound;

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
		public Font DefaultFont10 { get; private set; } = new Font("Aial", 10);
		public Font DefaultFont16 { get; private set; } = new Font("Aial", 16);

		// Property
		private int _crossHairLength = 1000;
		private int _textCullingSize = 3;

        public Renderer()
        {
		}

		public void BindGraphics(Graphics g)
		{
			_graphics = g;
			DrawOriginCross();
		}

		public void OnBeforeDraw()
		{
			_cameraViewBound = new BoundingBox(CameraWorldPosition,
											   ViewSize.X, ViewSize.Y);
		}

		public Vector2 GetMousePosition(Vector2 screenPos)
		{
			float mx = screenPos.X / ScreenSize.X;
			float my = 1.0f - screenPos.Y / ScreenSize.Y;

			var lb = ViewLeftBottom;
			var rt = ViewRightTop;

			mx = KaMath.Lerp(lb.X, rt.X, mx);
			my = KaMath.Lerp(lb.Y, rt.Y, my);

			return new Vector2(mx, my);
		}

		#region Line

		public void DrawLine(int x1, int y1, int x2, int y2, Color color)
		{
			DrawLine(new Vector2(x1, y1), new Vector2(x2, y2), color);
		}

		public void DrawLine(int x1, int y1, int x2, int y2)
		{
			DrawLine(new Vector2(x1, y1), new Vector2(x2, y2));
		}

        public void DrawLine(Vector2 p1, Vector2 p2)
		{
			this.DrawLine(p1, p2, _defaultColor);
		}

		public void DrawLine(Vector2 p1, Vector2 p2, Color color)
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

		public void DrawCircleFill(Vector2 center, float radius, Color color)
		{
			if (IsCulled(center, radius))
				return;

			center *= Zoom;
			center = center.FlipY() - ScreenCameraPosition.FlipY();
			radius *= Zoom * 2;
			center -= new Vector2(radius, radius) * 0.5f;

			using (Brush b = new SolidBrush(color))
			{
				_graphics.FillEllipse(b, center.X, center.Y, radius, radius);
			}
		}

		public void DrawCircle(Vector2 center, float radius, Color color)
		{
			if (IsCulled(center, radius))
				return;

			center *= Zoom;
			center = center.FlipY() - ScreenCameraPosition.FlipY();
			radius *= Zoom * 2;
			center -= new Vector2(radius, radius) * 0.5f;

			using (Pen p = new Pen(color))
			{
				_graphics.DrawEllipse(p, center.X, center.Y, radius, radius);
			}
		}

		public void DrawCircleGUI(Vector2 center, float radius, Color color)
		{
			center *= Zoom;
			center = center.FlipY() - ScreenCameraPosition.FlipY();
			center -= new Vector2(radius, radius) * 0.5f;

			using (Pen p = new Pen(color))
			{
				_graphics.DrawEllipse(p, center.X, center.Y, radius, radius);
			}
		}

		#endregion

		#region Polygon

		private Point[] _tempPoints = new Point[3];
		public void DrawPolygon(Vector2[] vertices, int[] triangles, Color color)
		{
			if (IsCulled(vertices))
				return;

			using (Pen p = new Pen(color))
			{
				int triangleCount = triangles.Length / 3;
				for (int i = 0; i < triangleCount; i++)
				{
					for (int v = 0; v < 3; v++)
					{
						int ti = triangles[i * 3 + v];
						Vector2 tv = vertices[ti].FlipY() * Zoom - ScreenCameraPosition.FlipY(); ;
						_tempPoints[v] = new Point((int)tv.X, (int)tv.Y);
					}

					_graphics.DrawPolygon(p, _tempPoints);
				}
			}
		}

		public void DrawPolygonFill(Vector2[] vertices, int[] triangles, Color color)
		{
			if (IsCulled(vertices))
				return;

			using (Brush b = new SolidBrush(color))
			{
				int triangleCount = triangles.Length / 3;
				for (int i = 0; i < triangleCount; i++)
				{
					for (int v = 0; v < 3; v++)
					{
						int ti = triangles[i * 3 + v];
						Vector2 tv = vertices[ti].FlipY() * Zoom - ScreenCameraPosition.FlipY(); ;
						_tempPoints[v] = new Point((int)tv.X, (int)tv.Y);
					}

					_graphics.FillPolygon(b, _tempPoints);
				}
			}
		}

		#endregion

		#region Box

		public void DrawBoxFill(Vector2 center, float width, float height, Color color)
		{
			if (IsCulled(center, width, height))
				return;

			center *= Zoom;
			center = center.FlipY() - ScreenCameraPosition.FlipY();

			width *= Zoom;
			height *= Zoom;
			center -= new Vector2(width, height) * 0.5f;

			using (Brush b = new SolidBrush(color))
			{
				_graphics.FillRectangle(b, center.X, center.Y, width, height);
			}
		}

		public void DrawBox(Vector2 center, float width, float height, Color color)
		{
			if (IsCulled(center, width, height))
				return;

			center *= Zoom;
			center = center.FlipY() - ScreenCameraPosition.FlipY();

			width *= Zoom;
			height *= Zoom;
			center -= new Vector2(width, height) * 0.5f;

			using (Pen p = new Pen(color))
			{
				_graphics.DrawRectangle(p, center.X, center.Y, width, height);
			}
		}

		public void DrawBoxGUI(Vector2 center, float width, float height, Color color, int outline = 1)
		{
			center *= Zoom;
			center = center.FlipY() - ScreenCameraPosition.FlipY();

			center -= new Vector2(width, height) * 0.5f;

			using (Pen p = new Pen(color, outline))
			{
				_graphics.DrawRectangle(p, center.X, center.Y, width, height);
			}
		}

		#endregion

		#region Text

		public void DrawTextGUI(string text, Vector2 position, Color color, Font? font = null)
		{
			Font f = font is null ? DefaultFont10 : font;
			using (SolidBrush b = new SolidBrush(color))
			{
				_graphics.DrawString(text, f, b, position.X, position.Y);
			}
		}

		public void DrawText(string text, Vector2 position, Color color,
							 bool isCenter = false, Font? font = null)
		{
			if (IsCulled(position, _textCullingSize, _textCullingSize))
				return;

			Font f = font is null ? DefaultFont10 : font;

			position *= Zoom;
			position = position.FlipY() - ScreenCameraPosition.FlipY();

			if (isCenter)
			{
				SizeF textSize = this._graphics.MeasureString(text, f);
				position.X -= textSize.Width * 0.5f;
				position.Y -= textSize.Height * 0.5f;
			}

			using (SolidBrush b = new SolidBrush(color))
			{
				_graphics.DrawString(text, f, b, position.X, position.Y);
			}
		}


		public void DrawText(string text, Vector2 position, Color foreground, Color outlineColor,
							 bool isCenter = false, Font? font = null)
		{
			if (IsCulled(position, _textCullingSize, _textCullingSize))
				return;

			Font f = font is null ? DefaultFont10 : font;

			position *= Zoom;
			position = position.FlipY() - ScreenCameraPosition.FlipY();

			if (isCenter)
			{
				SizeF textSize = this._graphics.MeasureString(text, f);
				position.X -= textSize.Width * 0.5f;
				position.Y -= textSize.Height * 0.5f;
			}

			using (SolidBrush b = new SolidBrush(outlineColor))
			{
				_graphics.DrawString(text, f, b, position.X + 1, position.Y + 1);
				_graphics.DrawString(text, f, b, position.X - 1, position.Y + 1);
				_graphics.DrawString(text, f, b, position.X + 1, position.Y - 1);
				_graphics.DrawString(text, f, b, position.X - 1, position.Y - 1);
			}

			using (SolidBrush b = new SolidBrush(foreground))
			{
				_graphics.DrawString(text, f, b, position.X, position.Y);
			}
		}

		#endregion

		public void DrawOriginCross()
		{
			this.DrawLine(-_crossHairLength, 0, _crossHairLength, 0, _crossColor);
			this.DrawLine(0, -_crossHairLength, 0, _crossHairLength, _crossColor);
		}

		public bool IsCulled(Vector2 worldPos, float width, float height)
		{
			BoundingBox objBound = new BoundingBox(worldPos, width, height);
			return !KaPhysics.IsIntersectAABBs(_cameraViewBound, objBound);
		}

		public bool IsCulled(Vector2 worldPos, float radius)
		{
			float diameter = radius * 2;
			BoundingBox objBound = new BoundingBox(worldPos, diameter, diameter);
			return !KaPhysics.IsIntersectAABBs(_cameraViewBound, objBound);
		}

		public bool IsCulled(Vector2[] vertices)
		{
			float minX = float.MaxValue;
			float minY = float.MaxValue;
			float maxX = float.MinValue;
			float maxY = float.MinValue;

			foreach (Vector2 v in vertices)
			{
				minX = MathF.Min(minX, v.X);
				minY = MathF.Min(minY, v.Y);
				maxX = MathF.Max(maxX, v.X);
				maxY = MathF.Max(maxY, v.Y);
			}

			BoundingBox objBound = new(new Vector2(minX, minY),
									   new Vector2(maxX, maxY));

			return !KaPhysics.IsIntersectAABBs(_cameraViewBound, objBound);
		}
	}
}
