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
			//var wsr = Game.Renderer.WorldPolygonRenderer;

			//wsr.DrawSprite();
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
