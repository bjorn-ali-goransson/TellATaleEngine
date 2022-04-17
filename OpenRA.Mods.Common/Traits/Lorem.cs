using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenRA.Traits;

namespace OpenRA.Mods.Common.Traits
{
	public class LoremInfo : TraitInfo
	{
		public override object Create(ActorInitializer init)
		{
			return new Lorem(init);
		}
	}

	public class Lorem : IOccupySpace
	{
		readonly Actor self;

		public Lorem(ActorInitializer init)
		{
			this.self = init.Self;
			var topLeft = init.GetValue<LocationInit, CPos>();
			CenterPosition = init.World.Map.CenterOfCell(topLeft);
		}

		public WPos CenterPosition { get; private set; }

		public CPos TopLeft => self.World.Map.CellContaining(CenterPosition);

		public (CPos Cell, SubCell SubCell)[] OccupiedCells()
		{
			return new[] { (TopLeft, SubCell.FullCell) };
		}
	}
}
