﻿using System.IO;

namespace uTinyRipper.Classes.Shaders.Exporters
{
	public class ShaderMetalExporter : ShaderTextExporter
	{
		public override void Export(ShaderWriter writer, ref ShaderSubProgram subProgram)
		{
			using (MemoryStream memStream = new MemoryStream(subProgram.ProgramData))
			{
				using (BinaryReader reader = new BinaryReader(memStream))
				{
					if (Shader.IsEncoded(writer.Version))
					{
						long position = reader.BaseStream.Position;
						uint fourCC = reader.ReadUInt32();
						if (fourCC == MetalFourCC)
						{
							int offset = reader.ReadInt32();
							reader.BaseStream.Position = position + offset;
						}
						using (EndianReader endReader = new EndianReader(reader.BaseStream))
						{
							EntryName = endReader.ReadStringZeroTerm();
						}
					}

					ExportText(writer, reader);
				}
			}
		}

		public string EntryName { get; private set; }

		private const uint MetalFourCC = 0xf00dcafe;
	}
}
