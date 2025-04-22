using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ECOM.Shared.Library.Functions.Helpers
{
	public static class CommonHelper
	{
		/// <summary>
		/// Executes an action asynchronously with retry logic. If the action fails, it will be retried 
		/// up to the specified maximum attempt count, with a delay between each retry.
		/// </summary>
		/// <typeparam name="TResult">The type of the result that the action will return.</typeparam>
		/// <param name="action">The asynchronous action to be executed, which returns a <typeparamref name="TResult"/>.</param>
		/// <param name="retryInterval">The interval between retry attempts in case of failure.</param>
		/// <param name="maxAttemptCount">The maximum number of retry attempts. Default is 3.</param>
		/// <returns>The result of the action if successful.</returns>
		/// <exception cref="AggregateException">Thrown if all retry attempts fail. Contains the list of exceptions thrown during each attempt.</exception>
		public static async Task<TResult> RetryAsync<TResult>(Func<Task<TResult>> action, TimeSpan retryInterval, int maxAttemptCount = 3)
		{
			var exceptions = new List<Exception>();

			for (int attempted = 0; attempted < maxAttemptCount; attempted++)
			{
				try
				{
					if (attempted > 0)
					{
						Task.Delay(retryInterval).Wait();
					}
					return await action();
				}
				catch (Exception ex)
				{
					exceptions.Add(ex);
				}
			}
			throw new AggregateException(exceptions);
		}

		/// <summary>
		/// Generates a sequential GUID by combining the current UTC timestamp with random bytes.
		/// This approach improves database indexing performance by reducing fragmentation,
		/// as the GUIDs are partially ordered based on creation time.
		/// </summary>
		/// <returns>A sequentially structured <see cref="Guid"/>.</returns>
		public static Guid GenerateSequenceGuid()
		{
			var randomBytes = new byte[10];
			RandomNumberGenerator.Fill(randomBytes);

			var timestamp = BitConverter.GetBytes(DateTime.UtcNow.Ticks);

			var guidBytes = new byte[16];
			Buffer.BlockCopy(timestamp, 2, guidBytes, 0, 6);
			Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);

			return new Guid(guidBytes);
		}
	}
}
