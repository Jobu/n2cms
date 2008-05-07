﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using N2.Engine;
using N2.Persistence.Finder;
using N2.Tests;

using N2.Linq;

namespace N2.Extensions.Tests.Linq
{
	[TestFixture]
	public class QueriableTests : PersistenceAwareBase
	{
		[TestFixtureSetUp]
		public override void TestFixtureSetUp()
		{
			base.TestFixtureSetUp();
			CreateDatabaseSchema();

			LinqItem root = CreateOneItem<LinqItem>(0, "root", null);
			root.StringProperty = "a string";
			engine.Persister.Save(root);
		}

		[Test]
		public void CanSelectAllItems()
		{
			var query = from ci in engine.Database().ContentItems
						select ci;

			EnumerableAssert.Count(1, query);
		}

		[Test]
		public void CanSelectAllItems_WithWhere()
		{
			var query = from ci in engine.Database().ContentItems
						where ci.Name == "root"
						select ci;

			EnumerableAssert.Count(1, query);
		}
	}
}