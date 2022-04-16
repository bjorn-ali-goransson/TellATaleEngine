using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenRA.Graphics;
using OpenRA.Primitives;
using OpenRA.Traits;

namespace OpenRA.Mods.Common.Traits.Render
{
	public class RenderPolygonInfo : TraitInfo
	{
		public override object Create(ActorInitializer init)
		{
			return new RenderPolygon();
		}
	}

	public class RenderPolygon : IRender
	{
		public IEnumerable<IRenderable> Render(Actor self, WorldRenderer wr)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Rectangle> ScreenBounds(Actor self, WorldRenderer wr)
		{
			throw new NotImplementedException();
		}
	}
}
