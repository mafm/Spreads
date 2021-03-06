// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.


namespace Spreads.Yeppp
{

	/// <summary>MIPS-specific SIMD extensions.</summary>
	/// <seealso cref="Library.IsSupported(CpuSimdFeature)" />
	public sealed class MipsCpuSimdFeature : CpuSimdFeature
	{

		/// <summary>MDMX instruction set.</summary>
		public static readonly MipsCpuSimdFeature MDMX         = new MipsCpuSimdFeature(0);
		/// <summary>Paired-single instructions.</summary>
		public static readonly MipsCpuSimdFeature PairedSingle = new MipsCpuSimdFeature(1);
		/// <summary>MIPS3D instruction set.</summary>
		public static readonly MipsCpuSimdFeature MIPS3D       = new MipsCpuSimdFeature(2);
		/// <summary>MIPS DSP extension.</summary>
		public static readonly MipsCpuSimdFeature DSP          = new MipsCpuSimdFeature(3);
		/// <summary>MIPS DSP Release 2 extension.</summary>
		public static readonly MipsCpuSimdFeature DSP2         = new MipsCpuSimdFeature(4);
		/// <summary>Loongson (Godson) MMX instruction set.</summary>
		public static readonly MipsCpuSimdFeature GodsonMMX    = new MipsCpuSimdFeature(5);
		/// <summary>Ingenic Media Extension.</summary>
		public static readonly MipsCpuSimdFeature MXU          = new MipsCpuSimdFeature(6);
		/// <summary>Ingenic Media Extension 2.</summary>
		public static readonly MipsCpuSimdFeature MXU2         = new MipsCpuSimdFeature(7);

		internal MipsCpuSimdFeature(uint id) : base(id, CpuArchitecture.MIPS.Id)
		{
		}

	}

}
