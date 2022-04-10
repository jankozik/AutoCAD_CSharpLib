﻿using ACadSharp.Header;
using ACadSharp.IO;
using ACadSharp.IO.DXF;
using ACadSharp.Tests.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace ACadSharp.Tests.IO.DXF
{
	public class DxfWriterTests
	{
		private const string _samplesFolder = "../../../../samples/out";

		protected readonly ITestOutputHelper _output;

		public DxfWriterTests(ITestOutputHelper output)
		{
			this._output = output;
		}

		[Fact]
		public void WriteAsciiTest()
		{
			CadDocument doc = new CadDocument();
			string path = Path.Combine(_samplesFolder, "out_sample.dxf");

			DxfWriter.Write(path, doc, false);
		}

		[Fact]
		public void WriteBinaryTest()
		{
			CadDocument doc = new CadDocument();
			string path = Path.Combine(_samplesFolder, "out_sample.dxf");

			DxfWriter.Write(path, doc, true);
		}
	}
}