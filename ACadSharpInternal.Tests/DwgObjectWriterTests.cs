﻿using ACadSharp;
using ACadSharp.Entities;
using ACadSharp.IO;
using ACadSharp.IO.DWG;
using ACadSharp.Tables;
using ACadSharp.Tables.Collections;
using ACadSharp.Tests.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace ACadSharpInternal.Tests
{
	public class DwgObjectWriterTests : DwgSectionWriterTestBase
	{
		public DwgObjectWriterTests(ITestOutputHelper output) : base(output)
		{
		}

		[Theory]
		[MemberData(nameof(DwgVersions))]
		public void WriteEmptyDocumentTest(ACadVersion version)
		{
			CadDocument document = new CadDocument();
			document.Header.Version = version;

			DwgDocumentBuilder builder = this.writeInfo(document);

			builder.BuildTables();

			assertTable(document.AppIds, builder.AppIds);
			assertTable(document.Layers, builder.Layers);
			assertTable(document.LineTypes, builder.LineTypesTable);
			assertTable(document.TextStyles, builder.TextStyles);
			assertTable(document.UCSs, builder.UCSs);
			assertTable(document.Views, builder.Views);
			assertTable(document.DimensionStyles, builder.DimensionStyles);
			assertTable(document.VPorts, builder.VPorts);
			assertTable(document.BlockRecords, builder.BlockRecords);
		}

		[Theory]
		[MemberData(nameof(DwgVersions))]
		public void EntitiesTest(ACadVersion version)
		{
			CadDocument document = new CadDocument();
			document.Header.Version = version;

			document.Entities.Add(EntityFactory.Create<Arc>());
			document.Entities.Add(EntityFactory.Create<Circle>());
			document.Entities.Add(EntityFactory.Create<Line>());
			document.Entities.Add(EntityFactory.Create<Point>());

			DwgDocumentBuilder builder = this.writeInfo(document);
		}

		private void assertTable<T>(Table<T> expected, Table<T> actual)
			where T : TableEntry
		{
			Assert.NotNull(expected);
			Assert.Equal(expected.Handle, actual.Handle);

			Assert.Equal(expected.Count, actual.Count);

			foreach (T entry in actual)
			{
				Assert.NotNull(expected[entry.Name]);
			}

			foreach (T entry in expected)
			{
				Assert.NotNull(actual[entry.Name]);
			}
		}

		private DwgDocumentBuilder writeInfo(CadDocument docToWrite)
		{
			Stream stream = new MemoryStream();

			DwgObjectWriter writer = new DwgObjectWriter(stream, docToWrite);
			writer.OnNotification += onNotification;
			writer.Write();

			var handles = new Queue<ulong>(writer.Map.Select(o => o.Key));

			CadDocument docResult = new CadDocument(false);
			docResult.Header = new ACadSharp.Header.CadHeader();
			docResult.Header.Version = docToWrite.Header.Version;

			DwgDocumentBuilder builder = new DwgDocumentBuilder(docResult, new ACadSharp.IO.DwgReaderConfiguration());
			IDwgStreamReader sreader = DwgStreamReaderBase.GetStreamHandler(docToWrite.Header.Version, stream, true);
			DwgObjectSectionReader reader = new DwgObjectSectionReader(
				docResult.Header.Version,
				builder,
				sreader,
				handles,
				writer.Map,
				new ACadSharp.Classes.DxfClassCollection()
				);
			reader.Read();

			return builder;
		}

		protected override void onNotification(object sender, NotificationEventArgs e)
		{
			Assert.False(e.NotificationType == NotificationType.NotImplemented);

			base.onNotification(sender, e);
		}
	}
}
