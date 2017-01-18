using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Common.Web.UrlState
{
	public interface IPageUrlState
	{
		UrlStateContainer UrlStateContainer { get; }
	}
}
