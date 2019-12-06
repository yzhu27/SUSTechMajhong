using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GameMain
{
	/// <summary>
	/// 特殊位，位于000X0000
	/// </summary>
	public enum Special
	{
		/// <summary>
		/// 普通牌
		/// </summary>
		None = 0xf,
		/// <summary>
		/// 癞子
		/// </summary>
		King = 0x0,
		/// <summary>
		/// 校徽
		/// </summary>
		Logo = 0x1,
		/// <summary>
		/// 签到
		/// </summary>
		Sign = 0x2,
		/// <summary>
		/// 园
		/// </summary>
		Land = 0x3
	}

	/// <summary>
	/// 园编码，与sequence共同位于000000X0
	/// </summary>
	public enum Land
	{
		/// <summary>
		/// 欣园
		/// </summary>
		Xin = 0x0,
		/// <summary>
		/// 荔园
		/// </summary>
		Lee = 0x1,
		/// <summary>
		/// 慧园
		/// </summary>
		Hui = 0x2,
		/// <summary>
		/// 创园
		/// </summary>
		Cng = 0x3,
		/// <summary>
		/// 智园
		/// </summary>
		Zhi = 0x4
	}

	/// <summary>
	/// 系编码，位于0000XX00
	/// </summary>
	public enum Department
	{
		// 理学院
		Math = 0x00,	// 数学系
		Phy = 0x01,     // 物理系
		Chem = 0x02,	// 化学系
		Bio = 0x03,     // 生物系
		Ess = 0x04,     // 地球与空间科学系
		StatDs = 0x05,  // 统计与数据科学系
		// 工学院
		Eee = 0x20,		// 电子与电气工程系
		Mse = 0x21,		// 材料科学与工程系
		Ocean = 0x22,	// 海洋科学与工程系
		Cse = 0x23,		// 计算机科学与工程系
		Ese = 0x24,		// 环境科学与工程学院
		Mae = 0x25,		// 力学与航天工程系
		Mee = 0x26,		// 机械与能源工程系
		Bme = 0x27,		// 生物医学工程系
		Sme = 0x28,		// 深港微电子学院
		Sdim = 0x29,	// 系统设计与智能制造学院
		// 商学院
		Fin = 0x40,		// 金融系
		// 医学院
		Med = 0x60		// 医学院
	}

	/// <summary>
	/// bit to shift
	/// </summary>
	public enum Location
	{
		Unique = 0,
		Reserve1 = 2,
		Sequence = 4,
		Land = Sequence,
		Department = 8,
		Special = 16,
		Reserve0 = 20,
		Zero = 24
	}
}
