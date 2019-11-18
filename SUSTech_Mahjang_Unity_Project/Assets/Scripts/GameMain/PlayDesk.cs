
namespace Assets.Scripts.GameMain
{
	/// <summary>
	/// 游戏内的主要物件, 每个<c>PlayDesk</c>包含四个<c>Player</c>对象
	/// </summary>
	public class PlayDesk
	{
		/// <summary>
		/// 自己
		/// </summary>
		public MainPlayer self;

		/// <summary>
		/// 上家
		/// </summary>
		public Player last;

		/// <summary>
		/// 对家
		/// </summary>
		public Player opposite;

		/// <summary>
		/// 下家
		/// </summary>
		public Player next;

		/// <summary>
		/// 每回合操作时限, 单位: 秒
		/// </summary>
		public float roundTimeOut;

		/// <summary>
		/// 每次响应的时限(如碰, 吃等)
		/// </summary>
		public float responseTimeOut;

		/// <summary>
		/// 当前进行回合的玩家编号, 0:自己, 1:上家, 2:对家, 3:下家
		/// </summary>
		public int roundPlayer;

		/// <summary>
		/// 当前进行响应的玩家编号, 0:自己, 1:上家, 2:对家, 3:下家
		/// </summary>
		public int[] responsePlayer;

		public PlayDesk() { }

		/// <summary>
		/// 开始一名玩家的回合
		/// </summary>
		/// <param name="player">0:自己, 1:上家, 2:对家, 3:下家</param>
		public void StartRound(int player) { }

		/// <summary>
		/// 开始响应
		/// </summary>
		/// <param name="players">0:自己, 1:上家, 2:对家, 3:下家</param>
		public void StartResponse(int[] players) { }
	}
}
