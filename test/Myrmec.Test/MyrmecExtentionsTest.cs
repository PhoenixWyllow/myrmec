﻿// <copyright file="MyrmecExtentionsTest.cs" company="Rocket Robin">
// Copyright (c) Rocket Robin. All rights reserved.
// Licensed under the Apache v2 license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Myrmec.Test
{
	[TestClass]
	public class MyrmecExtentionsTest
	{
		[TestMethod]
		public void ComplexFileTypeTest()
		{
			List<Metadata> list = new List<Metadata>();
			Record record = new Record()
			{
				Extentions = "a,b,c",
				Hex = "0x11 0x22 ?? ?? ?? 0x33",
				Offset = 2
			};

			list.Add(record);
			byte[] data = new byte[]
			{
				0x11, 0x11, 0x11, 0x22, 0xff, 0xdd, 0x1d, 0x33
			};
			bool result = list.First().Match(data);

			Assert.IsTrue(result);
		}
	}
}
