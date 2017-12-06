using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.ITSupport
{
	public class EquipAsset
	{
		public int Id { get; set;}
		public string AssetSn { get; set;}
		public Nullable<int> ApplyId { get; set;}
		public string BUsername { get; set;}

	}
}
