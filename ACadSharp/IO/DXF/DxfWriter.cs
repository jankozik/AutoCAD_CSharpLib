﻿using System;
using System.IO;
using System.Text;

namespace ACadSharp.IO.DXF
{
	public class DxfWriter : IDisposable
	{
		private CadDocument _document;
		private IDxfStreamWriter _writer;
		private CadObjectHolder _objectHolder = new CadObjectHolder();

		/// <summary>
		/// Initializes a new instance of the <see cref="DxfWriter"/> class.
		/// </summary>
		/// <param name="filename">The file to write into.</param>
		/// <param name="document"></param>
		/// <param name="binary"></param>
		public DxfWriter(string filename, CadDocument document, bool binary)
			: this(File.Create(filename), document, binary)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DxfWriter"/> class.
		/// </summary>
		/// <param name="stream">The stream to write into</param>
		/// <param name="document"></param>
		/// <param name="binary"></param>
		public DxfWriter(Stream stream, CadDocument document, bool binary)
		{
			var encoding = new UTF8Encoding(false);

			if (binary)
			{
				this._writer = new DxfBinaryWriter(new BinaryWriter(stream, encoding));
			}
			else
			{
				this._writer = new DxfAsciiWriter(new StreamWriter(stream, encoding));
			}

			this._document = document;
		}

		/// <summary>
		/// Write the <see cref="CadDocument"/>
		/// </summary>
		public void Write()
		{
			this._objectHolder.Objects.Enqueue(_document.RootDictionary);

			this.writeHeader();

			this.writeDxfClasses();

			this.writeTables();

			this.writeBlocks();

			this.writeEntities();

			this.writeObjects();

			this.writeACDSData();

			this._writer.Write(DxfCode.Start, DxfFileToken.EndOfFile);
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			this._writer.Dispose();
		}

		/// <summary>
		/// Write a <see cref="CadDocument"/> into a file
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="document"></param>
		/// <param name="binary"></param>
		public static void Write(string filename, CadDocument document, bool binary)
		{
			using (DxfWriter writer = new DxfWriter(filename, document, binary))
			{
				writer.Write();
			}
		}

		/// <summary>
		/// Write a <see cref="CadDocument"/> intio a <see cref="Stream"/>
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="document"></param>
		/// <param name="binary"></param>
		public static void Write(Stream stream, CadDocument document, bool binary)
		{
			using (DxfWriter writer = new DxfWriter(stream, document, binary))
			{
				writer.Write();
			}
		}

		private void writeHeader()
		{
			new DxfHeaderSectionWriter(this._writer, this._document, this._objectHolder).Write();
		}

		private void writeDxfClasses()
		{
			new DxfDxfClassesSectionWriter(this._writer, this._document, this._objectHolder).Write();
		}

		private void writeTables()
		{
			new DxfTablesSectionWriter(this._writer, this._document, this._objectHolder).Write();
		}

		private void writeBlocks()
		{
			new DxfBlocksSectionWriter(this._writer, this._document, this._objectHolder).Write();
		}

		private void writeEntities()
		{
			new DxfEntitiesSectionWriter(this._writer, this._document, this._objectHolder).Write();
		}

		private void writeObjects()
		{
			new DxfObjectsSectionWriter(this._writer, this._document, this._objectHolder).Write();
		}

		private void writeACDSData()
		{
		}
	}
}
