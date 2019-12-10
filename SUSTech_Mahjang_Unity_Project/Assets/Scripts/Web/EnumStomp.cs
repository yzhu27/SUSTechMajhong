using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Web
{
	public enum ClientCommand
	{
		SEND,
		SUBSCRIBE,
		UNSUBSCRIBE,
		BEGIN,
		COMMIT,
		ABORT,
		ACK,
		CONNECT,
		DISCONNECT
	}
	public enum ServerCommand
	{
		CONNECTED,
		MESSAGE,
		RECEIPT,
		ERROR
	}
}
