using N2.Web;
using N2.Engine;

namespace N2.Tests.Web.Items
{
	[Adapts(typeof(CustomItem))]
	public class CustomRequestAdapter : RequestAdapter
	{
	}
}