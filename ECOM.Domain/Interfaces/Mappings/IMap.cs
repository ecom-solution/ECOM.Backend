namespace ECOM.Domain.Interfaces.Mappings
{
	public interface IMap
	{
		/// <summary>
		/// Maps a source object to a destination type.
		/// </summary>
		/// <typeparam name="TDestination">The destination type.</typeparam>
		/// <param name="source">The source object to map.</param>
		/// <returns>The mapped object of the destination type.</returns>
		TDestination Map<TDestination>(object source);

		/// <summary>
		/// Maps values from the source object into an existing destination object.
		/// </summary>
		/// <typeparam name="TSource">The source type.</typeparam>
		/// <typeparam name="TDestination">The destination type.</typeparam>
		/// <param name="source">The source object.</param>
		/// <param name="destination">The existing destination object to update.</param>
		/// <returns>The updated destination object.</returns>
		TDestination Map<TSource, TDestination>(TSource source, TDestination destination);

		/// <summary>
		/// Maps a collection of source objects to a collection of destination objects.
		/// </summary>
		/// <typeparam name="TSource">The source type.</typeparam>
		/// <typeparam name="TDestination">The destination type.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>The mapped collection of destination objects.</returns>
		IEnumerable<TDestination> MapList<TSource, TDestination>(IEnumerable<TSource> source);
	}
}
