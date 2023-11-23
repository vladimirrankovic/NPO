using System;

namespace BinderClientCSharp
{
	public interface IWorkerHandler
	{
		void run(IWorkerConnector workerConnector);
	}
}

