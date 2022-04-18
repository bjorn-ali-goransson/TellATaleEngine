using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenRA.Primitives;

namespace OpenRA.Graphics
{
	public class PolygonRenderable : IRenderable, IFinalizedRenderable
	{
		readonly WPos pos;
		readonly WVec offset;
		readonly int zOffset;
		readonly bool isDecoration;

		public PolygonRenderable(WPos pos, WVec offset, int zOffset, bool isDecoration)
		{
			this.pos = pos;
			this.offset = offset;
			this.zOffset = zOffset;
			this.isDecoration = isDecoration;
		}

		public WPos Pos => pos + offset;
		public int ZOffset => zOffset;
		public bool IsDecoration => isDecoration;

		public IRenderable WithZOffset(int newOffset) { return new PolygonRenderable(pos, offset, newOffset, isDecoration); }
		public IRenderable OffsetBy(in WVec vec) { return new PolygonRenderable(pos + vec, offset, zOffset, isDecoration); }
		public IRenderable AsDecoration() { return new PolygonRenderable(pos, offset, zOffset, true); }

		public IFinalizedRenderable PrepareRender(WorldRenderer wr) { return this; }

		public void Render(WorldRenderer wr)
		{


			var color = Color.Red;
			var closed = true;

			var width = 1f;
			var blendMode = BlendMode.Alpha;
			var p = wr.Screen3DPxPosition(pos);
			var tl = (float3)wr.Viewport.WorldToViewPx(p);
			var br = (float3)wr.Viewport.WorldToViewPx(p + new float3(10, 10, 10));
			var tr = new float3(br.X, tl.Y, tl.Z);
			var bl = new float3(tl.X, br.Y, br.Z);
			var points = new[] { tl, tr, br, bl };
			var vertices = new Vertex[6];
			// Not a line
			if (points.Length < 2)
				return;

			// Single segment
			if (points.Length == 2)
			{
				Game.Renderer.RgbaColorRenderer.DrawLine(points[0], points[1], width, color, blendMode);
				return;
			}

			color = Util.PremultiplyAlpha(color);
			var r = color.R / 255.0f;
			var g = color.G / 255.0f;
			var b = color.B / 255.0f;
			var a = color.A / 255.0f;

			var start = points[0];
			var end = points[1];
			var dir = (end - start) / (end - start).XY.Length;
			var corner = width / 2 * new float3(-dir.Y, dir.X, dir.Z);

			// Corners for start of line segment
			var ca = start - corner;
			var cb = start + corner;

			// Vertices for the corners joining start-end to end-next
			var cc = end + corner;
			var cd = end - corner;

			var Offset = new float3(0.5f, 0.5f, 0f);
			// Fill segment
			vertices[0] = new Vertex(ca + Offset, r, g, b, a, 0, 0);
			vertices[1] = new Vertex(cb + Offset, r, g, b, a, 0, 0);
			vertices[2] = new Vertex(cc + Offset, r, g, b, a, 0, 0);
			vertices[3] = new Vertex(cc + Offset, r, g, b, a, 0, 0);
			vertices[4] = new Vertex(cd + Offset, r, g, b, a, 0, 0);
			vertices[5] = new Vertex(ca + Offset, r, g, b, a, 0, 0);
			Game.Renderer.SpriteRenderer.DrawRGBAVertices(vertices, blendMode);
		}

		public void RenderDebugGeometry(WorldRenderer wr)
		{
		}

		public Rectangle ScreenBounds(WorldRenderer wr)
		{
			return new Rectangle();
		}
	}
}
